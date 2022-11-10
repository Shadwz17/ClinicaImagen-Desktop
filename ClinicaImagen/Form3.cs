using IronPdf;
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

            PdfDocument doc = PdfDocument.FromFile(@"C:\Users\fedec\Desktop\Test\rellenable.pdf");
            var form = doc.Form;


            form.Fields[0].Value = (string)reader["nombre"];
            form.Fields[1].Value = txtPaciente.Text;

            //Generos
            if (M.Checked)
            {
                form.Fields[2].Value = "1";
            }
            else
            {
                form.Fields[3].Value = "1";
            }


            form.Fields[4].Value = datePicker.Text;

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
                bool[] listaDientes_1 = new bool[groupBox17.Controls.Count];




                MessageBox.Show(listaDientes_1.Length.ToString());

                for (int i = 0; i < listaDientes_1.Length; i++)
                {
                    foreach (Control c in groupBox17.Controls)
                    {
                        if ((c is CheckBox) && ((CheckBox)c).Checked)
                        {
                            listaDientes_1[i] = true;
                        }
                        else if ((c is CheckBox) && !((CheckBox)c).Checked)
                        {
                            listaDientes_1[i] = false;
                        }
                    }
                }

                for (int i = 10; i < 41; i++)
                {
                    for (int n = 0; i < listaDientes_1.Length; i++)
                    {
                        MessageBox.Show(listaDientes_1[n].ToString());
                        form.Fields[i].Value = listaDientes_1[n].ToString();
                    }
                }
            }
            form.Fields[42].Value = "0";
            form.Fields[43].Value = "1";


            doc.SaveAs($@"C:\Users\fedec\Desktop\Test\formulario-{txtPaciente.Text}-{DateTime.Today.ToString("dd-mm-yyyyy")}.pdf");
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
