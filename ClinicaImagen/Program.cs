using System;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Net.Mail;

namespace ClinicaImagen
{    
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var appSettings = LoadConfiguration();
            MainFunc.Configure(appSettings);

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogin());

        }

        private static AppSettings LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            if (appSettings == null)
            {
                throw new InvalidOperationException("No se encontró la sección AppSettings en la configuración.");
            }

            return appSettings;
        }
    }
    public class MainFunc
    {
        public static string correoAdmin { get; private set; } = string.Empty;
        public static MySqlConnection? connection;
        public static string connString { get; private set; } = string.Empty;
        private static SmtpSettings smtpSettings = new();

        public static void Configure(AppSettings appSettings)
        {
            if (appSettings == null)
            {
                throw new ArgumentNullException(nameof(appSettings));
            }

            if (string.IsNullOrWhiteSpace(appSettings.ConnectionString))
            {
                throw new InvalidOperationException("La cadena de conexión no está configurada.");
            }

            if (string.IsNullOrWhiteSpace(appSettings.AdminEmail))
            {
                throw new InvalidOperationException("El correo del administrador no está configurado.");
            }

            connString = appSettings.ConnectionString;
            correoAdmin = appSettings.AdminEmail;
            smtpSettings = appSettings.Smtp ?? new SmtpSettings();
        }

        internal static void Email(string EmailUser, string Subject, string Body)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(smtpSettings.Host) ||
                    string.IsNullOrWhiteSpace(smtpSettings.UserName) ||
                    string.IsNullOrWhiteSpace(smtpSettings.Password))
                {
                    throw new InvalidOperationException("La configuración SMTP no está completa.");
                }

                MailMessage newMail = new MailMessage();
                SmtpClient client = new SmtpClient(smtpSettings.Host);

                newMail.From = new MailAddress(smtpSettings.UserName, "Clinica Imagen");

                newMail.To.Add(EmailUser);

                newMail.Subject = Subject;

                newMail.IsBodyHtml = true;
                newMail.Body = Body;

                client.EnableSsl = smtpSettings.EnableSsl;
                client.Port = smtpSettings.Port;
                client.Credentials = new System.Net.NetworkCredential(smtpSettings.UserName, smtpSettings.Password);

                client.Send(newMail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error -" + ex);
            }
        }
    }

    public class AppSettings
    {
        public string? ConnectionString { get; set; }
        public string? AdminEmail { get; set; }
        public SmtpSettings Smtp { get; set; } = new();
    }

    public class SmtpSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

