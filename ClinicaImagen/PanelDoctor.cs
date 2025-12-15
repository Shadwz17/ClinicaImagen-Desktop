using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClinicaImagen.Services;

namespace ClinicaImagen
{
    public partial class PanelDoctor : Form
    {
        public PanelDoctor()
        private readonly IPacienteService _pacienteService;

        public PanelDoctor(IPacienteService? pacienteService = null)
        {
            InitializeComponent();
            _pacienteService = pacienteService ?? AppServices.PacienteService ?? throw new InvalidOperationException("PacienteService no configurado.");
            dgvPacientes.ScrollBars = ScrollBars.Horizontal;
            dgvPacientes.DataSource = Pacientes();
            UIStyles.ApplyFormStyles(this);
            UIStyles.ApplyEmptyState(dgvPacientes, "No hay pacientes asignados todavía.");
            UIStyles.ApplyEmptyState(dgvEntrevistas, "No hay entrevistas registradas para este paciente.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            MainContainer.Current?.ShowView(form3);
        }

        DataTable resultados = new DataTable();
        string? _dgvrowValue;
        public string dgvrowValue
        {
            get { return _dgvrowValue; }
            set
            {
                if (value == _dgvrowValue) return;
                _dgvrowValue = value;
            }
        }

        private async Task<DataTable> PacientesAsync()
        {
            return await _pacienteService.ObtenerPacientesAsync(FormLogin.informacion.correoLogin ?? string.Empty);
        }

        private async void dgvPacientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnindex = dgvPacientes.CurrentCell.ColumnIndex;
            if (columnindex == 0)
            {
                _dgvrowValue = dgvPacientes.CurrentRow.Cells[0].Value.ToString();
                await MostrarEntrevistasAsync();
            }
        }

        private async void dgvEntrevistas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            await MostrarEntrevistasAsync();
        }

        private void btnAgregarPaciente_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dgvrowValue);
            Form1 form2 = new Form1();
            MainContainer.Current?.ShowView(form2);

        }

        private void btnEntrevistas_Click(object sender, EventArgs e)
        {

        }

        private async Task<DataTable> MostrarEntrevistasAsync()
        {
            ToggleEntrevistasLoading(true);
            DataTable entrevistas = await _pacienteService.ObtenerEntrevistasAsync(dgvrowValue ?? string.Empty, FormLogin.informacion.correoLogin ?? string.Empty);
            dgvEntrevistas.DataSource = entrevistas;
            ToggleEntrevistasLoading(false);
            return entrevistas;
        }

        private void dgvEntrevistas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
          
        }

        private async void PanelDoctor_Load(object sender, EventArgs e)
        {
            await CargarPacientesAsync();
        }

        private async Task CargarPacientesAsync()
        {
            TogglePacientesLoading(true);
            dgvPacientes.DataSource = await PacientesAsync();
            TogglePacientesLoading(false);
        }

        private void TogglePacientesLoading(bool isLoading)
        {
            lblPacientesLoading.Visible = isLoading;
        }

        private void ToggleEntrevistasLoading(bool isLoading)
        {
            lblEntrevistasLoading.Visible = isLoading;
        }
    }
}
