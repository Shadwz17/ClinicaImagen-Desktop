using System.Data;
using System.Threading.Tasks;

namespace ClinicaImagen.Services
{
    public interface IUsuarioService
    {
        Task<DataTable> ObtenerUsuariosVerificadosAsync();
        Task<DataTable> ObtenerUsuariosNoVerificadosAsync();
        Task<DataTable> ObtenerFormulariosAsync();
        Task<string[]> ObtenerDatosUsuarioAsync(string correo);
        Task ActualizarVerificacionAsync(string correo);
        Task ResetearContrasenaAsync(string correo);
        Task<string?> ObtenerCargoAsync(string correo, string password);
    }
}
