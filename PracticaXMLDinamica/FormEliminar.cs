using System;
using System.Windows.Forms;

namespace PracticaXMLDinamica
{
    public partial class FormEliminar : Form
    {
        private DataGridView tabla; // referencia a la tabla

        public FormEliminar(DataGridView tabla)
        {
            InitializeComponent();
            this.tabla = tabla;

            if (tabla.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila antes de eliminar.");
                this.Close();
                return;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (tabla.CurrentRow == null)
            {
                MessageBox.Show("No hay fila seleccionada.");
                return;
            }

            var confirmar = MessageBox.Show(
                "¿Seguro que deseas eliminar este registro?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (confirmar == DialogResult.Yes)
            {
                tabla.Rows.Remove(tabla.CurrentRow);
                MessageBox.Show("Registro eliminado correctamente ✔");
            }

            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
