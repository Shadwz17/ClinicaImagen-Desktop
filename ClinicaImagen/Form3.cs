using IronPdf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ClinicaImagen.FormLogin;

namespace ClinicaImagen
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(MainFunc.connString);
            connection.Open();

            MessageBox.Show(informacion.correoLogin);
            var loginQuery = new MySqlCommand($"SELECT nombre FROM usuarios WHERE correo =\"{informacion.correoLogin}\"", connection);
            var reader = loginQuery.ExecuteReader();
            reader.Read();
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            MessageBox.Show(projectDirectory);

            PdfDocument doc = PdfDocument.FromFile(projectDirectory + "\\Files\\template-pdf.pdf");
            var form = doc.Form;


            form.Fields[0].Value = (string)reader["nombre"];
            form.Fields[1].Value = txtPaciente.Text;

            //Generos
            if (M.Checked)
            {
                form.Fields[2].Value = "1";
            }
            else if (F.Checked)
            {
                form.Fields[4].Value = "1";
            }

            MessageBox.Show(form.GetFieldByName("Fecha de Nacimiento").ToString());
            form.Fields[3].Value = datePicker.Value.ToString("dd/MM/yyyy");

            if (A1.Checked)
            {
                form.Fields[5].Value = "1";
            }
            else if (sS1.Checked)
            {
                form.Fields[6].Value = "1";

            }
            else
            {
                form.Fields[7].Value = "1";
            }

            if (Lim_Ninguna.Checked)
            {
                form.Fields[8].Value = "1";
            }
            else
            {
                form.Fields[9].Value = "1";
                //Dientes
                List<CheckBox> checkboxesDientes1 = new List<CheckBox>();

                    foreach(var c in groupBox17.Controls)
                    {
                        if (c is CheckBox)
                        {
                        checkboxesDientes1.Add((CheckBox)c);
                        }
                    }

                CheckBox[] checkBoxes = checkboxesDientes1.ToArray();
                int checksS = 0;
                int checksN = 0;
                for (int v = 0; v < checkBoxes.Length; v++)
                {

                    if (checkBoxes[v].Checked)
                    {
                        checksS++;
                    }
                    else
                    {
                        checksN++;
                    }
                }

                MessageBox.Show($"Cantidad de checkboxes\nTienen Check: {checksS}\nNo check: {checksN}");

                CheckBox[]? CBTxt = new CheckBox[32];
                bool[] vResultArray = new bool[32];

                for (int n = 0; n < CBTxt.Length; n++)
                {
                    CBTxt[n] = this.Controls.Find("CB" + (n+ 1), true).FirstOrDefault() as CheckBox;
                    MessageBox.Show($"{CBTxt[n]}");
                }

                for (int i = 0; i < CBTxt.Length; i++)
                {
                    if (CBTxt[i].ToString().Contains('1'))
                    {
                        vResultArray[i] = true;
                    }
                }

                for (int i = 0; i < vResultArray.Length; i++)
                {
                    form.Fields[i + 11].Value = vResultArray[i].ToString();
                }
            }
            form.Fields[43].Value = "0";
            form.Fields[44].Value = "1";

            //11-42 (43 for loop) 1ra tanda de dientes /44 - 75 (76 en loop) segunda tanda de Dientes


            String fechaActual = DateTime.Now.ToString("dd-MM-yyyy");
            string archivoAlTerminar = $"\\ClinicaImagenForm-{fechaActual}-{txtPaciente.Text}.pdf";
            doc.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + archivoAlTerminar);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
