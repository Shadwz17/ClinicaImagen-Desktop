using System.Data;
using System.Threading.Tasks;

namespace ClinicaImagen.Services
{
    public interface IPacienteService
    {
        Task<DataTable> ObtenerPacientesAsync(string correoDoctor);
        Task<DataTable> ObtenerEntrevistasAsync(string nombrePaciente, string correoDoctor);
    }
}
