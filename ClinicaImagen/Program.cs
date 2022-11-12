using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogin());

        }
    }
    public class MainFunc
    {
        public static String correoAdmin = "ascheiber2k04@gmail.com";
        public static MySqlConnection? connection;
        public static string connString = "server=localhost;database=clinicaimagen_dev;uid=root;pwd=\"\"";

        internal static void Email(string EmailUser, string Subject, string Body)
        {
            try
            {
                MailMessage newMail = new MailMessage();
                // use the Gmail SMTP Host
                SmtpClient client = new SmtpClient("smtp.gmail.com");

                // Follow the RFS 5321 Email Standard
                newMail.From = new MailAddress("tecnologicfzala@gmail.com", "Clinica Imagen");

                newMail.To.Add(EmailUser);// declare the email subject

                newMail.Subject = Subject; // use HTML for the email body

                newMail.IsBodyHtml = true; newMail.Body = Body;

                // enable SSL for encryption across channels
                client.EnableSsl = true;
                // Port 465 for SSL communication
                client.Port = 587;
                // Provide authentication information with Gmail SMTP server to authenticate your sender account
                client.Credentials = new System.Net.NetworkCredential("tecnologicfzala@gmail.com", "tmbwbbgrftjkrhgg");

                client.Send(newMail); // Send the constructed mail
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error -" + ex);
            }
        }
    }
}

