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
    public partial class ManualUsuario2 : Form
    {
        public ManualUsuario2()
        {
            InitializeComponent();
            UIStyles.ApplyFormStyles(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainContainer.Current?.ShowView(new FormLogin());
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            MainContainer.Current?.ShowView(new ManualUsuario());
        }
    }
}
