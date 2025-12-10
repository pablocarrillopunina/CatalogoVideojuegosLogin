using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using PracticaXMLDinamica.Data;

namespace PracticaXMLDinamica
{
    public partial class FormLogin : Form
    {
        public List<(string username, string password)> usuarios = new List<(string, string)>();



        public FormLogin()
        {
            InitializeComponent();
            CargarFondo();
            CrearInterfaz();

            // Cuando el formulario ya está pintado → centrar login
            this.Shown += (s, e) =>
            {
                Panel panel = (Panel)this.Controls["panelLogin"];
                CentrarLogin(panel);
            };

            // Cuando el panel cambie de tamaño → recentrar
            Panel p = (Panel)this.Controls["panelLogin"];
            p.Resize += (s, e) => CentrarLogin(p);
        }


        // =====================================================================
        //   CREACIÓN DE INTERFAZ
        // =====================================================================
        private void CrearInterfaz()
        {
            // 🎨 FORM
            this.Text = "Login";
            this.BackColor = Color.FromArgb(25, 25, 25);
            this.ClientSize = new Size(1000, 600);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;

            // PANEL PRINCIPAL
            Panel panel = new Panel();
            panel.Name = "panelLogin";
            panel.BackColor = Color.FromArgb(40, 40, 40);
            panel.Size = new Size(700, 420);
            panel.Location = new Point(
                (this.ClientSize.Width - panel.Width) / 2,
                (this.ClientSize.Height - panel.Height) / 2
            );
            panel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(panel);

            // TÍTULO
            Label lblTitulo = new Label();
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Text = "Login del Jugador Supremo";
            lblTitulo.ForeColor = Color.AntiqueWhite;
            lblTitulo.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            panel.Controls.Add(lblTitulo);

            // USER LABEL
            Label lblUser = new Label();
            lblUser.Name = "lblUser";
            lblUser.Text = "Nickname:";
            lblUser.ForeColor = Color.White;
            lblUser.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblUser.AutoSize = true;
            panel.Controls.Add(lblUser);

            // USER TEXTBOX
            TextBox txtUser = new TextBox();
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(300, 30);
            panel.Controls.Add(txtUser);

            // PASSWORD LABEL
            Label lblPass = new Label();
            lblPass.Name = "lblPass";
            lblPass.Text = "Clave secreta:";
            lblPass.ForeColor = Color.White;
            lblPass.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblPass.AutoSize = true;
            panel.Controls.Add(lblPass);

            // PASSWORD TEXTBOX
            TextBox txtPass = new TextBox();
            txtPass.Name = "txtPass";
            txtPass.PasswordChar = '*';
            txtPass.Size = new Size(300, 30);
            panel.Controls.Add(txtPass);

            // ==========================
            // Botón de inicio de sesión
            // ==========================

            Button btnLogin = new Button();
            btnLogin.Name = "btnLogin";
            btnLogin.Text = "Iniciar Sesion";
            btnLogin.Size = new Size(150, 40);

            // 🔥 Estilo mejorado pero manteniendo tu estructura
            btnLogin.BackColor = Color.Black;
            btnLogin.ForeColor = Color.White;

            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 2; // Borde gamer
            btnLogin.FlatAppearance.BorderColor = Color.FromArgb(255, 60, 60); // Rojo neon suave

            btnLogin.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Hover suave con glow (manteniendo tu lógica)
            btnLogin.MouseEnter += (s, e) =>
            {
                btnLogin.BackColor = Color.FromArgb(35, 35, 35);
                btnLogin.FlatAppearance.BorderColor = Color.FromArgb(255, 120, 120); // Glow aumentado
            };

            btnLogin.MouseLeave += (s, e) =>
            {
                btnLogin.BackColor = Color.Black;
                btnLogin.FlatAppearance.BorderColor = Color.FromArgb(255, 60, 60);
            };

            btnLogin.Click += BtnLogin_Click;
            panel.Controls.Add(btnLogin);



            // ==========================
            // Botón de nuevo usuario
            // ==========================

            Button btnNuevo = new Button();
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Text = "Nuevo Usuario";
            btnNuevo.Size = new Size(150, 40);

            // 🔥 Estilo mejorado igual que Login
            btnNuevo.BackColor = Color.Black;
            btnNuevo.ForeColor = Color.White;

            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.FlatAppearance.BorderSize = 2;
            btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(255, 60, 60);

            btnNuevo.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            // Hover suave con glow
            btnNuevo.MouseEnter += (s, e) =>
            {
                btnNuevo.BackColor = Color.FromArgb(35, 35, 35);
                btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(255, 120, 120);
            };

            btnNuevo.MouseLeave += (s, e) =>
            {
                btnNuevo.BackColor = Color.Black;
                btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(255, 60, 60);
            };

            btnNuevo.Click += BtnNuevo_Click;
            panel.Controls.Add(btnNuevo);





        }
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
                else
                {
                    MessageBox.Show("⚠ No se encontró la imagen en:\n" + ruta);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠ Error cargando fondo:\n\n" + ex.Message);
            }
        }

