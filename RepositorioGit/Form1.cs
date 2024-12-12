using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Yandell
namespace RepositorioGit
{
    public partial class Form1 : Form
    {
        string connectionString = "Server=.;Database=AgendaDB;Trusted_Connection=True;";


        //Variable para almacenar el ID del contacto seleccionado para editar
        private int contactoID = -1;

        public Form1()
        {
            InitializeComponent();
            CargarContactos();
        }

        //Metodo para agregar o actualizar un contacto
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

        //Metodo para agregar nuevo contacto
        private void AgregarContacto()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Insert into Contactos (Nombre, Apellido, Telefono, Correo, Direccion) " +
                               "Values (@Nombre, @Apellido, @Telefono, @Correo, @Direccion)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Apellido", txtApellido.Text);
                cmd.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Contacto agregado exitosamente");
                CargarContactos();
                //LimpiarCampos();

            }
        }

        //Metodo para actualizar un contacto existente
        private void ActualizarContacto()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Contactos SET Nombre = @Nombre, Apellido = @Apellido, Telefono = @Telefono, " +
                               "Correo = @Correo, Direccion = @Direccion Where ID = @ID";
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

                MessageBox.Show("Contacto actualizado exitosamente");
                CargarContactos();
                //LimpiarCampos();

            }
        }

        //Evento para Cargar datos seleccionados y cambiar el boton agregar a actualizar
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


        // David

        // Método para cargar los contactos en el DataGridView
        private void CargarContactos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Contactos";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvContactos.DataSource = dt;
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
                }
            }
            else
            {
                MessageBox.Show("Seleccione un contacto para eliminar.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
        //Aaron

// Método para buscar contactos por nombre o apellido (pon tu codigo abajo de esto)



//Starling
//Método para limpiar los campos de texto (pon tu codigo abajo de esto)






