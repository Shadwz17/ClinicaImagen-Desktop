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
        private readonly IUsuarioService _usuarioService;
        private readonly IPacienteService _pacienteService;

        public FormLogin(IUsuarioService? usuarioService = null, IPacienteService? pacienteService = null)
        {
            InitializeComponent();
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
            var correo_form = txtUser.Text;
            var passwd_form = txtPasswd.Text;
            var cargo = await _usuarioService.ObtenerCargoAsync(correo_form, passwd_form);

            if (cargo == "Asesor")
            {
                Paneladmin paneladmin = new Paneladmin(_usuarioService);
                this.Hide();
                paneladmin.Show();
            }
            else if (cargo == "Doctor")
            {
                PanelDoctor form4 = new PanelDoctor(_pacienteService);
                this.Hide();
                form4.Show();
            }

            else
            {
                MessageBox.Show("Su usuario/contraseña son invalidos o no se encuentra verificado en este momento", "Error de consulta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
