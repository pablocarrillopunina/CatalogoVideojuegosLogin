using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace PracticaXMLDinamica
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(750, 550);

            CargarInterfazDesdeXML();
        }

        // ============================
        // 🎨 FONDO DEGRADADO MODERNO
        // ============================
        protected override void OnPaint(PaintEventArgs e)
        {
            // 🔥 ESTA LÍNEA SOLUCIONA EL PROBLEMA
            base.OnPaint(e);

            LinearGradientBrush grad = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(30, 100, 220),
                Color.FromArgb(150, 200, 255),
                90f
            );

            e.Graphics.FillRectangle(grad, this.ClientRectangle);
        }


        // ============================
        // 📌 CARGAR INTERFAZ DESDE XML
        // ============================
        private void CargarInterfazDesdeXML()
        {
            string rutaXML = "Interfaz.xml";

            try
            {
                if (!System.IO.File.Exists(rutaXML))
                {
                    MessageBox.Show("No se encontró Interfaz.xml", "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(rutaXML);

                // === 1) Cargar menú ===
                CargarMenu(doc.SelectSingleNode("/Interfaz/MenuStrip"));

                // === 2) Controles sueltos (NO los de dentro de paneles) ===
                XmlNodeList listaControles = doc.SelectNodes("/Interfaz/Control");
                foreach (XmlNode nodo in listaControles)
                    CrearControl(nodo, this);

                // === 3) Paneles (Contenedores) ===
                XmlNodeList paneles = doc.SelectNodes("/Interfaz/Panel");
                foreach (XmlNode panelNode in paneles)
                    CrearPanel(panelNode);

                // === 4) DataGridView ===
                XmlNode tablaNode = doc.SelectSingleNode("/Interfaz/DataGrid");
                if (tablaNode != null)
                    CrearTabla(tablaNode);

                CentrarTitulo();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar XML:\n" + ex.Message);
            }
        }

        // ============================
        // 📌 CREAR MENUSTRIP
        // ============================
        private void CargarMenu(XmlNode menuNode)
        {
            if (menuNode == null) return;

            MenuStrip menu = new MenuStrip();

            foreach (XmlNode itemNode in menuNode.SelectNodes("MenuItem"))
            {
                ToolStripMenuItem item = CrearMenuItem(itemNode);
                menu.Items.Add(item);
            }

            this.Controls.Add(menu);
        }

        private ToolStripMenuItem CrearMenuItem(XmlNode nodo)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = nodo.Attributes["texto"].Value;

            // Submenú
            foreach (XmlNode hijo in nodo.SelectNodes("MenuItem"))
            {
                item.DropDownItems.Add(CrearMenuItem(hijo));
            }

            // Acción opcional del XML
            if (nodo.Attributes["accion"] != null)
            {
                string accion = nodo.Attributes["accion"].Value;

                item.Click += (s, e) =>
                {
                    if (accion == "salir") Application.Exit();
                    if (accion == "guardar") MessageBox.Show("Guardado!");
                    if (accion == "acerca") MessageBox.Show("Hecho por Pablo 😉");
                };
            }

            return item;
        }

        // ============================
        // 📦 CREAR PANEL + HIJOS
        // ============================
        private void CrearPanel(XmlNode nodo)
        {
            Panel p = new Panel();

            p.Name = nodo.Attributes["nombre"].Value;
            p.Left = int.Parse(nodo.Attributes["x"].Value);
            p.Top = int.Parse(nodo.Attributes["y"].Value);
            p.Width = int.Parse(nodo.Attributes["width"].Value);
            p.Height = int.Parse(nodo.Attributes["height"].Value);

            if (nodo.Attributes["backColor"] != null)
                p.BackColor = ColorTranslator.FromHtml(nodo.Attributes["backColor"].Value);

            // Agregar controles HIJOS del panel
            foreach (XmlNode hijo in nodo.SelectNodes("Control"))
                CrearControl(hijo, p);

            this.Controls.Add(p);
        }

        // ============================
        // 🎛 CREAR CONTROL NORMAL
        // ============================
        private void CrearControl(XmlNode nodo, Control contenedor)
        {
            string tipo = nodo.Attributes["tipo"].Value;
            Control control = null;

            switch (tipo)
            {
                case "Label":
                    control = new Label()
                    {
                        ForeColor = Color.White,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    break;

                case "Button":
                    Button b = new Button();
                    b.FlatStyle = FlatStyle.Flat;
                    b.BackColor = Color.White;
                    b.ForeColor = Color.Black;

                    // Hover
                    b.MouseEnter += (s, e) => b.BackColor = Color.LightBlue;
                    b.MouseLeave += (s, e) => b.BackColor = Color.White;

                    // Evento CLICK
                    b.Click += (s, e) =>
                    {
                        string nombre = nodo.Attributes["nombre"].Value;
                        MessageBox.Show("Has pulsado " + b.Text);

                        // 👉 Obtener la tabla para pasarla al formulario
                        DataGridView tabla = this.Controls["tablaDatos"] as DataGridView;

                        if (tabla == null)
                        {
                            MessageBox.Show("No se encontró la tabla 'tablaDatos' en el formulario.");
                            return;
                        }

                        // 👉 ABRIR FORMULARIOS CRUD PASANDO LA TABLA
                        if (nombre == "btnAñadir")
                            new FormAñadir(tabla).ShowDialog();

                        else if (nombre == "btnEditar")
                            new FormEditar(tabla).ShowDialog();

                        else if (nombre == "btnEliminar")
                            new FormEliminar(tabla).ShowDialog();
                        // Extensiones panel
                        else if (nombre == "btnExportar")
                            MessageBox.Show("Exportando datos...");

                        else if (nombre == "btnImportar")
                            MessageBox.Show("Importando datos...");
                    };

                    control = b;
                    break;

                case "TextBox":
                    control = new TextBox();
                    break;
            }

            control.Name = nodo.Attributes["nombre"].Value;
            control.Text = nodo.Attributes["texto"].Value;
            control.Left = int.Parse(nodo.Attributes["x"].Value);
            control.Top = int.Parse(nodo.Attributes["y"].Value);
            control.Width = int.Parse(nodo.Attributes["width"].Value);
            control.Height = int.Parse(nodo.Attributes["height"].Value);

            // Color opcional desde XML
            if (nodo.Attributes["foreColor"] != null)
                control.ForeColor = ColorTranslator.FromHtml(nodo.Attributes["foreColor"].Value);

            contenedor.Controls.Add(control);
        }

        // ============================
        // 📊 CREAR DATAGRIDVIEW
        // ============================
        private void CrearTabla(XmlNode tablaNode)
        {
            DataGridView dgv = new DataGridView();

            dgv.Name = tablaNode.Attributes["nombre"].Value;
            dgv.Left = int.Parse(tablaNode.Attributes["x"].Value);
            dgv.Top = int.Parse(tablaNode.Attributes["y"].Value);
            dgv.Width = int.Parse(tablaNode.Attributes["width"].Value);
            dgv.Height = int.Parse(tablaNode.Attributes["height"].Value);

            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Columnas
            foreach (XmlNode col in tablaNode.SelectNodes("Columna"))
            {
                dgv.Columns.Add(col.Attributes["nombre"].Value,
                                col.Attributes["nombre"].Value);

                dgv.Columns[col.Attributes["nombre"].Value].Width =
                    int.Parse(col.Attributes["width"].Value);
            }

            // Filas
            foreach (XmlNode fila in tablaNode.SelectNodes("Fila"))
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgv);

                int i = 0;
                foreach (XmlNode celda in fila.SelectNodes("Celda"))
                {
                    row.Cells[i].Value = celda.InnerText;
                    i++;
                }

                dgv.Rows.Add(row);
            }

            this.Controls.Add(dgv);
        }

        // ============================
        // 🔥 CENTRAR TITULO
        // ============================
        private void CentrarTitulo()
        {
            Control titulo = this.Controls["lblTitulo"];
            if (titulo != null)
                titulo.Left = (this.ClientSize.Width - titulo.Width) / 2;
        }
    }
}
