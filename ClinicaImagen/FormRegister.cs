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
    public partial class FormRegister : Form
    {
        private readonly AuthService _authService = new AuthService(MainFunc.connString);

        public FormRegister()
        {
            InitializeComponent();
            UIStyles.ApplyFormStyles(this);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text;
            string correo = txtCorreo.Text;
            string passwd = txtPwd.Text;

            try
            {
                var result = _authService.RegisterUser(nombre, correo, passwd);
                if (!result.Success)
                {
                    MessageBox.Show(result.ErrorMessage, "Error de registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MainFunc.Email(correo, "Registro de Usuario - FZALA",
                    "Su registro fue enviado con exito<br>Un asesor lo atendera en brevedad<br><br>Saludos cordiales,<br>FZALA");
                MainFunc.Email(MainFunc.correoAdmin, "Registro de Usuario - FZALA",
                    $"El Usuario {nombre} con email: {correo}. Se ha registrado.<br><br>Mensaje de Sistema automatizado de FZALA");

                MessageBox.Show("Usuario registrado correctamente", "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MainContainer.Current?.ShowView(new FormLogin());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            MainContainer.Current?.ShowView(new FormLogin());
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
