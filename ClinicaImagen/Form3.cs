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
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Net.Mime;

namespace ClinicaImagen
{
    public partial class Form3 : Form
    {
        MySqlCommand cmd;
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
            var loginQuery = new MySqlCommand($"SELECT id, nombre FROM doctor WHERE email=\"{informacion.correoLogin}\"", connection);
            var reader = loginQuery.ExecuteReader();
            reader.Read();
            int doctorID = (int)reader["id"];
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

           
            form.Fields[5].Value =A1.Checked.ToString();
            form.Fields[6].Value = sS1.Checked.ToString();
            form.Fields[7].Value = sl1.Checked.ToString();
            

            if (Lim_Ninguna.Checked)
            {
                form.Fields[8].Value = "1";
            }
            else
            {
                form.Fields[9].Value = "1";
                form.Fields[11].Value = CB1.Checked.ToString();
                form.Fields[12].Value = CB9.Checked.ToString();
                form.Fields[13].Value = CB2.Checked.ToString();
                form.Fields[14].Value = CB10.Checked.ToString();
                form.Fields[15].Value = CB3.Checked.ToString();
                form.Fields[16].Value = CB11.Checked.ToString();
                form.Fields[17].Value = CB4.Checked.ToString();
                form.Fields[18].Value = CB12.Checked.ToString();
                form.Fields[19].Value = CB5.Checked.ToString();
                form.Fields[20].Value = CB13.Checked.ToString();
                form.Fields[21].Value = CB6.Checked.ToString();
                form.Fields[22].Value = CB14.Checked.ToString();
                form.Fields[23].Value = CB7.Checked.ToString();
                form.Fields[24].Value = CB15.Checked.ToString();
                form.Fields[25].Value = CB8.Checked.ToString();
                form.Fields[26].Value = CB16.Checked.ToString();

                form.Fields[27].Value = CB17.Checked.ToString();
                form.Fields[28].Value = CB25.Checked.ToString();
                form.Fields[29].Value = CB18.Checked.ToString();
                form.Fields[30].Value = CB26.Checked.ToString();
                form.Fields[31].Value = CB19.Checked.ToString();
                form.Fields[32].Value = CB27.Checked.ToString();
                form.Fields[33].Value = CB20.Checked.ToString();
                form.Fields[34].Value = CB28.Checked.ToString();
                form.Fields[35].Value = CB21.Checked.ToString();
                form.Fields[36].Value = CB29.Checked.ToString();
                form.Fields[37].Value = CB22.Checked.ToString();
                form.Fields[38].Value = CB30.Checked.ToString();
                form.Fields[39].Value = CB23.Checked.ToString();
                form.Fields[40].Value = CB31.Checked.ToString();
                form.Fields[41].Value = CB24.Checked.ToString();
                form.Fields[42].Value = CB32.Checked.ToString();
            }
            if (ataches_colocar.Checked)
            {
                form.Fields[10].Value = "1";
            }
            else
            {
                form.Fields[43].Value = "1";
                form.Fields[44].Value = CBB1.Checked.ToString();
                form.Fields[45].Value = CBB2.Checked.ToString();
                form.Fields[46].Value = CBB3.Checked.ToString();
                form.Fields[47].Value = CBB4.Checked.ToString();
                form.Fields[48].Value = CBB5.Checked.ToString();
                form.Fields[49].Value = CBB6.Checked.ToString();
                form.Fields[50].Value = CBB7.Checked.ToString();
                form.Fields[51].Value = CBB8.Checked.ToString();
                form.Fields[52].Value = CBB9.Checked.ToString();
                form.Fields[53].Value = CBB10.Checked.ToString();
                form.Fields[54].Value = CBB11.Checked.ToString();
                form.Fields[55].Value = CBB12.Checked.ToString();
                form.Fields[56].Value = CBB13.Checked.ToString();
                form.Fields[57].Value = CBB14.Checked.ToString();
                form.Fields[58].Value = CBB15.Checked.ToString();
                form.Fields[59].Value = CBB16.Checked.ToString();

                form.Fields[60].Value = CBB17.Checked.ToString();
                form.Fields[61].Value = CBB18.Checked.ToString();
                form.Fields[62].Value = CBB19.Checked.ToString();
                form.Fields[63].Value = CBB20.Checked.ToString();
                form.Fields[64].Value = CBB21.Checked.ToString();
                form.Fields[65].Value = CBB22.Checked.ToString();
                form.Fields[66].Value = CBB23.Checked.ToString();
                form.Fields[67].Value = CBB24.Checked.ToString();
                form.Fields[68].Value = CBB25.Checked.ToString();
                form.Fields[69].Value = CBB26.Checked.ToString();
                form.Fields[70].Value = CBB27.Checked.ToString();
                form.Fields[71].Value = CBB28.Checked.ToString();
                form.Fields[72].Value = CBB29.Checked.ToString();
                form.Fields[73].Value = CBB30.Checked.ToString();
                form.Fields[74].Value = CBB31.Checked.ToString();
                form.Fields[75].Value = CBB32.Checked.ToString();

            }
               form.Fields[76].Value = radioButton11.Checked.ToString();
               form.Fields[77].Value = radioButton10.Checked.ToString();
            
