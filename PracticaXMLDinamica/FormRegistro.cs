using System;
using System.Windows.Forms;

namespace PracticaXMLDinamica
{
    public partial class FormRegistro : Form
    {
        private FormLogin loginForm;

        public FormRegistro(FormLogin formLogin)
        {
            InitializeComponent();
            loginForm = formLogin;
            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            // FORM
            this.Text = "Registrar Usuario";
            this.BackColor = Color.FromArgb(30, 30, 30);   // Fondo oscuro
            this.Size = new Size(420, 330);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            Font fuente = new Font("Segoe UI", 10, FontStyle.Bold);
            Font fuenteCampos = new Font("Segoe UI", 10, FontStyle.Regular);

            // LABEL NUEVO USUARIO
            Label lblUser = new Label()
            {
                Text = "Nuevo usuario:",
                Left = 30,
                Top = 40,
                Width = 140,
                ForeColor = Color.White,
                Font = fuente
            };

            // TEXTBOX USUARIO
            TextBox txtUser = new TextBox()
            {
                Name = "txtUserReg",
                Left = 180,
                Top = 38,
                Width = 180,
                Font = fuenteCampos
            };

            // LABEL CONTRASEÑA
            Label lblPass1 = new Label()
            {
                Text = "Contraseña:",
                Left = 30,
                Top = 100,
                Width = 140,
                ForeColor = Color.White,
                Font = fuente
            };

            // TEXTBOX 1
            TextBox txtPass1 = new TextBox()
            {
                Name = "txtPass1Reg",
                Left = 180,
                Top = 98,
                Width = 180,
                PasswordChar = '*',
                Font = fuenteCampos
            };

            // LABEL REPETIR
            Label lblPass2 = new Label()
            {
                Text = "Repetir:",
                Left = 30,
                Top = 160,
                Width = 140,
                ForeColor = Color.White,
                Font = fuente
            };

            // TEXTBOX 2
            TextBox txtPass2 = new TextBox()
            {
                Name = "txtPass2Reg",
                Left = 180,
                Top = 158,
                Width = 180,
                PasswordChar = '*',
                Font = fuenteCampos
            };

            // BOTÓN REGISTRAR (tunear)
            Button btnRegistrar = new Button()
            {
                Text = "Registrar",
                Left = 150,
                Top = 220,
                Width = 110,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(200, 16, 46),     // Rojo
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnRegistrar.FlatAppearance.BorderSize = 2;
            btnRegistrar.FlatAppearance.BorderColor = Color.FromArgb(0, 56, 168);

            // EFECTO HOVER
            btnRegistrar.MouseEnter += (s, e) => btnRegistrar.BackColor = Color.FromArgb(0, 56, 168);
            btnRegistrar.MouseLeave += (s, e) => btnRegistrar.BackColor = Color.FromArgb(200, 16, 46);

            // EVENTO REGISTRAR
            btnRegistrar.Click += (s, e) =>
            {
                string user = txtUser.Text.Trim();
                string pass1 = txtPass1.Text;
                string pass2 = txtPass2.Text;

                if (user == "" || pass1 == "" || pass2 == "")
                {
                    MessageBox.Show("⚠ Rellene todos los campos.", "Campos vacíos",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (pass1 != pass2)
                {
                    MessageBox.Show("❌ Las contraseñas no coinciden.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Comprobar si ya existe
                foreach (var u in loginForm.usuarios)
                {
                    if (u.username == user)
                    {
                        MessageBox.Show("❌ Ese usuario ya existe.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                loginForm.usuarios.Add((user, pass1));
                MessageBox.Show("✔ Usuario registrado correctamente.", "Registro exitoso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            };

            // AÑADIR TODO AL FORMULARIO
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUser);
            this.Controls.Add(lblPass1);
            this.Controls.Add(txtPass1);
            this.Controls.Add(lblPass2);
            this.Controls.Add(txtPass2);
            this.Controls.Add(btnRegistrar);
        }

    }
}
