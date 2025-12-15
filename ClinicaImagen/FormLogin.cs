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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            UIStyles.ApplyFormStyles(this);
        }


        public static class informacion
        {
            public static string? correoLogin { get; set; }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(MainFunc.connString);
            connection.Open();
            informacion.correoLogin = txtUser.Text;
            var correo_form = txtUser.Text;
            var passwd_form = txtPasswd.Text;
            var loginQuery = new MySqlCommand($"SELECT cargo FROM usuarios WHERE correo=\"{correo_form}\" AND passwd=\"{passwd_form}\"", connection);
            var reader = loginQuery.ExecuteReader();
            reader.Read();

            if ((reader.HasRows && reader["cargo"].ToString() == "Asesor"))
            {
                MainContainer.Current?.ShowView(new Paneladmin());
            }
            else if (reader.HasRows && reader["cargo"].ToString() == "Doctor")
            {
                MainContainer.Current?.ShowView(new PanelDoctor());
            }
            
            else
            {
                MessageBox.Show("Su usuario/contraseña son invalidos o no se encuentra verificado en este momento", "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            MainContainer.Current?.ShowView(new FormRegister());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
         
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
          
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MainContainer.Current?.ShowView(new ManualUsuario());
        }
    }
}
