using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using System.IO;

namespace PracticaXMLDinamica
{
    // ====================================================================
    // 🎨 TABLA DE COLORES PERSONALIZADA PARA EL MENUSTRIP
    // ====================================================================
    public class MyColorTable : ProfessionalColorTable
    {
        public override Color ToolStripDropDownBackground => Color.FromArgb(48, 48, 48);
        public override Color MenuItemSelected => Color.FromArgb(63, 63, 63);
        public override Color MenuBorder => Color.FromArgb(50, 50, 50);

        public override Color ImageMarginGradientBegin => Color.FromArgb(48, 48, 48);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(48, 48, 48);
        public override Color ImageMarginGradientEnd => Color.FromArgb(48, 48, 48);
    }

    public class ForcefulToolStripRenderer : ToolStripProfessionalRenderer
    {
        public ForcefulToolStripRenderer() : base(new MyColorTable()) { }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White;
            base.OnRenderItemText(e);
        }
    }

    // ====================================================================
    // 🏠 FORM PRINCIPAL
    // ====================================================================
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(900, 650);

            CargarFondo();
            CargarInterfazDesdeXML();

            foreach (Control c in this.Controls)
                c.BringToFront();

            this.Resize += (s, e) =>
            {
                CentrarPanelPrincipal();
                CentrarTabla();
            };

            this.Shown += (s, e) =>
            {
                CentrarPanelPrincipal();
                CentrarTabla();
            };
        }

        // ====================================================================
        // 🌌 FONDO OSCURO CON IMAGEN + OVERLAY
        // ====================================================================
        private void CargarFondo()
        {
            try
            {
                string ruta = Path.Combine(Application.StartupPath, "Resources", "login.jpg");

                if (File.Exists(ruta))
                {
                    this.BackgroundImage = Image.FromFile(ruta);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }

                Panel overlay = new Panel
                {
                    BackColor = Color.FromArgb(130, 0, 0, 0),
                    Dock = DockStyle.Fill,
                    Enabled = false
                };

                this.Controls.Add(overlay);
                overlay.SendToBack();
            }
            catch { }
        }

        // ====================================================================
        // 📌 MENU SUPERIOR
        // ====================================================================
        private void CargarMenu(XmlNode menuNode)
        {
            if (menuNode == null) return;

            MenuStrip menu = new MenuStrip();
            menu.Renderer = new ForcefulToolStripRenderer();
            menu.BackColor = Color.FromArgb(50, 50, 50);
            menu.ForeColor = Color.White;
            menu.Dock = DockStyle.Top;
            menu.Padding = new Padding(5, 2, 0, 2);

            foreach (XmlNode itemNode in menuNode.SelectNodes("MenuItem"))
            {
                string texto = itemNode.Attributes["texto"].Value;

                if (texto.Contains("Mi perfil"))
                {
                    ToolStripLabel sep = new ToolStripLabel
                    {
                        AutoSize = false,
                        Width = 250
                    };
                    menu.Items.Add(sep);
                }

                ToolStripMenuItem item = CrearMenuItem(itemNode);
                menu.Items.Add(item);
            }

            this.Controls.Add(menu);
        }

        private ToolStripMenuItem CrearMenuItem(XmlNode nodo)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(nodo.Attributes["texto"].Value)
            {
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            };

            foreach (XmlNode hijo in nodo.SelectNodes("MenuItem"))
            {
                ToolStripMenuItem sub = CrearMenuItem(hijo);
                sub.Font = new Font("Segoe UI", 9F);
                item.DropDownItems.Add(sub);
            }

            if (nodo.Attributes["accion"] != null)
            {
                string accion = nodo.Attributes["accion"].Value;
                item.Click += (s, e) =>
                {
                    switch (accion)
                    {
                        case "nav": MessageBox.Show($"Has pulsado: {item.Text}"); break;
                        case "logout": new FormLogin().Show(); this.Hide(); break;
                        case "exitApp": Application.Exit(); break;
                    }
                };
            }

            return item;
        }

        // ====================================================================
        // 🧱 CARGA DE PANELES Y CONTROLES DESDE XML
        // ====================================================================
        private void CargarInterfazDesdeXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Interfaz.xml");

            CargarMenu(doc.SelectSingleNode("/Interfaz/MenuStrip"));

            foreach (XmlNode nodo in doc.SelectNodes("/Interfaz/Panel"))
                CrearPanel(nodo);

            CrearTabla(doc.SelectSingleNode("/Interfaz/DataGrid"));
        }

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
                        TextAlign = ContentAlignment.MiddleLeft,
                        Font = new Font("Segoe UI", nodo.Attributes["nombre"].Value == "lblTitulo" ? 14 : 11, FontStyle.Bold)
                    };
                    break;

                case "TextBox":
                    control = new TextBox
                    {
                        BackColor = Color.White,
                        ForeColor = Color.Black,
                        Font = new Font("Segoe UI", 10F)
                    };
                    break;

                case "Button":
                    Button btn = new Button
                    {
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.FromArgb(200, 16, 46),
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold)
                    };
                    btn.FlatAppearance.BorderColor = Color.FromArgb(0, 56, 168);
                    btn.FlatAppearance.BorderSize = 2;

                    btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(0, 56, 168);
                    btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(200, 16, 46);

                    control = btn;
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

        // ====================================================================
        // 🎯 CENTRAR PANEL Y TABLA AUTOMÁTICAMENTE
        // ====================================================================
        private void CentrarPanelPrincipal()
        {
            var panel = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;
            if (panel != null)
                panel.Left = (this.ClientSize.Width - panel.Width) / 2;
        }

        private void CentrarTabla()
        {
            var tabla = this.Controls.Find("tablaDatos", true).FirstOrDefault() as DataGridView;
            if (tabla != null)
                tabla.Left = (this.ClientSize.Width - tabla.Width) / 2;
        }

        // ====================================================================
        // 📊 TABLA DE VIDEOJUEGOS
        // ====================================================================
        private void CrearTabla(XmlNode nodo)
        {
            if (nodo == null) return;

            DataGridView dgv = new DataGridView
            {
                Name = nodo.Attributes["nombre"].Value,
                Left = int.Parse(nodo.Attributes["x"].Value),
                Top = int.Parse(nodo.Attributes["y"].Value),
                Width = int.Parse(nodo.Attributes["width"].Value),
                Height = int.Parse(nodo.Attributes["height"].Value),
                BackgroundColor = Color.FromArgb(60, 63, 65),
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                EnableHeadersVisualStyles = false
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgv.DefaultCellStyle.BackColor = Color.FromArgb(60, 63, 65);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 56, 168);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;

            foreach (XmlNode col in nodo.SelectNodes("Columna"))
            {
                dgv.Columns.Add(col.Attributes["nombre"].Value, col.Attributes["nombre"].Value);
                dgv.Columns[col.Attributes["nombre"].Value].Width = int.Parse(col.Attributes["width"].Value);
            }

            dgv.SelectionChanged += Dgv_SelectionChanged;

            this.Controls.Add(dgv);
            CargarCatalogoEnTabla(dgv);
        }

        private void Dgv_SelectionChanged(object sender, EventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv.SelectedRows.Count == 0) return;

            var row = dgv.SelectedRows[0];
            var p = this.Controls.Find("panelPrincipal", true).FirstOrDefault() as Panel;

            (p.Controls.Find("txtTitulo", true).FirstOrDefault() as TextBox).Text = row.Cells[1].Value.ToString();
            (p.Controls.Find("txtDesarrollador", true).FirstOrDefault() as TextBox).Text = row.Cells[2].Value.ToString();
            (p.Controls.Find("txtPlataforma", true).FirstOrDefault() as TextBox).Text = row.Cells[3].Value.ToString();
            (p.Controls.Find("txtPrecio", true).FirstOrDefault() as TextBox).Text = row.Cells[4].Value.ToString();
        }

        // ====================================================================
        // ✔ CRUD XML
        // ====================================================================
        private void CargarCatalogoEnTabla(DataGridView dgv)
        {
            dgv.Rows.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load("catalogo.xml");

            XmlNodeList juegos = doc.SelectNodes("/Catalogo/Videojuego");
            int id = 1;

            foreach (XmlNode juego in juegos)
            {
                dgv.Rows.Add(id++,
                    juego["Titulo"].InnerText,
                    juego["Desarrollador"].InnerText,
                    juego["Plataforma"].InnerText,
                    juego["Precio"].InnerText);
            }

            if (dgv.Rows.Count > 0)
                dgv.ClearSelection();
        }
    }
}
