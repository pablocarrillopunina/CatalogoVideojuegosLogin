using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using PracticaXMLDinamica.Data;

namespace PracticaXMLDinamica
{
    public partial class FormAdmin : Form
    {
        DataGridView dgvUsuarios;
        TextBox txtBuscar, txtUsuario, txtPassword, txtEmail;
        Button btnBuscar, btnListar, btnAgregar, btnEditar, btnEliminar;
        Button btnLimpiar, btnCerrarSesion, btnSalir, btnBanear;

        bool cargando = true;

        public FormAdmin()
        {
            InitializeComponent();
            ConfigurarFormulario();
            CrearInterfaz();
            ListarUsuarios();
            Limpiar();
        }

        // =========================
        // FORMULARIO
        // =========================
        private void ConfigurarFormulario()
        {
            this.Text = "Panel de Administración de Usuarios";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;

            try
            {
                string ruta = Path.Combine(Application.StartupPath, "Resources", "login.jpg");
                if (File.Exists(ruta))
                {
                    using (Image img = Image.FromFile(ruta))
                        this.BackgroundImage = new Bitmap(img);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch { }
        }

        // =========================
        // INTERFAZ
        // =========================
        private void CrearInterfaz()
        {
            GroupBox gbBuscar = new GroupBox
            {
                Text = "Buscar usuario",
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White
            };

            Label lblBuscar = new Label { Text = "Nombre:", ForeColor = Color.White, Left = 20, Top = 30 };
            txtBuscar = new TextBox { Left = 120, Top = 27, Width = 300 };

            btnBuscar = CrearBotonSuperior("Buscar", 440);
            btnListar = CrearBotonSuperior("Listar", 570);

            btnBuscar.Click += (s, e) => BuscarUsuarios();
            btnListar.Click += (s, e) => { txtBuscar.Clear(); ListarUsuarios(); };

            gbBuscar.Controls.AddRange(new Control[] { lblBuscar, txtBuscar, btnBuscar, btnListar });
            this.Controls.Add(gbBuscar);

            GroupBox gbListado = new GroupBox
            {
                Text = "Listado de usuarios",
                Dock = DockStyle.Top,
                Height = 220,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White
            };

            dgvUsuarios = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(45, 45, 45),
                BorderStyle = BorderStyle.None
            };

            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            dgvUsuarios.DefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.Red;

            dgvUsuarios.CellMouseClick += CargarSeleccion;

            gbListado.Controls.Add(dgvUsuarios);
            this.Controls.Add(gbListado);

            Panel panelInferior = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 260,
                BackColor = Color.FromArgb(20, 20, 20)
            };
            this.Controls.Add(panelInferior);

            GroupBox gbDatos = new GroupBox
            {
                Text = "Datos del usuario",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 30, 30),
                Left = 20,
                Top = 15,
                Width = 420,
                Height = 220
            };

            gbDatos.Controls.Add(new Label { Text = "Usuario:", ForeColor = Color.White, Left = 20, Top = 40 });
            txtUsuario = new TextBox { Left = 140, Top = 37, Width = 240 };
            gbDatos.Controls.Add(txtUsuario);

            gbDatos.Controls.Add(new Label { Text = "Contraseña:", ForeColor = Color.White, Left = 20, Top = 80 });
            txtPassword = new TextBox { Left = 140, Top = 77, Width = 200, UseSystemPasswordChar = true };
            gbDatos.Controls.Add(txtPassword);

            // 👁 BOTÓN VER CONTRASEÑA
            Button btnVerPass = new Button
            {
                Text = "👁",
                Left = txtPassword.Right + 5,
                Top = txtPassword.Top,
                Width = 35,
                Height = txtPassword.Height
            };
            btnVerPass.Click += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            };
            gbDatos.Controls.Add(btnVerPass);

            gbDatos.Controls.Add(new Label { Text = "Email:", ForeColor = Color.White, Left = 20, Top = 120 });
            txtEmail = new TextBox { Left = 140, Top = 117, Width = 240 };
            gbDatos.Controls.Add(txtEmail);

            panelInferior.Controls.Add(gbDatos);

