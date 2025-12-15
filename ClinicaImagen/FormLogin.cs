using System;
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
        private readonly AuthService _authService = new AuthService(MainFunc.connString);

        public FormLogin()
        {
            InitializeComponent();
        }


        public static class informacion
        {
            public static string? correoLogin { get; set; }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            informacion.correoLogin = txtUser.Text;
            var correoForm = txtUser.Text;
            var passwdForm = txtPasswd.Text;

            try
            {
                var result = _authService.AuthenticateUser(correoForm, passwdForm);
                if (!result.Success)
                {
                    MessageBox.Show(result.ErrorMessage, "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (result.Role == "Asesor")
                {
                    Paneladmin paneladmin = new Paneladmin();
                    this.Hide();
                    paneladmin.Show();
                }
                else if (result.Role == "Doctor")
                {
                    PanelDoctor form4 = new PanelDoctor();
                    this.Hide();
                    form4.Show();
                }
                else
                {
                    MessageBox.Show("No se pudo determinar el rol del usuario.", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormRegister Registro = new FormRegister();
            Registro.Show();
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
  this.Hide();
            ManualUsuario Manual = new ManualUsuario();
            Manual.Show();
        }
    }
}
