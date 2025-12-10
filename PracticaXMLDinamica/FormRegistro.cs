using MySql.Data.MySqlClient;
using PracticaXMLDinamica.Data;
using System;
using System.Windows.Forms;
using System.IO; // importante

namespace PracticaXMLDinamica
{
    public partial class FormRegistro : Form
    {
        private FormLogin loginForm;


        public FormRegistro(FormLogin formLogin)
        {
            InitializeComponent();
            loginForm = formLogin;
            CargarFondo();
            CrearInterfaz();
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

                // VALIDACIÓN DE CAMPOS
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

                try
                {
                    using (MySqlConnection con = DatabaseHelper.GetConnection())
                    {
                        con.Open();

                        // 1️⃣ Comprobar si el usuario existe
                        string checkQuery = "SELECT * FROM Usuarios WHERE nombre_usuario=@user";
                        MySqlCommand checkCmd = new MySqlCommand(checkQuery, con);
                        checkCmd.Parameters.AddWithValue("@user", user);

                        MySqlDataReader reader = checkCmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            MessageBox.Show("❌ Ese usuario ya existe.", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        reader.Close();

                        // 2️⃣ Insertar el nuevo usuario
                        string insertQuery =
                            "INSERT INTO Usuarios (nombre_usuario, password) VALUES (@user, @pass)";
                        MySqlCommand insertCmd = new MySqlCommand(insertQuery, con);
                        insertCmd.Parameters.AddWithValue("@user", user);
                        insertCmd.Parameters.AddWithValue("@pass", pass1);

                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show("✔ Usuario registrado correctamente.", "Registro exitoso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("⚠ Error conectando a MySQL:\n\n" + ex.Message,
                        "Error de conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
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
