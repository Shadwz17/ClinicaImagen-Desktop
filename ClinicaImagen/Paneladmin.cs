using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.VisualBasic;
using EASendMail;
using System.Net.Mail;
using SmtpClient = System.Net.Mail.SmtpClient;
using MailAddress = System.Net.Mail.MailAddress;
using System.IO;

namespace ClinicaImagen
{
    public partial class Paneladmin : Form
    {
        string inputFile;
        string[] datosDoctor = new string[2];
        string[] datos = new string[2];
        public Paneladmin()
        {
            InitializeComponent();
            dgVerificados.DataSource = usuariosVerificados();
            dataGridView1.DataSource = usuariosnoVerificados();
            dgFormularios.DataSource = MostrarFormularios();
            datosUsuario();
            lblCorreo.Text = $"Correo: \n{datos[0]}";
            lblUsuario.Text = $"Usuario: \n{datos[1]}";
            
        }
        
        private void datosUsuario()
        {
            using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand($"SELECT correo, nombre from usuarios WHERE correo=\"{FormLogin.informacion.correoLogin}\"", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    for (int i = 0; i < 2; i++)
                    {
                        reader.Read();
                        datos[i] = reader.GetString(i);
                    }
                }
            }
        }

        private void datosDoctorF()
        {
            using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand($"SELECT email, nombre from doctor", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    for (int i = 0; i < 2; i++)
                    {
                        reader.Read();
                        datosDoctor[i] = reader.GetString(i);
                    }
                }
            }
        }

        private DataTable usuariosVerificados()
        {
            DataTable usuarios = new DataTable();
            using (MySqlConnection connection =  new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT nombre, correo from usuarios WHERE verificado=1", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    usuarios.Load(reader);
                }
            }
            return usuarios;
        }
        private DataTable usuariosnoVerificados()
        {
            DataTable usuarios = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT nombre, correo from usuarios WHERE verificado=0", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    usuarios.Load(reader);
                }
            }
            return usuarios;
        }

        private DataTable MostrarFormularios()
        {
            datosDoctorF();
            DataTable usuarios = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand($"SELECT num_form AS \"Numero de Formulario\", fecha AS \"Ingresado\" from formulario", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    usuarios.Load(reader);
                }
            }
            return usuarios;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Paneladmin_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgVerificados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
                string correo;
                correo = Interaction.InputBox("Ingrese el correo a verificar: ", "Verificador");


                using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
                {
                    using (MySqlCommand verificarQuery = new MySqlCommand($"UPDATE usuarios SET verificado=1 WHERE email=\"{correo}\";", connection))
                    {
                        connection.Open();
                        MySqlDataReader reader = verificarQuery.ExecuteReader();
                    }
                }


            }
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dgVerificados.DataSource = usuariosVerificados();
            dgFormularios.DataSource = MostrarFormularios();
        }

        private void btnActualizarContraseña_Click(object sender, EventArgs e)
        {
            string correo;
            correo = Interaction.InputBox("Correo a resetear: ", "Clinica Imagen - Admin");

            using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand verificarQuery = new MySqlCommand($"UPDATE usuarios SET passwd=\"CICliente\" WHERE correo=\"{correo}\";", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = verificarQuery.ExecuteReader();
                    MainFunc.Email(correo, "Reseteo de contraseña - FZALA",
                        "Su contrseña fue restablecida.<br>Contraseña nueva: CICliente<br><br>Saludos cordiales,<br>FZALA<br>Para alguna otra consulta inserte email de asesor");
                    MessageBox.Show("La contraseña por defecto es CICliente", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            inputFile = Microsoft.VisualBasic.Interaction.InputBox("Cual archivo desea descargar? (ID)", "Descarga - CI", "", 0, 0);
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Document (.pdf)|*.pdf", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    DialogResult result = MessageBox.Show("Estas seguro que quieres descargar este archivo? ", "Descargas", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        String filename = sfd.FileName;
                        Downloadfile(filename);
                    }
                }
            } 
        }

        public void Downloadfile(string file)
        {
           MySqlConnection conDownload = new MySqlConnection(MainFunc.connString);
            conDownload.Open();
            bool em = false;
            using (MySqlCommand cmd = new MySqlCommand($"SELECT archivo FROM formulario WHERE num_form = {inputFile}", conDownload))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                {
                    if (reader.Read())
                    {
                        em = true;
                        byte[] fileData = (byte[])reader.GetValue(0);
                        using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fs))
                            {
                                bw.Write(fileData);
                                bw.Close();
                            }
                        }

            if (em == false)
                        {
                            MessageBox.Show("Ningun dato ingresado", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    reader.Close();
                }
            }
        }

        private void btnGraficas_Click(object sender, EventArgs e)
        {
            formGrafico form = new formGrafico();
            this.Hide();
            form.Show();
        }

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
