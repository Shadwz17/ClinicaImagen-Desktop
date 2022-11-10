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

namespace ClinicaImagen
{
    public partial class Paneladmin : Form
    {
        string[] datos = new string[2];
        public Paneladmin()
        {
            InitializeComponent();
            dgVerificados.DataSource = usuariosVerificados();
            dgNoVerificados.DataSource = usuariosNoVerificados();
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

        private DataTable usuariosVerificados()
        {
            DataTable usuarios = new DataTable();
            using (MySqlConnection connection =  new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT correo, passwd, nombre from usuarios WHERE verificado=1 AND cargo=\"Asesor\"", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    usuarios.Load(reader);
                }
            }
            return usuarios;
        }

        private DataTable usuariosNoVerificados()
        {
            DataTable usuarios = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT correo, passwd, nombre from usuarios WHERE verificado=0", connection))
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
            dgNoVerificados.DataSource = usuariosNoVerificados();
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
            try
            {
                MailMessage newMail = new MailMessage();
                // use the Gmail SMTP Host
                SmtpClient client = new SmtpClient("smtp.gmail.com");

                // Follow the RFS 5321 Email Standard
                newMail.From = new MailAddress("harrypotato62@gmail.com", "Futbol Sala");

                newMail.To.Add("ascheiber2k04@gmail.com");// declare the email subject

                newMail.Subject = "My First Email"; // use HTML for the email body

                newMail.IsBodyHtml = true; newMail.Body = "<h1> This is my first Templated Email in C# </h1>";

                // enable SSL for encryption across channels
                client.EnableSsl = true;
                // Port 465 for SSL communication
                client.Port = 587;
                // Provide authentication information with Gmail SMTP server to authenticate your sender account
                client.Credentials = new System.Net.NetworkCredential("harrypotato62@gmail.com", "-YourPWD-");

                client.Send(newMail); // Send the constructed mail
                MessageBox.Show("Email Sent");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error -" + ex);
            }
        }
    }
}
