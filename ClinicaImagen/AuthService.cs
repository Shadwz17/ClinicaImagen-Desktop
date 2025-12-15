using System;
using System.Data;
using System.Net.Mail;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace ClinicaImagen
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class MySqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }

    public record AuthResult(bool Success, string? ErrorMessage = null, string? Role = null);

    public class AuthService
    {
        private readonly IConnectionFactory _connectionFactory;

        public AuthService(string connectionString)
            : this(new MySqlConnectionFactory(connectionString))
        {
        }

        public AuthService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public AuthResult AuthenticateUser(string email, string password)
        {
            var validationResult = ValidateInputs(email, password);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT cargo, passwd FROM usuarios WHERE correo = @correo LIMIT 1";
            var correoParam = command.CreateParameter();
            correoParam.ParameterName = "@correo";
            correoParam.Value = email;
            command.Parameters.Add(correoParam);

            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return new AuthResult(false, "Credenciales inválidas.");
            }

            var storedHash = reader["passwd"].ToString();
            if (string.IsNullOrWhiteSpace(storedHash) || !VerifyPassword(storedHash!, password))
            {
                return new AuthResult(false, "Credenciales inválidas.");
            }

            var role = reader["cargo"].ToString();
            return new AuthResult(true, Role: role);
        }

        public AuthResult RegisterUser(string name, string email, string password)
        {
            var validationResult = ValidateInputs(email, password);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return new AuthResult(false, "El nombre es obligatorio.");
            }

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            using (var checkCommand = connection.CreateCommand())
            {
                checkCommand.CommandText = "SELECT correo FROM usuarios WHERE correo = @correo LIMIT 1";
                var correoParam = checkCommand.CreateParameter();
                correoParam.ParameterName = "@correo";
                correoParam.Value = email;
                checkCommand.Parameters.Add(correoParam);

                using var reader = checkCommand.ExecuteReader();
                if (reader.Read())
                {
                    return new AuthResult(false, "El correo ya existe.");
                }
            }

            var hashedPassword = HashPassword(password);

            using (var insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = "INSERT INTO usuarios (nombre, correo, passwd) VALUES (@nombre, @correo, @passwd)";

                var nombreParam = insertCommand.CreateParameter();
                nombreParam.ParameterName = "@nombre";
                nombreParam.Value = name;
                insertCommand.Parameters.Add(nombreParam);

                var correoParam = insertCommand.CreateParameter();
                correoParam.ParameterName = "@correo";
                correoParam.Value = email;
                insertCommand.Parameters.Add(correoParam);

                var passwdParam = insertCommand.CreateParameter();
                passwdParam.ParameterName = "@passwd";
                passwdParam.Value = hashedPassword;
                insertCommand.Parameters.Add(passwdParam);

                insertCommand.ExecuteNonQuery();
            }

            return new AuthResult(true);
        }

        internal static string HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            var salt = GenerateSalt();
            var hash = DeriveHash(password, salt);
            return $"{salt}:{hash}";
        }

        internal static bool VerifyPassword(string storedHash, string password)
        {
            if (string.IsNullOrWhiteSpace(storedHash) || password == null)
            {
                return false;
            }

            var parts = storedHash.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = parts[0];
            var hash = parts[1];
            try
            {
                var computedHash = DeriveHash(password, salt);
                return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(hash), Convert.FromBase64String(computedHash));
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static string DeriveHash(string password, string salt)
        {
            using var deriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(deriveBytes.GetBytes(32));
        }

        private static string GenerateSalt()
        {
            var saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        private AuthResult ValidateInputs(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return new AuthResult(false, "El correo y la contraseña son obligatorios.");
            }

            try
            {
                _ = new MailAddress(email);
            }
            catch (FormatException)
            {
                return new AuthResult(false, "El formato del correo es inválido.");
            }

            if (password.Length < 6)
            {
                return new AuthResult(false, "La contraseña debe tener al menos 6 caracteres.");
            }

            return new AuthResult(true);
        }
    }
}
