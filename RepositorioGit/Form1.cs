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

    }
}