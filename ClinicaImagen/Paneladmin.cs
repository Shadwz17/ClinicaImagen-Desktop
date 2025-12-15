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
using ClinicaImagen.Services;

namespace ClinicaImagen
{
    public partial class Paneladmin : Form
    {
        string inputFile;
        string[] datos = new string[2];
        private readonly IUsuarioService _usuarioService;
        public Paneladmin(IUsuarioService? usuarioService = null)
        {
            InitializeComponent();
            _usuarioService = usuarioService ?? AppServices.UsuarioService ?? throw new InvalidOperationException("UsuarioService no configurado.");

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void Paneladmin_Load(object sender, EventArgs e)
        {
            await InicializarAsync();
        }

        private async Task InicializarAsync()
        {
            await datosUsuario();
            await CargarUsuariosAsync();
            await CargarFormulariosAsync();
        }

        private async Task datosUsuario()
        {
            datos = await _usuarioService.ObtenerDatosUsuarioAsync(FormLogin.informacion.correoLogin ?? string.Empty);
            lblCorreo.Text = $"Correo: \n{datos[0]}";
            lblUsuario.Text = $"Usuario: \n{datos[1]}";
        }

        private async Task CargarUsuariosAsync()
        {
            ToggleUsuariosLoading(true);
            dgVerificados.DataSource = await _usuarioService.ObtenerUsuariosVerificadosAsync();
            dataGridView1.DataSource = await _usuarioService.ObtenerUsuariosNoVerificadosAsync();
            ToggleUsuariosLoading(false);
        }

        private async Task CargarFormulariosAsync()
        {
            ToggleFormulariosLoading(true);
            dgFormularios.DataSource = await _usuarioService.ObtenerFormulariosAsync();
            ToggleFormulariosLoading(false);
        }

        private void ToggleUsuariosLoading(bool isLoading)
        {
            lblUsuariosLoading.Visible = isLoading;
        }

        private void ToggleFormulariosLoading(bool isLoading)
        {
            lblFormulariosLoading.Visible = isLoading;
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

        private async void btnVerificar_Click(object sender, EventArgs e)
        {
                string correo;
                correo = Interaction.InputBox("Ingrese el correo a verificar: ", "Verificador");


                await _usuarioService.ActualizarVerificacionAsync(correo);
                await CargarUsuariosAsync();
            }
        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            await CargarUsuariosAsync();
            await CargarFormulariosAsync();
        }

        private async void btnActualizarContraseña_Click(object sender, EventArgs e)
        {
            string correo;
            correo = Interaction.InputBox("Correo a resetear: ", "Clinica Imagen - Admin");

            await _usuarioService.ResetearContrasenaAsync(correo);
            MainFunc.Email(correo, "Reseteo de contraseña - FZALA",
                        "Su contrseña fue restablecida.<br>Contraseña nueva: CICliente<br><br>Saludos cordiales,<br>FZALA<br>Para alguna otra consulta inserte email de asesor");
            MessageBox.Show("La contraseña por defecto es CICliente", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
