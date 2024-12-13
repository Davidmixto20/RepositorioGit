using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RepositorioGit
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=.;Database=AgendaDB;Trusted_Connection=True;";
        private int contactoID = -1;

        public Form1()
        {
            InitializeComponent();
            CargarContactos();
        }

        // Método para cargar los contactos en el DataGridView
        private void CargarContactos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT * FROM Contactos";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvContactos.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar contactos: " + ex.Message);
                }
            }
        }

        // Método para limpiar los campos
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
            contactoID = -1;
            btnAgregar.Text = "Agregar";
        }

        // Evento para agregar o actualizar un contacto
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
            if (!ValidarCampos())
            {
                return;
            }

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

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Contacto agregado exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al agregar el contacto: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    CargarContactos();
                    LimpiarCampos();
                }
            }
        }

        // Método para actualizar un contacto existente
        private void ActualizarContacto()
        {
            if (!ValidarCampos())
            {
                return;
            }

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

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Contacto actualizado exitosamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al actualizar el contacto: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                    CargarContactos();
                    LimpiarCampos();
                }
            }
        }

        // Método para eliminar un contacto
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvContactos.SelectedRows.Count > 0)
            {
                var confirmacion = MessageBox.Show("¿Estás seguro de que deseas eliminar este contacto?",
                                                   "Confirmar Eliminación",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Warning);

                if (confirmacion == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvContactos.SelectedRows[0].Cells["ID"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Contactos WHERE ID = @ID";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@ID", id);

                        try
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Contacto eliminado exitosamente.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar el contacto: " + ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                            CargarContactos();
                            LimpiarCampos();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un contacto para eliminar.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Método para validar que los campos no estén vacíos
        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text) ||
                string.IsNullOrWhiteSpace(txtCorreo.Text) ||
                string.IsNullOrWhiteSpace(txtDireccion.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios. Por favor, complétalos antes de continuar.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        // Evento para editar un contacto
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvContactos.SelectedRows.Count > 0)
            {
                contactoID = Convert.ToInt32(dgvContactos.SelectedRows[0].Cells["ID"].Value);
                txtNombre.Text = dgvContactos.SelectedRows[0].Cells["Nombre"].Value.ToString();
                txtApellido.Text = dgvContactos.SelectedRows[0].Cells["Apellido"].Value.ToString();
                txtTelefono.Text = dgvContactos.SelectedRows[0].Cells["Telefono"].Value.ToString();
                txtCorreo.Text = dgvContactos.SelectedRows[0].Cells["Correo"].Value.ToString();
                txtDireccion.Text = dgvContactos.SelectedRows[0].Cells["Direccion"].Value.ToString();

                btnAgregar.Text = "Actualizar";
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un contacto para editar.");
            }
        }

        // Método para buscar contactos
        // Método para buscar contactos por nombre y/o apellido
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string apellido = txtApellido.Text.Trim();

            // Validación para verificar si ambos campos están vacíos
            if (string.IsNullOrWhiteSpace(nombre) && string.IsNullOrWhiteSpace(apellido))
            {
                MessageBox.Show("Por favor, ingresa un nombre o apellido para realizar la búsqueda.",
                                "Campos Vacíos",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Contactos WHERE (@Nombre = '' OR Nombre LIKE @Nombre) AND (@Apellido = '' OR Apellido LIKE @Apellido)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", "%" + nombre + "%");
                cmd.Parameters.AddWithValue("@Apellido", "%" + apellido + "%");

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvContactos.DataSource = dt;

                    // Verificar si se encontraron resultados
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No se encontraron contactos con los criterios de búsqueda.",
                                        "Sin Resultados",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar contactos: " + ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }
    }
}