        // =====================================================================
        //   CENTRADO COMPLETO DEL LOGIN
        // =====================================================================
        private void CentrarLogin(Panel panel)
        {
            Label lblTitulo = (Label)panel.Controls["lblTitulo"];
            Label lblUser = (Label)panel.Controls["lblUser"];
            TextBox txtUser = (TextBox)panel.Controls["txtUser"];
            Label lblPass = (Label)panel.Controls["lblPass"];
            TextBox txtPass = (TextBox)panel.Controls["txtPass"];
            Button btnLogin = (Button)panel.Controls["btnLogin"];
            Button btnNuevo = (Button)panel.Controls["btnNuevo"];

            int centerX = panel.Width / 2;

            // 👉 DESPLAZAMIENTO A LA DERECHA
            int offsetX = 100;  // ← AJUSTA ESTE VALOR para moverlos más o menos

            // ======================
            //   TÍTULO centrado
            // ======================
            lblTitulo.Left = centerX - lblTitulo.Width / 2;
            lblTitulo.Top = 40;

            // Distancia vertical entre los campos
            int spacingY = 60;

            // Posición inicial Y
            int startY = 130;

            // ======================
            //   USER (centrado + desplazado a la derecha)
            // ======================
            txtUser.Left = centerX - txtUser.Width / 2 + offsetX;
            txtUser.Top = startY;

            lblUser.Left = txtUser.Left - lblUser.Width - 15;
            lblUser.Top = txtUser.Top + 3;

            // ======================
            //   PASSWORD (igual desplazado)
            // ======================
            txtPass.Left = centerX - txtPass.Width / 2 + offsetX;
            txtPass.Top = txtUser.Top + spacingY;

            lblPass.Left = txtPass.Left - lblPass.Width - 15;
            lblPass.Top = txtPass.Top + 3;

            // ======================
            //   BOTONES centrados
            // ======================
            btnLogin.Top = txtPass.Top + spacingY + 30;
            btnNuevo.Top = btnLogin.Top;

            btnLogin.Left = centerX - btnLogin.Width - 10;
            btnNuevo.Left = centerX + 10;
        }



        // =====================================================================
        //   CLICK — NUEVO USUARIO
        // =====================================================================
        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            FormRegistro reg = new FormRegistro(this);
            reg.ShowDialog();
        }

        // =====================================================================
        //   CLICK — LOGIN a Mysql 
        // =====================================================================
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)this.Controls["panelLogin"];
            TextBox user = (TextBox)panel.Controls["txtUser"];
            TextBox pass = (TextBox)panel.Controls["txtPass"];

            // VALIDACIÓN DE CAMPOS VACÍOS
            if (string.IsNullOrWhiteSpace(user.Text) || string.IsNullOrWhiteSpace(pass.Text))
            {
                MessageBox.Show("📝 Por favor, introduzca el usuario y la contraseña.");
                return;
            }

            // CONSULTA SQL PARA VERIFICAR USUARIO
            string query = "SELECT * FROM Usuarios WHERE nombre_usuario=@user AND password=@pass";

            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                // ❗ PRIMERA PROTECCIÓN: Si la conexión vino NULL
                if (con == null)
                {
                    MessageBox.Show("❌ No se pudo crear la conexión con MySQL.\n" +
                                    "Revise el puerto, usuario, contraseña o el estado del servidor.",
                                    "Error de conexión",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    // ❗ SEGUNDA PROTECCIÓN: Intentar abrir la conexión
                    con.Open();

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", user.Text);
                    cmd.Parameters.AddWithValue("@pass", pass.Text);

                    MySqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        // LOGIN CORRECTO
                        MessageBox.Show("✔ Inicio de sesión correcto\n\nBienvenido al sistema",
                            "Autenticación exitosa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        Form1 home = new Form1();
                        home.Show();
                        this.Hide();
                    }
                    else
                    {
                        // LOGIN INCORRECTO
                        MessageBox.Show("❌ Usuario o contraseña incorrectos.",
                            "Error de autenticación",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                catch (MySqlException ex)
                {
                    // ❗ ERRORES DE MYSQL (puerto, usuario, contraseña, servidor apagado, etc.)
                    MessageBox.Show("⚠ Error conectando a MySQL:\n\n" + ex.Message,
                        "Error de MySQL",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    // ❗ Errores generales
                    MessageBox.Show("⚠ Error inesperado:\n\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

        }

    }
}
