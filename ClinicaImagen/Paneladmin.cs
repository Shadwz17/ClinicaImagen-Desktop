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
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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
            dgVerificados.DataSource = usuariosVerificados();
            dataGridView1.DataSource = usuariosnoVerificados();
            dgFormularios.DataSource = MostrarFormularios();
            datosUsuario();
            lblCorreo.Text = $"Correo: \n{datos[0]}";
            lblUsuario.Text = $"Usuario: \n{datos[1]}";
            UIStyles.ApplyFormStyles(this);
            UIStyles.ApplyEmptyState(dgVerificados, "No hay usuarios verificados para mostrar.");
            UIStyles.ApplyEmptyState(dataGridView1, "No hay usuarios pendientes de verificación.");
            UIStyles.ApplyEmptyState(dgFormularios, "No se encontraron formularios enviados.");

        }
        
        private void datosUsuario()
        {
            if (!IsValidEmail(FormLogin.informacion.correoLogin))
            {
                MessageBox.Show("El correo del usuario no es válido.", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT correo, nombre FROM usuarios WHERE correo=@correo", connection))
                {
                    cmd.Parameters.AddWithValue("@correo", FormLogin.informacion.correoLogin);
                    connection.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                datos[i] = reader.GetString(i);
                            }
                        }
                    }
                }
            }
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

            if (!IsValidEmail(correo))
            {
                MessageBox.Show("Ingrese un correo válido antes de verificar.", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
                {
                    using (MySqlCommand verificarQuery = new MySqlCommand("UPDATE usuarios SET verificado=1 WHERE email=@correo;", connection))
                    {
                        verificarQuery.Parameters.AddWithValue("@correo", correo);
                        connection.Open();
                        int affected = verificarQuery.ExecuteNonQuery();
                        if (affected > 0)
                        {
                            MessageBox.Show("Usuario verificado correctamente.", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogAudit("Verificación de usuario", $"Se verificó el correo {correo}.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró un usuario con ese correo.", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar el usuario: {ex.Message}", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnActualizar_Click(object sender, EventArgs e)
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

            if (!IsValidEmail(correo))
            {
                MessageBox.Show("Ingrese un correo válido antes de restablecer la contraseña.", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tempPassword = GenerateTemporaryPassword();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
                {
                    using (MySqlCommand verificarQuery = new MySqlCommand("UPDATE usuarios SET passwd=@passwd WHERE correo=@correo;", connection))
                    {
                        verificarQuery.Parameters.AddWithValue("@passwd", tempPassword);
                        verificarQuery.Parameters.AddWithValue("@correo", correo);
                        connection.Open();
                        int affected = verificarQuery.ExecuteNonQuery();
                        if (affected > 0)
                        {
                            MainFunc.Email(correo, "Reseteo de contraseña - FZALA",
                                $"Su contraseña fue restablecida.<br>Contraseña nueva: {tempPassword}<br><br>Saludos cordiales,<br>FZALA<br>Para alguna otra consulta inserte email de asesor");
                            MessageBox.Show("Se generó una contraseña temporal y se envió al usuario.", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogAudit("Reseteo de contraseña", $"Se restableció la contraseña del correo {correo}.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró un usuario con ese correo.", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al restablecer la contraseña: {ex.Message}", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            if (!IsValidId(inputFile, out int formId))
            {
                MessageBox.Show("Ingrese un ID numérico válido.", "Clinica Imagen - Admin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conDownload = new MySqlConnection(MainFunc.connString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT archivo FROM formulario WHERE num_form = @num_form", conDownload))
                    {
                        cmd.Parameters.AddWithValue("@num_form", formId);
                        conDownload.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.Default))
                        {
                            if (reader.Read())
                            {
                                byte[] fileData = (byte[])reader.GetValue(0);
                                using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.ReadWrite))
                                using (BinaryWriter bw = new BinaryWriter(fs))
                                {
                                    bw.Write(fileData);
                                }

                                MessageBox.Show("Archivo descargado correctamente.", "Descargas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LogAudit("Descarga de archivo", $"Se descargó el formulario con ID {formId}.");
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron archivos para el ID indicado.", "Descargas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar el archivo: {ex.Message}", "Descargas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGraficas_Click(object sender, EventArgs e)
        {
            formGrafico form = new formGrafico();
            MainContainer.Current?.ShowView(form);
        }

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {

        }

        private bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(email);
                return Regex.IsMatch(mailAddress.Address, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidId(string? idInput, out int id)
        {
            return int.TryParse(idInput, out id) && id > 0;
        }

        private string GenerateTemporaryPassword(int length = 12)
        {
            const string allowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@#$%";
            byte[] randomBytes = new byte[length];
            StringBuilder result = new StringBuilder(length);

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            foreach (byte b in randomBytes)
            {
                result.Append(allowedChars[b % allowedChars.Length]);
            }

            return result.ToString();
        }

        private void LogAudit(string action, string detail)
        {
            try
            {
                string auditPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "audit.log");
                string actor = IsValidEmail(FormLogin.informacion.correoLogin) ? FormLogin.informacion.correoLogin : "Desconocido";
                string entry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ({actor}) {action}: {detail}{Environment.NewLine}";
                File.AppendAllText(auditPath, entry);
            }
            catch
            {
                // No interrumpir el flujo de la aplicación si el registro falla.
            }
        }
    }
}
