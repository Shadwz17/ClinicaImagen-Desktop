using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClinicaImagen
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(MainFunc.connString);
            string nombre = txtNombre.Text;
            string correo = txtCorreo.Text;
            string passwd = txtPwd.Text;

            connection.Open();
            var checkInfo = new MySqlCommand($"SELECT correo FROM usuarios WHERE correo =\"{correo}\"", connection);
            var reader = checkInfo.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                reader.Close();
                MessageBox.Show("El correo ya existe", "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                reader.Close();
                MainFunc.Email(correo, "Registro de Usuario - FZALA",
                    "Su registro fue enviado con exito<br>Un asesor lo atendera en brevedad<br><br>Saludos cordiales,<br>FZALA");
                MainFunc.Email(MainFunc.correoAdmin, "Registro de Usuario - FZALA",
                    $"El Usuario {nombre} con email: {correo}. Se ha registrado.<br><br>Mensaje de Sistema automatizado de FZALA");


                var registerQuery = new MySqlCommand($"INSERT INTO usuarios (nombre, correo, passwd) VALUES (\"{nombre}\", \"{correo}\", \"{passwd}\")", connection);
                registerQuery.ExecuteNonQuery();
                MessageBox.Show("Usuario registrado correctamente", "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                FormLogin Login = new FormLogin();
                Login.Show();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void FormRegister_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            this.Hide();
            formLogin.Show();
        }
    }
    
}