               form.Fields[78].Value = M3.Checked.ToString();
               form.Fields[79].Value = M4.Checked.ToString();
           
            if (Mo.Checked)
            {
                form.Fields[80].Value = "1";
                M6.Enabled = false;
                M9.Enabled = false;
                mm.Enabled = false;
                mm2.Enabled = false;
            }
            else if (M6.Checked)
            {
                form.Fields[81].Value = "1";
            }
            else if (M9.Checked)
            {
                form.Fields[82].Value = "1";
            }



            if (mm.Checked)
            {
                form.Fields[83].Value = "1";
            }
            else if (mm2.Checked)
            {
                form.Fields[84].Value = "1";
            }
            if (Mo2.Checked)
            {
                form.Fields[85].Value = "1";
               
            }


            else if (M0.Checked)
            {
                form.Fields[86].Value = "1";
            }
            else if (M01.Checked)
            {
                form.Fields[87].Value = "1";
            }
            if (mm3.Checked)
            {
                form.Fields[88].Value = "1";
            }
            else if (mm4.Checked)
            {
                form.Fields[89].Value = "1";
            }
            if (D1.Checked)
            {
                form.Fields[90].Value = "1";
                
            }
            else if (D2.Checked)
            {
                form.Fields[91].Value = "1";
            }
            else if (St1.Checked)
            {
                form.Fields[92].Value = "1";
            }
            else if (O1.Checked)
            {
                form.Fields[93].Value = "1";
                form.Fields[95].Value = tb3.Text;
            }
       
            form.Fields[94].Value = C1.Checked.ToString();
            form.Fields[96].Value = D3.Checked.ToString();

            form.Fields[97].Value =  C2.Checked.ToString();
            form.Fields[98].Value = D4.Checked.ToString();
            
            form.Fields[99].Value = Pp1.Checked.ToString();
            form.Fields[100].Value = Sj1.Checked.ToString();
            form.Fields[101].Value = nn1.Checked.ToString();

            form.Fields[102].Value = Pp3.Checked.ToString();
            form.Fields[103].Value = Sj3.Checked.ToString();
            form.Fields[104].Value = nn3.Checked.ToString();

            form.Fields[105].Value = Pp2.Checked.ToString();
            form.Fields[106].Value = Sj2.Checked.ToString();
            form.Fields[107].Value = nn2.Checked.ToString();

            form.Fields[108].Value = Pp4.Checked.ToString();
            form.Fields[109].Value = Sj4.Checked.ToString();
            form.Fields[110].Value = nn4.Checked.ToString();

            form.Fields[111].Value = Pp5.Checked.ToString();
            form.Fields[112].Value = Sj5.Checked.ToString();
            form.Fields[113].Value = nn5.Checked.ToString();

            form.Fields[114].Value = Pp6.Checked.ToString();
            form.Fields[115].Value = Sj6.Checked.ToString();
            form.Fields[116].Value = nn6.Checked.ToString();



            form.Fields[117].Value = M1.Checked.ToString();
            form.Fields[118].Value =  M2.Checked.ToString();
            
            form.Fields[119].Value = textBox1.Text;
            form.Fields[121].Value = textBox2.Text;







            MainFunc.Email(MainFunc.correoAdmin, "Nuevo formulario - Clinica Imagen", $"{(string)reader["nombre"]}, ha enviado un nuevo formulario,$\"{(string)reader["ruta"]}.");
            reader.Close();
            String fechaActual = DateTime.Now.ToString("dd-MM-yyyy");
            string archivoAlTerminar = $"\\ClinicaImagenForm-{fechaActual}-{txtPaciente.Text}.pdf";
            doc.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + archivoAlTerminar);

            FileStream fstream = File.OpenRead(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + archivoAlTerminar);
            byte[] contents = new byte[fstream.Length];
            fstream.Read(contents, 0, (int)fstream.Length);
            fstream.Close();
            using (cmd = new MySqlCommand($"insert into formulario(archivo, fecha, doctor) values(@archivo, '{DateTime.Now.ToString("yyyy-MM-dd")}', '{doctorID}')", connection))
            {
                MySqlDataReader reader2;
                cmd.Parameters.AddWithValue("@archivo", contents);
                reader2 = cmd.ExecuteReader();
            }






            connection.Close();
            PanelDoctor formp = new PanelDoctor();
            this.Hide();
            formp.Show();
           


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

        private void ataches_colocar_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox20_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
