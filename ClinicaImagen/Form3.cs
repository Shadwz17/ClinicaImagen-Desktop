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
             form.Fields[11].Value = CB1.Checked.ToString();
                form.Fields[12].Value = CB2.Checked.ToString();
                form.Fields[13].Value = CB3.Checked.ToString();
                form.Fields[14].Value = CB4.Checked.ToString();
                form.Fields[15].Value = CB5.Checked.ToString();
                form.Fields[16].Value = CB6.Checked.ToString();
                form.Fields[17].Value = CB7.Checked.ToString();
                form.Fields[18].Value = CB8.Checked.ToString();
                form.Fields[19].Value = CB9.Checked.ToString();
                form.Fields[20].Value = CB10.Checked.ToString();
                form.Fields[21].Value = CB11.Checked.ToString();
                form.Fields[22].Value = CB12.Checked.ToString();
                form.Fields[23].Value = CB13.Checked.ToString();
                form.Fields[24].Value = CB14.Checked.ToString();
                form.Fields[25].Value = CB15.Checked.ToString();
                form.Fields[26].Value = CB16.Checked.ToString();
                form.Fields[27].Value = CB17.Checked.ToString();
                form.Fields[28].Value = CB18.Checked.ToString();
                form.Fields[29].Value = CB19.Checked.ToString();
                form.Fields[30].Value = CB20.Checked.ToString();
                form.Fields[31].Value = CB21.Checked.ToString();
                form.Fields[32].Value = CB22.Checked.ToString();
                form.Fields[33].Value = CB23.Checked.ToString();
                form.Fields[34].Value = CB24.Checked.ToString();
                form.Fields[35].Value = CB25.Checked.ToString();
                form.Fields[36].Value = CB26.Checked.ToString();
                form.Fields[37].Value = CB27.Checked.ToString();
                form.Fields[38].Value = CB28.Checked.ToString();
                form.Fields[39].Value = CB29.Checked.ToString();
                form.Fields[40].Value = CB30.Checked.ToString();
                form.Fields[41].Value = CB31.Checked.ToString();
                form.Fields[42].Value = CB32.Checked.ToString();

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
