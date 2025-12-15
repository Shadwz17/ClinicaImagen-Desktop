using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace ClinicaImagen.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly string _connectionString;

        public PacienteService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DataTable> ObtenerPacientesAsync(string correoDoctor)
        {
            DataTable pacientes = new();
            using MySqlConnection connection = new(_connectionString);
            using MySqlCommand cmd = new($"SELECT nombre, direccion, telefono FROM paciente WHERE idD=(SELECT id FROM doctor WHERE email=@correoDoctor)", connection);
            cmd.Parameters.AddWithValue("@correoDoctor", correoDoctor);
            await connection.OpenAsync().ConfigureAwait(false);
            using MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            pacientes.Load(reader);
            return pacientes;
        }

        public async Task<DataTable> ObtenerEntrevistasAsync(string nombrePaciente, string correoDoctor)
        {
            DataTable entrevistas = new();
            using MySqlConnection connection = new(_connectionString);
            using MySqlCommand cmd = new($"SELECT fecha FROM entrevista WHERE idP=(SELECT id FROM paciente WHERE nombre=@nombrePaciente) AND idD=(SELECT id FROM doctor WHERE email=@correoDoctor)", connection);
            cmd.Parameters.AddWithValue("@nombrePaciente", nombrePaciente);
            cmd.Parameters.AddWithValue("@correoDoctor", correoDoctor);
            await connection.OpenAsync().ConfigureAwait(false);
            using MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            entrevistas.Load(reader);
            return entrevistas;
        }
    }
}
