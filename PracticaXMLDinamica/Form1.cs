using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.Linq;

namespace PracticaXMLDinamica
{
    // =============================================================
    // 🎨 TABLA DE COLORES PERSONALIZADA PARA EL MENUSTRIP
    // =============================================================
    public class MyColorTable : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => Color.FromArgb(48, 48, 48);
        public override Color MenuItemSelected => Color.FromArgb(63, 63, 63);
        public override Color MenuBorder => Color.FromArgb(50, 50, 50);

        // Fondo del margen donde irían iconos (no usamos, pero unificado)
        public override Color ImageMarginGradientBegin => Color.FromArgb(48, 48, 48);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(48, 48, 48);
        public override Color ImageMarginGradientEnd => Color.FromArgb(48, 48, 48);
    }

    // =============================================================
    // 🎨 RENDERER QUE APLICA LOS COLORES AL MENUSTRIP
    // =============================================================
    public class ForcefulToolStripRenderer : ToolStripProfessionalRenderer
    {
        public ForcefulToolStripRenderer() : base(new MyColorTable()) { }

        // Texto blanco para menú y submenús
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White;
            base.OnRenderItemText(e);
        }
    }


    // =============================================================
    // 🏠 FORM PRINCIPAL (GESTIÓN DEL CATÁLOGO)
    // =============================================================
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(880, 620);

            // Cargar interfaz desde XML
            CargarInterfazDesdeXML();

            // Centrados y limpieza al iniciar
            this.Shown += (s, e) =>
            {
                CentrarTitulo();
                CentrarBotones();
                LimpiarCampos();

                var dgv = this.Controls.Find("tablaDatos", true).FirstOrDefault() as DataGridView;
                dgv?.ClearSelection();
            };
        }

        // =============================================================
        // 📌 CREACIÓN DEL MENU SUPERIOR
        // =============================================================
        private void CargarMenu(XmlNode menuNode)
        {
            if (menuNode == null) return;

            MenuStrip menu = new MenuStrip();
            menu.Renderer = new ForcefulToolStripRenderer();
            menu.BackColor = Color.FromArgb(50, 50, 50);
            menu.ForeColor = Color.White;
            menu.Dock = DockStyle.Top;

            // Padding general del menú (estilo Steam)
            menu.Padding = new Padding(5, 2, 0, 2);

            // Recorrer los items definidos en el XML
            foreach (XmlNode itemNode in menuNode.SelectNodes("MenuItem"))
            {
                string texto = itemNode.Attributes["texto"].Value;

                // Inserta un separador invisible ANTES de "Mi perfil"
                if (texto.Contains("Mi perfil"))
                {
                    ToolStripLabel separadorFlexible = new ToolStripLabel();
                    separadorFlexible.AutoSize = false;

                    // Ajusta la separación lateral del menú
                    separadorFlexible.Width = 190;

                    menu.Items.Add(separadorFlexible);
                }

                // Crear el item
                ToolStripMenuItem menuItem = CrearMenuItem(itemNode);

                // Evitar que el desplegable sobresalga por la derecha
                if (menuItem.Text.Contains("Mi perfil"))
                {
                    menuItem.DropDownOpening += MiPerfilToolStripMenuItem_DropDownOpening;
                }

                menu.Items.Add(menuItem);
            }

            this.Controls.Add(menu);
        }

        // Evita que el menú desplegable salga fuera del formulario
        private void MiPerfilToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem?.DropDown == null) return;

            ToolStripDropDown dropDown = menuItem.DropDown;

            Point screenLocation = this.PointToScreen(new Point(0, 0));
            int formRightEdge = screenLocation.X + this.Width;

            if (dropDown.Right > formRightEdge)
            {
                dropDown.Left = formRightEdge - dropDown.Width;
            }
        }

        // Crea cada item del menú a partir del XML
        private ToolStripMenuItem CrearMenuItem(XmlNode nodo)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(nodo.Attributes["texto"].Value);
            item.ForeColor = Color.White;
            item.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);

            // Submenús
            foreach (XmlNode hijo in nodo.SelectNodes("MenuItem"))
            {
                ToolStripMenuItem subItem = CrearMenuItem(hijo);
                subItem.Font = new Font("Segoe UI", 9F);
                item.DropDownItems.Add(subItem);
            }

            // Acciones
            if (nodo.Attributes["accion"] != null)
            {
                string accion = nodo.Attributes["accion"].Value;

                item.Click += (s, e) =>
                {
                    switch (accion)
                    {
                        case "salir": Application.Exit(); break;
                        case "guardar": MessageBox.Show("Cambios guardados ✔"); break;
                        case "acerca": MessageBox.Show("Aplicación creada por Pablo 😉"); break;
                        case "logout":
                            FormLogin login = new FormLogin();
                            login.Show();
                            this.Hide();
                            break;
                        case "exitApp": Application.Exit(); break;
                        case "nav": MessageBox.Show($"Has pulsado: {item.Text}"); break;
                    }
                };
            }

            return item;
        }

        // =============================================================
        // 🎨 FONDO EN DEGRADADO OSCURO
        // =============================================================
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var grad = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(20, 20, 20),
                Color.FromArgb(70, 70, 70),
                90f))
            {
                e.Graphics.FillRectangle(grad, this.ClientRectangle);
            }
        }

        // =============================================================
        // 📌 CARGA UI DESDE XML (PANELES Y CONTROLES)
        // =============================================================
        private void CargarInterfazDesdeXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Interfaz.xml");

            CargarMenu(doc.SelectSingleNode("/Interfaz/MenuStrip"));

            foreach (XmlNode nodo in doc.SelectNodes("/Interfaz/Panel"))
                CrearPanel(nodo);

            XmlNode tablaNode = doc.SelectSingleNode("/Interfaz/DataGrid");
            if (tablaNode != null) CrearTabla(tablaNode);
        }

        // Crear panel dinámico
        private void CrearPanel(XmlNode nodo)
        {
            Panel p = new Panel
            {
                Name = nodo.Attributes["nombre"].Value,
                Left = int.Parse(nodo.Attributes["x"].Value),
                Top = int.Parse(nodo.Attributes["y"].Value),
                Width = int.Parse(nodo.Attributes["width"].Value),
                Height = int.Parse(nodo.Attributes["height"].Value),
                BackColor = ColorTranslator.FromHtml(nodo.Attributes["backColor"].Value)
            };

            foreach (XmlNode hijo in nodo.SelectNodes("Control"))
                CrearControl(hijo, p);

            this.Controls.Add(p);
        }

        // Crear controles (Label, Button, TextBox)
        private void CrearControl(XmlNode nodo, Control contenedor)
        {
            string tipo = nodo.Attributes["tipo"].Value;
            Control control;

            switch (tipo)
            {
                case "Label":
                    control = new Label
                    {
                        ForeColor = Color.White,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = nodo.Attributes["nombre"].Value == "lblTitulo"
                            ? new Font("Segoe UI", 14F, FontStyle.Bold)
                            : new Font("Segoe UI", 11, FontStyle.Bold)
                    };
                    break;

                case "Button":
                    Button btn = new Button
                    {
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                        BackColor = Color.FromArgb(200, 16, 46),
                        ForeColor = Color.White
                    };
                    btn.FlatAppearance.BorderColor = Color.FromArgb(0, 56, 168);
                    btn.FlatAppearance.BorderSize = 2;

                    btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 56, 168);
                    btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(200, 16, 46);

                    btn.Click += (s, e) =>
                    {
                        switch (nodo.Attributes["nombre"].Value)
                        {
                            case "btnAñadir": AñadirJuego(); break;
                            case "btnEditar": ModificarJuego(); break;
                            case "btnEliminar": EliminarJuego(); break;
                        }
                    };

                    control = btn;
                    break;

                case "TextBox":
                    control = new TextBox
                    {
                        BackColor = Color.FromArgb(240, 240, 240),
                        ForeColor = Color.Black,
                        Font = new Font("Segoe UI", 9.5F)
                    };
                    break;

                default: return;
            }

            control.Name = nodo.Attributes["nombre"].Value;
            control.Text = nodo.Attributes["texto"].Value;
            control.Left = int.Parse(nodo.Attributes["x"].Value);
            control.Top = int.Parse(nodo.Attributes["y"].Value);
            control.Width = int.Parse(nodo.Attributes["width"].Value);
            control.Height = int.Parse(nodo.Attributes["height"].Value);

            contenedor.Controls.Add(control);
        }

        // =============================================================
        // 🎯 CENTRADO DE TITULO Y BOTONES
        // =============================================================
        private void CentrarTitulo()
        {
            var panel = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;
            var lblTitulo = panel?.Controls.Find("lblTitulo", true).FirstOrDefault() as Label;

            if (panel != null && lblTitulo != null)
                lblTitulo.Left = (panel.ClientSize.Width - lblTitulo.Width) / 2 + 60;
        }

        private void CentrarBotones()
        {
            var panel = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;
            if (panel == null) return;

            var btn1 = panel.Controls.Find("btnAñadir", true).FirstOrDefault() as Button;
            var btn2 = panel.Controls.Find("btnEditar", true).FirstOrDefault() as Button;
            var btn3 = panel.Controls.Find("btnEliminar", true).FirstOrDefault() as Button;

            if (btn1 == null || btn2 == null || btn3 == null) return;

            int espacio = 15;
            int total = btn1.Width + btn2.Width + btn3.Width + (espacio * 2);
            int startX = (panel.ClientSize.Width - total) / 2;
            int y = btn1.Top;

            btn1.Left = startX;
            btn2.Left = btn1.Right + espacio;
            btn3.Left = btn2.Right + espacio;
        }

        // =============================================================
        // 📊 TABLA DE JUEGOS
        // =============================================================
        private void CrearTabla(XmlNode tablaNode)
        {
            DataGridView dgv = new DataGridView
            {
                Name = tablaNode.Attributes["nombre"].Value,
                Left = int.Parse(tablaNode.Attributes["x"].Value),
                Top = int.Parse(tablaNode.Attributes["y"].Value),
                Width = int.Parse(tablaNode.Attributes["width"].Value),
                Height = int.Parse(tablaNode.Attributes["height"].Value),
                BackgroundColor = Color.FromArgb(60, 63, 65),
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                EnableHeadersVisualStyles = false
            };

            // Cabecera
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Filas
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(60, 63, 65);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 56, 168);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            // Columnas desde XML
            foreach (XmlNode col in tablaNode.SelectNodes("Columna"))
            {
                dgv.Columns.Add(col.Attributes["nombre"].Value, col.Attributes["nombre"].Value);
                dgv.Columns[col.Attributes["nombre"].Value].Width = int.Parse(col.Attributes["width"].Value);
            }

            dgv.SelectionChanged += Dgv_SelectionChanged;

            CargarCatalogoEnTabla(dgv);

            this.Controls.Add(dgv);
        }

        // Rellenar textboxes cuando el usuario selecciona una fila
        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.SelectedRows.Count == 0) return;

            DataGridViewRow row = dgv.SelectedRows[0];
            var p = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;

            (p.Controls.Find("txtTitulo", true).FirstOrDefault() as TextBox).Text = row.Cells[1].Value.ToString();
            (p.Controls.Find("txtDesarrollador", true).FirstOrDefault() as TextBox).Text = row.Cells[2].Value.ToString();
            (p.Controls.Find("txtPlataforma", true).FirstOrDefault() as TextBox).Text = row.Cells[3].Value.ToString();
            (p.Controls.Find("txtPrecio", true).FirstOrDefault() as TextBox).Text = row.Cells[4].Value.ToString();
        }

        // =============================================================
        // ✔ VALIDACIÓN Y CRUD XML
        // =============================================================
        private bool ValidarCampos()
        {
            var p = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;

            TextBox[] campos =
            {
                p.Controls.Find("txtTitulo", true).FirstOrDefault() as TextBox,
                p.Controls.Find("txtDesarrollador", true).FirstOrDefault() as TextBox,
                p.Controls.Find("txtPlataforma", true).FirstOrDefault() as TextBox,
                p.Controls.Find("txtPrecio", true).FirstOrDefault() as TextBox
            };

            bool valido = true;

            // Validación vacíos
            foreach (TextBox t in campos)
            {
                if (string.IsNullOrWhiteSpace(t.Text))
                {
                    t.BackColor = Color.FromArgb(255, 170, 170);
                    valido = false;
                }
                else
                {
                    t.BackColor = Color.FromArgb(240, 240, 240);
                }
            }

            // Validación numérica
            if (!decimal.TryParse(campos[3].Text, out _))
            {
                campos[3].BackColor = Color.FromArgb(255, 170, 170);
                valido = false;
            }

            return valido;
        }

        private void LimpiarCampos()
        {
            var p = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;

            (p.Controls.Find("txtTitulo", true).FirstOrDefault() as TextBox).Clear();
            (p.Controls.Find("txtDesarrollador", true).FirstOrDefault() as TextBox).Clear();
            (p.Controls.Find("txtPlataforma", true).FirstOrDefault() as TextBox).Clear();
            (p.Controls.Find("txtPrecio", true).FirstOrDefault() as TextBox).Clear();
        }

        private void AñadirJuego()
        {
            if (!ValidarCampos())
            {
                MessageBox.Show("Debe rellenar todos los campos correctamente.");
                return;
            }

            var p = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;

            XmlDocument doc = new XmlDocument();
            doc.Load("catalogo.xml");

            XmlNode root = doc.SelectSingleNode("/Catalogo");

            XmlElement juego = doc.CreateElement("Videojuego");

            juego.AppendChild(CrearNodo(doc, "Titulo", (p.Controls.Find("txtTitulo", true).FirstOrDefault() as TextBox).Text));
            juego.AppendChild(CrearNodo(doc, "Desarrollador", (p.Controls.Find("txtDesarrollador", true).FirstOrDefault() as TextBox).Text));
            juego.AppendChild(CrearNodo(doc, "Plataforma", (p.Controls.Find("txtPlataforma", true).FirstOrDefault() as TextBox).Text));
            juego.AppendChild(CrearNodo(doc, "Precio", (p.Controls.Find("txtPrecio", true).FirstOrDefault() as TextBox).Text));

            root.AppendChild(juego);
            doc.Save("catalogo.xml");

            CargarCatalogoEnTabla((DataGridView)this.Controls.Find("tablaDatos", true).FirstOrDefault());
            LimpiarCampos();

            MessageBox.Show("Juego añadido correctamente ✔");
        }

        private void ModificarJuego()
        {
            var tabla = (DataGridView)this.Controls.Find("tablaDatos", true).FirstOrDefault();

            if (tabla.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un juego para modificar.");
                return;
            }

            if (!ValidarCampos())
            {
                MessageBox.Show("Debe rellenar todos los campos correctamente.");
                return;
            }

            int index = tabla.SelectedRows[0].Index;

            var p = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;

            XmlDocument doc = new XmlDocument();
            doc.Load("catalogo.xml");

            XmlNodeList juegos = doc.SelectNodes("/Catalogo/Videojuego");

            juegos[index]["Titulo"].InnerText = (p.Controls.Find("txtTitulo", true).FirstOrDefault() as TextBox).Text;
            juegos[index]["Desarrollador"].InnerText = (p.Controls.Find("txtDesarrollador", true).FirstOrDefault() as TextBox).Text;
            juegos[index]["Plataforma"].InnerText = (p.Controls.Find("txtPlataforma", true).FirstOrDefault() as TextBox).Text;
            juegos[index]["Precio"].InnerText = (p.Controls.Find("txtPrecio", true).FirstOrDefault() as TextBox).Text;

            doc.Save("catalogo.xml");

            CargarCatalogoEnTabla(tabla);
            LimpiarCampos();

            MessageBox.Show("Juego modificado correctamente ✔");
        }

        private void EliminarJuego()
        {
            var tabla = (DataGridView)this.Controls.Find("tablaDatos", true).FirstOrDefault();

            if (tabla.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un juego para eliminar.");
                return;
            }

            DialogResult r = MessageBox.Show(
                "¿Seguro que desea eliminar este juego?",
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (r != DialogResult.Yes) return;

            int index = tabla.SelectedRows[0].Index;

            XmlDocument doc = new XmlDocument();
            doc.Load("catalogo.xml");

            XmlNodeList juegos = doc.SelectNodes("/Catalogo/Videojuego");
            juegos[index].ParentNode.RemoveChild(juegos[index]);

            doc.Save("catalogo.xml");

            CargarCatalogoEnTabla(tabla);
            LimpiarCampos();

            MessageBox.Show("Juego eliminado ✔");
        }

        // Crear nodo XML
        private XmlElement CrearNodo(XmlDocument doc, string nombre, string valor)
        {
            XmlElement nodo = doc.CreateElement(nombre);
            nodo.InnerText = valor;
            return nodo;
        }

        // Carga catálogo en tabla
        private void CargarCatalogoEnTabla(DataGridView dgv)
        {
            dgv.Rows.Clear();

            XmlDocument doc = new XmlDocument();
            doc.Load("catalogo.xml");

            XmlNodeList juegos = doc.SelectNodes("/Catalogo/Videojuego");

            int idAuto = 1;

            foreach (XmlNode juego in juegos)
            {
                dgv.Rows.Add(
                    idAuto,
                    juego["Titulo"].InnerText,
                    juego["Desarrollador"].InnerText,
                    juego["Plataforma"].InnerText,
                    juego["Precio"].InnerText);

                idAuto++;
            }

            if (dgv.Rows.Count > 0)
                dgv.ClearSelection();
        }
    }
}
