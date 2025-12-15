using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace ClinicaImagen.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<DataTable> ObtenerUsuariosVerificadosAsync()
        {
            return await CargarTablaAsync("SELECT nombre, correo from usuarios WHERE verificado=1").ConfigureAwait(false);
        }

        public async Task<DataTable> ObtenerUsuariosNoVerificadosAsync()
        {
            return await CargarTablaAsync("SELECT nombre, correo from usuarios WHERE verificado=0").ConfigureAwait(false);
        }

        public async Task<DataTable> ObtenerFormulariosAsync()
        {
            return await CargarTablaAsync("SELECT num_form AS 'Numero de Formulario', fecha AS 'Ingresado' from formulario").ConfigureAwait(false);
        }

        public async Task<string[]> ObtenerDatosUsuarioAsync(string correo)
        {
            string[] datos = new string[2];
            using MySqlConnection connection = new(_connectionString);
            using MySqlCommand cmd = new("SELECT correo, nombre from usuarios WHERE correo=@correo", connection);
            cmd.Parameters.AddWithValue("@correo", correo);
            await connection.OpenAsync().ConfigureAwait(false);
            using MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            if (await reader.ReadAsync().ConfigureAwait(false))
            {
                datos[0] = reader.GetString(0);
                datos[1] = reader.GetString(1);
            }
            return datos;
        }

        public async Task ActualizarVerificacionAsync(string correo)
        {
            using MySqlConnection connection = new(_connectionString);
            using MySqlCommand verificarQuery = new("UPDATE usuarios SET verificado=1 WHERE email=@correo;", connection);
            verificarQuery.Parameters.AddWithValue("@correo", correo);
            await connection.OpenAsync().ConfigureAwait(false);
            await verificarQuery.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        public async Task ResetearContrasenaAsync(string correo)
        {
            using MySqlConnection connection = new(_connectionString);
            using MySqlCommand resetQuery = new("UPDATE usuarios SET passwd='CICliente' WHERE correo=@correo;", connection);
            resetQuery.Parameters.AddWithValue("@correo", correo);
            await connection.OpenAsync().ConfigureAwait(false);
            await resetQuery.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        public async Task<string?> ObtenerCargoAsync(string correo, string password)
        {
            using MySqlConnection connection = new(_connectionString);
            using MySqlCommand loginQuery = new("SELECT cargo FROM usuarios WHERE correo=@correo AND passwd=@password", connection);
            loginQuery.Parameters.AddWithValue("@correo", correo);
            loginQuery.Parameters.AddWithValue("@password", password);
            await connection.OpenAsync().ConfigureAwait(false);
            using MySqlDataReader reader = (MySqlDataReader)await loginQuery.ExecuteReaderAsync().ConfigureAwait(false);
            if (await reader.ReadAsync().ConfigureAwait(false))
            {
                return reader["cargo"].ToString();
            }

            return null;
        }

        private async Task<DataTable> CargarTablaAsync(string query)
        {
            DataTable usuarios = new();
            using MySqlConnection connection = new(_connectionString);
            using MySqlCommand cmd = new(query, connection);
            await connection.OpenAsync().ConfigureAwait(false);
            using MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            usuarios.Load(reader);
            return usuarios;
        }
    }
}
