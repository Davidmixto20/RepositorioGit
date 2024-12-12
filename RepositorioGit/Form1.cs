using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepositorioGit
{
    public partial class Form1 : Form
    {
        // Cadena de conexión a SQL Server
        string connectionString = "Server=.;Database=AgendaDB;Trusted_Connection=True;";

        // Variable para almacenar el ID del contacto seleccionado para editar
        private int contactoID = -1;

        public Form1()
        {
            InitializeComponent();
            CargarContactos();
        }

        // Método para agregar o actualizar un contacto
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (contactoID == -1)
            {
                AgregarContacto();
            }
            else
            {
                ActualizarContacto();
            }
        }

        // Método para agregar un nuevo contacto
        private void AgregarContacto()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Contactos (Nombre, Apellido, Telefono, Correo, Direccion) " +
                               "VALUES (@Nombre, @Apellido, @Telefono, @Correo, @Direccion)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Contacto agregado exitosamente.");
                CargarContactos();
                LimpiarCampos();
            }
        }

        // Método para actualizar un contacto existente
        private void ActualizarContacto()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Contactos SET Nombre = @Nombre, Apellido = @Apellido, Telefono = @Telefono, " +
                               "Correo = @Correo, Direccion = @Direccion WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@ID", contactoID);
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Contacto actualizado correctamente.");
                CargarContactos();
                LimpiarCampos();
            }
        }

        // Evento para cargar datos del contacto seleccionado en los TextBox y cambiar el botón a "Actualizar"
        private void dgvContactos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvContactos.Rows[e.RowIndex];
                contactoID = Convert.ToInt32(row.Cells["ID"].Value);
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtApellido.Text = row.Cells["Apellido"].Value.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value.ToString();
                txtCorreo.Text = row.Cells["Correo"].Value.ToString();
                txtDireccion.Text = row.Cells["Direccion"].Value.ToString();

                btnAgregar.Text = "Actualizar";
            }
        }

        // Método para eliminar un contacto
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvContactos.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgvContactos.SelectedRows[0].Cells["ID"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Contactos WHERE ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    MessageBox.Show("Contacto eliminado exitosamente.");
                    CargarContactos();
                    LimpiarCampos();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un contacto para eliminar.");
            }
        }

        // Método para buscar contactos por nombre o apellido
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Contactos WHERE Nombre LIKE @Nombre OR Apellido LIKE @Apellido";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@Nombre", "%" + txtNombre.Text + "%");
                adapter.SelectCommand.Parameters.AddWithValue("@Apellido", "%" + txtApellido.Text + "%");

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvContactos.DataSource = dt;
            }
        }

    }
}