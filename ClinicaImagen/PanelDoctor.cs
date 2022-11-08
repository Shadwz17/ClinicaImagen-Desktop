using MySql.Data.MySqlClient;
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
    public partial class PanelDoctor : Form
    {
        string[] datos = new string[2];
        public PanelDoctor()
        { 
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }

        void Pacientes()
        {
            using (MySqlConnection connection = new MySqlConnection(MainBD.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand($"SELECT nombre, direccion, telefono FROM paciente WHERE idD=\"{FormLogin.informacion.correoLogin}\"", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    for (int i = 0; i < 2; i++)
                    {
                        reader.Read();
                        datos[i] = reader.GetString(i);
                    }
                }
            }
        }
    }
}
