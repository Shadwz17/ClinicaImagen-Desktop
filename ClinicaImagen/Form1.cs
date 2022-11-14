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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox46_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(MainFunc.connString);
            string nombre = txtNombre.Text;
            string direccion = txtCorreo.Text;
            string telefono = txtPwd.Text;
            string cedula = txtRepPwd.Text;
            connection.Open();
            
            var nuevopaciente = new MySqlCommand($"INSERT INTO paciente (nombre, direccion, telefono,cedula) VALUES (\"{nombre}\", \"{direccion}\", \"{telefono}\", \"{cedula}\")", connection);
            nuevopaciente.ExecuteNonQuery();
            MessageBox.Show("Paciente creado correctamente", "Paciente creado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();
            PanelDoctor PanelDoctor = new PanelDoctor();
            PanelDoctor.Show();
            

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            PanelDoctor PanelDoctor = new PanelDoctor();
            PanelDoctor.Show();
        }
    }
}
