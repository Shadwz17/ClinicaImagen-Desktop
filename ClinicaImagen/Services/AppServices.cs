using System;

namespace ClinicaImagen.Services
{
    public static class AppServices
    {
        public static IPacienteService? PacienteService { get; private set; }
        public static IUsuarioService? UsuarioService { get; private set; }

        public static void Configure(string connectionString)
        {
            PacienteService = new PacienteService(connectionString);
            UsuarioService = new UsuarioService(connectionString);
        }
    }
}