            GroupBox gbAcciones = new GroupBox
            {
                Text = "Acciones",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 30, 30),
                Left = 470,
                Top = 15,
                Width = 250,
                Height = 220
            };

            btnAgregar = CrearBoton("Agregar", 30);
            btnEditar = CrearBoton("Editar", 70);
            btnEliminar = CrearBoton("Eliminar", 110);
            btnLimpiar = CrearBoton("Limpiar", 150);
            btnBanear = CrearBoton("Banear / Activar", 190);

            btnAgregar.Click += (s, e) => AgregarUsuario();
            btnEditar.Click += (s, e) => EditarUsuario();
            btnEliminar.Click += (s, e) => EliminarUsuario();
            btnLimpiar.Click += (s, e) => Limpiar();
            btnBanear.Click += (s, e) => CambiarEstadoUsuario();

            gbAcciones.Controls.AddRange(new Control[]
            {
                btnAgregar, btnEditar, btnEliminar, btnLimpiar, btnBanear
            });

            panelInferior.Controls.Add(gbAcciones);

            btnCerrarSesion = CrearBotonSuperior("Cerrar sesión", 760);
            btnCerrarSesion.Top = 40;
            btnCerrarSesion.Width = 180;
            btnCerrarSesion.Click += (s, e) => { new FormLogin().Show(); this.Close(); };

            btnSalir = CrearBotonSuperior("Salir", 760);
            btnSalir.Top = 90;
            btnSalir.Width = 180;
            btnSalir.Click += (s, e) => Application.Exit();

            panelInferior.Controls.Add(btnCerrarSesion);
            panelInferior.Controls.Add(btnSalir);
        }

        private Button CrearBoton(string texto, int top) =>
            new Button
            {
                Text = texto,
                Left = 20,
                Top = top,
                Width = 200,
                Height = 30,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

        private Button CrearBotonSuperior(string texto, int left)
        {
            Button b = new Button
            {
                Text = texto,
                Left = left,
                Top = 22,
                Width = 120,
                Height = 35,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.Black,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            b.FlatAppearance.BorderColor = Color.Red;
            b.FlatAppearance.BorderSize = 2;
            return b;
        }

        // =========================
        // CRUD-METODOS
        // =========================
        private void ListarUsuarios()
        {
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(
                    "SELECT id,nombre_usuario,rol,email,estado FROM usuarios", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUsuarios.DataSource = dt;
                dgvUsuarios.ClearSelection();
                cargando = false;
            }
        }

        private void BuscarUsuarios()
        {
            if (string.IsNullOrWhiteSpace(txtBuscar.Text))
            {
                MessageBox.Show("Introduce un nombre para buscar.");
                return;
            }

            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(
                    "SELECT id,nombre_usuario,rol,email,estado FROM usuarios WHERE nombre_usuario LIKE @n", con);
                da.SelectCommand.Parameters.AddWithValue("@n", "%" + txtBuscar.Text + "%");

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Usuario no encontrado.");
                    dgvUsuarios.DataSource = null;
                    return;
                }

                dgvUsuarios.DataSource = dt;
            }
        }


        private void AgregarUsuario()
        {
            if (CamposVacios())
            {
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            if (MessageBox.Show("¿Crear usuario?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            EjecutarSQL(
                "INSERT INTO usuarios (nombre_usuario,password,email,rol,estado) VALUES (@u,@p,@e,'USER','ACTIVO')",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@u", txtUsuario.Text);
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text);
                });

            RegistrarLog("Creó el usuario " + txtUsuario.Text);
            MessageBox.Show("Usuario creado correctamente.");

            Limpiar();
            ListarUsuarios();
        }

        private void EditarUsuario()
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un usuario.");
                return;
            }

            if (MessageBox.Show("¿Guardar cambios?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["id"].Value);

            string sql;

            // Si el admin escribe una contraseña, se actualiza
            if (!string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                sql = "UPDATE usuarios SET nombre_usuario=@u, email=@e, password=@p WHERE id=@id";
            }
            // Si no, se mantiene la actual
            else
            {
                sql = "UPDATE usuarios SET nombre_usuario=@u, email=@e WHERE id=@id";
            }

            EjecutarSQL(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@u", txtUsuario.Text);
                cmd.Parameters.AddWithValue("@e", txtEmail.Text);
                cmd.Parameters.AddWithValue("@id", id);

                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text);
                }
            });

            RegistrarLog("Editó el usuario " + txtUsuario.Text);
            MessageBox.Show("Usuario actualizado.");

            Limpiar();
            ListarUsuarios();
        }


        private void EliminarUsuario()
        {
            if (dgvUsuarios.SelectedRows.Count == 0) return;

            string rol = dgvUsuarios.SelectedRows[0].Cells["rol"].Value.ToString();
            if (rol == "ADMIN")
            {
                MessageBox.Show("No se puede eliminar un administrador.");
                return;
            }

            if (MessageBox.Show("¿Eliminar usuario?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["id"].Value);

            EjecutarSQL("DELETE FROM usuarios WHERE id=@id",
                cmd => cmd.Parameters.AddWithValue("@id", id));

            RegistrarLog("Eliminó un usuario");
            MessageBox.Show("Usuario eliminado.");

            Limpiar();
            ListarUsuarios();
        }

        private void CambiarEstadoUsuario()
        {
            if (dgvUsuarios.SelectedRows.Count == 0) return;

            int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["id"].Value);
            string estado = dgvUsuarios.SelectedRows[0].Cells["estado"].Value.ToString();
            string nuevo = estado == "ACTIVO" ? "BANEADO" : "ACTIVO";

            if (MessageBox.Show($"¿Cambiar estado a {nuevo}?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            EjecutarSQL("UPDATE usuarios SET estado=@e WHERE id=@id",
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@e", nuevo);
                    cmd.Parameters.AddWithValue("@id", id);
                });

            RegistrarLog("Cambió estado del usuario ID " + id);
            ListarUsuarios();
        }

        private void CargarSeleccion(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ignorar clicks en cabecera
            if (e.RowIndex < 0) return;

            DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];

            txtUsuario.Text = fila.Cells["nombre_usuario"].Value.ToString();
            txtEmail.Text = fila.Cells["email"].Value.ToString();

            // Nunca cargar contraseña
            txtPassword.Clear();
        }


        private bool CamposVacios() =>
            string.IsNullOrWhiteSpace(txtUsuario.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text) ||
            string.IsNullOrWhiteSpace(txtEmail.Text);

        private void Limpiar()
        {
            txtUsuario.Clear();
            txtPassword.Clear();
            txtEmail.Clear();
        }

        private void EjecutarSQL(string sql, Action<MySqlCommand> parametros)
        {
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                parametros(cmd);
                cmd.ExecuteNonQuery();
            }
        }

        private void RegistrarLog(string accion)
        {
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO log_actividad (admin_usuario, accion, fecha) VALUES (@a,@ac,NOW())";
                MySqlCommand cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@a", FormLogin.UsuarioLogueado);
                cmd.Parameters.AddWithValue("@ac", accion);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
