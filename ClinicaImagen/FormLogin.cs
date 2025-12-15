using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClinicaImagen.Services;

namespace ClinicaImagen
{
    public partial class FormLogin : Form
    {
        private readonly AuthService _authService = new AuthService(MainFunc.connString);

        public FormLogin()
        private readonly IUsuarioService _usuarioService;
        private readonly IPacienteService _pacienteService;

        public FormLogin(IUsuarioService? usuarioService = null, IPacienteService? pacienteService = null)
        {
            InitializeComponent();
            UIStyles.ApplyFormStyles(this);
            _usuarioService = usuarioService ?? AppServices.UsuarioService ?? throw new InvalidOperationException("UsuarioService no configurado.");
            _pacienteService = pacienteService ?? AppServices.PacienteService ?? throw new InvalidOperationException("PacienteService no configurado.");
        }


        public static class informacion
        {
            public static string? correoLogin { get; set; }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
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
            var correo_form = txtUser.Text;
            var passwd_form = txtPasswd.Text;
            var cargo = await _usuarioService.ObtenerCargoAsync(correo_form, passwd_form);

            if (cargo == "Asesor")
            {
                MainContainer.Current?.ShowView(new Paneladmin());
                Paneladmin paneladmin = new Paneladmin(_usuarioService);
                this.Hide();
                paneladmin.Show();
            }
            else if (cargo == "Doctor")
            {
                MainContainer.Current?.ShowView(new PanelDoctor());
                PanelDoctor form4 = new PanelDoctor(_pacienteService);
                this.Hide();
                form4.Show();
            }

            else
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
