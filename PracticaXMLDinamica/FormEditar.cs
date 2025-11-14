using System;
using System.Windows.Forms;

namespace PracticaXMLDinamica
{
    public partial class FormEditar : Form
    {
        private DataGridView tabla;
        private int filaSeleccionada;

        public FormEditar(DataGridView tabla)
        {
            InitializeComponent();
            this.tabla = tabla;

            this.BackColor = Color.White;

            if (tabla.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de editar.");
                this.Close();
                return;
            }

            filaSeleccionada = tabla.CurrentRow.Index;

            txtNombre.Text = tabla.Rows[filaSeleccionada].Cells[1].Value.ToString();
            txtEdad.Text = tabla.Rows[filaSeleccionada].Cells[2].Value.ToString();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtEdad.Text))
            {
                MessageBox.Show("Rellena todos los campos.");
                return;
            }

            tabla.Rows[filaSeleccionada].Cells[1].Value = txtNombre.Text;
            tabla.Rows[filaSeleccionada].Cells[2].Value = txtEdad.Text;

            MessageBox.Show("Registro actualizado correctamente ✔");
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
