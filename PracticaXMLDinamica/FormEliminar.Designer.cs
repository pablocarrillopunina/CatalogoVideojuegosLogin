namespace PracticaXMLDinamica
{
    partial class FormEliminar
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblPregunta;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnCancelar;

        /// <summary>
        /// Limpieza de recursos.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblPregunta = new System.Windows.Forms.Label();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // 
            // lblPregunta
            // 
            this.lblPregunta.Text = "¿Seguro que quieres eliminar el registro?";
            this.lblPregunta.Left = 20;
            this.lblPregunta.Top = 20;
            this.lblPregunta.AutoSize = true;
            this.lblPregunta.MaximumSize = new Size(340, 0);

            // 
            // btnEliminar
            // 
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Left = 20;
            this.btnEliminar.Top = 70;
            this.btnEliminar.Width = 90;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // 
            // btnCancelar
            // 
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Left = 130;
            this.btnCancelar.Top = 70;
            this.btnCancelar.Width = 90;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);

            // 
            // FormEliminar
            // 
            this.ClientSize = new System.Drawing.Size(300, 150);
            this.Controls.Add(this.lblPregunta);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnCancelar);
            this.Name = "FormEliminar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Eliminar";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
