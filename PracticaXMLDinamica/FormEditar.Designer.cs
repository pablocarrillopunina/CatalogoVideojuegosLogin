namespace PracticaXMLDinamica
{
    partial class FormEditar
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblNombre;
        private TextBox txtNombre;
        private Label lblEdad;
        private TextBox txtEdad;
        private Button btnGuardar;
        private Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblNombre = new Label();
            this.txtNombre = new TextBox();
            this.lblEdad = new Label();
            this.txtEdad = new TextBox();
            this.btnGuardar = new Button();
            this.btnCancelar = new Button();

            this.SuspendLayout();

            // lblNombre
            this.lblNombre.Font = new Font("Segoe UI", 11F);
            this.lblNombre.Location = new Point(20, 20);
            this.lblNombre.Size = new Size(80, 25);
            this.lblNombre.Text = "Nombre:";

            // txtNombre
            this.txtNombre.Font = new Font("Segoe UI", 11F);
            this.txtNombre.Location = new Point(110, 18);
            this.txtNombre.Size = new Size(160, 28);

            // ⭐ CORRECCIÓN DEL ERROR DE PRIMERA LETRA ⭐
            this.txtNombre.AutoSize = false;
            this.txtNombre.Height = 28;
            this.txtNombre.BorderStyle = BorderStyle.FixedSingle;
            this.txtNombre.Padding = new Padding(0);
            this.txtNombre.BackColor = Color.White;
            this.txtNombre.ForeColor = Color.Black;

            // lblEdad
            this.lblEdad.Font = new Font("Segoe UI", 11F);
            this.lblEdad.Location = new Point(20, 60);
            this.lblEdad.Size = new Size(80, 25);
            this.lblEdad.Text = "Edad:";

            // txtEdad
            this.txtEdad.Font = new Font("Segoe UI", 11F);
            this.txtEdad.Location = new Point(110, 58);
            this.txtEdad.Size = new Size(160, 28);

            // ⭐ MISMO ARREGLO PARA EVITAR EL ERRO DE DPI ⭐
            this.txtEdad.AutoSize = false;
            this.txtEdad.Height = 28;
            this.txtEdad.BorderStyle = BorderStyle.FixedSingle;
            this.txtEdad.Padding = new Padding(0);
            this.txtEdad.BackColor = Color.White;
            this.txtEdad.ForeColor = Color.Black;

            // btnGuardar
            this.btnGuardar.Font = new Font("Segoe UI", 10F);
            this.btnGuardar.Location = new Point(45, 110);
            this.btnGuardar.Size = new Size(90, 30);
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += btnGuardar_Click;

            // btnCancelar
            this.btnCancelar.Font = new Font("Segoe UI", 10F);
            this.btnCancelar.Location = new Point(155, 110);
            this.btnCancelar.Size = new Size(90, 30);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Click += btnCancelar_Click;

            // FormEditar
            this.ClientSize = new Size(300, 170);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblEdad);
            this.Controls.Add(this.txtEdad);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.btnCancelar);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Editar";
            this.BackColor = Color.White;

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
