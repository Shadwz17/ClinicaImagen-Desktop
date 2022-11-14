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
        public PanelDoctor()
        { 
            InitializeComponent();
            dgvPacientes.ScrollBars = ScrollBars.Horizontal;
            dgvPacientes.DataSource = Pacientes();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.Show();
        }

        DataTable resultados = new DataTable();
        string _dgvrowValue;
        public string dgvrowValue
        {
            get { return _dgvrowValue; }
            set
            {
                if (value == _dgvrowValue) return;
                _dgvrowValue = value;
            }
        }

        DataTable Pacientes()
        {
            DataTable pacientes = new DataTable();
            using (MySqlConnection connection = new MySqlConnection(MainFunc.connString))
            {
                using (MySqlCommand cmd = new MySqlCommand($"SELECT nombre, direccion, telefono FROM paciente WHERE idD=(SELECT id FROM doctor WHERE email=\"{FormLogin.informacion.correoLogin}\")", connection))
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    pacientes.Load(reader);
                }
                return pacientes;
            }
        }

        private void dgvPacientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnindex = dgvPacientes.CurrentCell.ColumnIndex;
            if (columnindex == 0)
            {
                _dgvrowValue = dgvPacientes.CurrentRow.Cells[0].Value.ToString();
            }
        }

        private void dgvEntrevistas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Descargar desde panel doctor - TODO
        }

        private void btnAgregarPaciente_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dgvrowValue);
            Form1 form2 = new Form1();
            this.Hide();
            form2.Show();
                    
        }

        private void btnEntrevistas_Click(object sender, EventArgs e)
        {
            dgvEntrevistas.Rows.Clear();
            dgvEntrevistas.Columns.Clear();
            MySqlConnection conx = new MySqlConnection(MainFunc.connString);
            conx.Open();
            MySqlCommand cmd = new MySqlCommand($"SELECT fecha FROM entrevista WHERE idP=(SELECT id FROM paciente WHERE nombre=\"{dgvrowValue}\") " +
                                                $"AND idD=(SELECT id FROM doctor WHERE email=\"{FormLogin.informacion.correoLogin}\")  ", conx);
            MySqlDataReader reader = cmd.ExecuteReader();
            resultados.Load(reader);
            
        }

        private void dgvEntrevistas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
          
        }
    }
}
