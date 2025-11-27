using System;
using System.Drawing;
using System.Windows.Forms;

namespace PracticaXMLDinamica
{
    public partial class FormLogin : Form
    {
        public List<(string username, string password)> usuarios = new List<(string, string)>();

        // Usuario simulado
        private string userGuardado = "pablo";
        private string passGuardada = "1234";

        private int intentosFallidos = 0;
        private const int MAX_INTENTOS = 3;

        public FormLogin()
        {
            InitializeComponent();
            CrearInterfaz();

            // Usuario inicial
            usuarios.Add((userGuardado, passGuardada));

            // Centrado FINAL del login
            this.Shown += (s, e) =>
            {
                Panel panel = (Panel)this.Controls["panelLogin"];
                CentrarLogin(panel);
            };
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
            lblTitulo.Text = "Acceso al Sistema";
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitulo.AutoSize = true;
            panel.Controls.Add(lblTitulo);

            // USER LABEL
            Label lblUser = new Label();
            lblUser.Name = "lblUser";
            lblUser.Text = "User:";
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
            lblPass.Text = "Password:";
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

            // BOTÓN LOGIN
            Button btnLogin = new Button();
            btnLogin.Name = "btnLogin";
            btnLogin.Text = "Iniciar Sesion";
            btnLogin.Size = new Size(150, 40);
            btnLogin.BackColor = Color.FromArgb(200, 16, 46);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 2;
            btnLogin.FlatAppearance.BorderColor = Color.FromArgb(0, 56, 168);
            btnLogin.Click += BtnLogin_Click;
            panel.Controls.Add(btnLogin);

            // BOTÓN NUEVO USUARIO
            Button btnNuevo = new Button();
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Text = "Registrarse";
            btnNuevo.Size = new Size(150, 40);
            btnNuevo.BackColor = Color.FromArgb(200, 16, 46);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.FlatAppearance.BorderSize = 2;
            btnNuevo.FlatAppearance.BorderColor = Color.FromArgb(0, 56, 168);
            btnNuevo.Click += BtnNuevo_Click;
            panel.Controls.Add(btnNuevo);
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

            // Título
            lblTitulo.Left = centerX - lblTitulo.Width / 2;
            lblTitulo.Top = 40;

            // User
            txtUser.Left = centerX - txtUser.Width / 2;
            txtUser.Top = 130;

            lblUser.Left = txtUser.Left - lblUser.Width - 15;
            lblUser.Top = txtUser.Top + 3;

            // Password
            txtPass.Left = centerX - txtPass.Width / 2;
            txtPass.Top = 190;

            lblPass.Left = txtPass.Left - lblPass.Width - 15;
            lblPass.Top = txtPass.Top + 3;

            // Botones
            btnLogin.Top = 270;
            btnNuevo.Top = 270;

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
        //   CLICK — LOGIN
        // =====================================================================
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            Panel panel = (Panel)this.Controls["panelLogin"];

            TextBox user = (TextBox)panel.Controls["txtUser"];
            TextBox pass = (TextBox)panel.Controls["txtPass"];

            // VALIDACIÓN DE CAMPOS VACÍOS
            if (string.IsNullOrWhiteSpace(user.Text) || string.IsNullOrWhiteSpace(pass.Text))
            {
                MessageBox.Show(
      "📝 Por favor, introduzca el usuario y la contraseña antes de continuar.",
      "Información",
      MessageBoxButtons.OK,
      MessageBoxIcon.None
  );


                return;
            }

            // BUSCAR USUARIO EN LA LISTA (memoria)
            var encontrado = usuarios.Find(u => u.username == user.Text);

            // SI NO EXISTE → MENSAJE
            if (encontrado.username == null)
            {
                MessageBox.Show(
    "❌ El usuario introducido no existe.\n\n" +
    "Verifique sus datos antes de continuar.",
    "Error de autenticación",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error
);

                return;
            }

            // SI EXISTE PERO CONTRASEÑA INCORRECTA
            if (encontrado.password != pass.Text)
            {
                intentosFallidos++;

                if (intentosFallidos >= MAX_INTENTOS)
                {
                    MessageBox.Show(
     "⚠ Has alcanzado el número máximo de intentos.\n\n" +
     "El usuario quedará bloqueado durante 8 segundos.",
     "Usuario bloqueado",
     MessageBoxButtons.OK,
     MessageBoxIcon.Warning
 );

                    this.Enabled = false;

                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 8000;
                    timer.Tick += (s2, e2) =>
                    {
                        intentosFallidos = 0;
                        this.Enabled = true;
                        timer.Stop();
                    };
                    timer.Start();

                }
                else
                {
                    MessageBox.Show(
    " La contraseña introducida no es correcta.\n\nPor favor, inténtelo de nuevo.",
    "Error de autenticación",
    MessageBoxButtons.OK,
    MessageBoxIcon.Error
);

                }

                return;
            }

            // LOGIN CORRECTO
            MessageBox.Show(
    "✔ Inicio de sesión correcto\n\nBienvenido al sistema",
    "Autenticación exitosa",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information
);
            Form1 home = new Form1();
            home.Show();
            this.Hide();
        }
    }
}
