using System;
using System.Drawing;
using System.Windows.Forms;

namespace PracticaXMLDinamica
{
    public partial class FormAñadir : Form
    {
        public DataGridView Tabla;

        public FormAñadir(DataGridView tabla)
        {
            InitializeComponent();
            Tabla = tabla;

            this.BackColor = Color.White;

            FixTextBoxVisual(txtNombre);
            FixTextBoxVisual(txtEdad);
        }

        private void FixTextBoxVisual(TextBox txt)
        {
            txt.RightToLeft = RightToLeft.No;
            txt.TextAlign = HorizontalAlignment.Left;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.AutoSize = false;
            txt.Height = 28;
            txt.Padding = new Padding(0);
            txt.BackColor = Color.White;
            txt.ForeColor = Color.Black;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtEdad.Text))
            {
                MessageBox.Show("Completa todos los campos.");
                return;
            }

            // ============================
            //   ID CORRECTO — PERFECTO
            // ============================
            int nuevoID = 1;

            for (int i = Tabla.Rows.Count - 1; i >= 0; i--)
            {
                if (Tabla.Rows[i].Cells[0].Value != null &&
                    Tabla.Rows[i].Cells[0].Value.ToString() != "")
                {
                    nuevoID = Convert.ToInt32(Tabla.Rows[i].Cells[0].Value) + 1;
                    break;
                }
            }

            Tabla.Rows.Add(nuevoID, txtNombre.Text, txtEdad.Text);

            MessageBox.Show("Registro añadido correctamente ✔");
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
