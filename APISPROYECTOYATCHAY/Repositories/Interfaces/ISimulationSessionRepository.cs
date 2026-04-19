using APISPROYECTOYATCHAY.Models;

namespace APISPROYECTOYATCHAY.Repositories.Interfaces
{
    public interface ISimulationSessionRepository
    {
        Task<SimulationSession?> ObtenerPorIdAsync(int idSession);
        Task<SimulationSession?> ObtenerActivaPorUsuarioAsync(int idUsuario);
        Task<List<SimulationSession>> ObtenerTodosPorUsuarioAsync(int idUsuario);
        Task<int> InsertarAsync(SimulationSession session);
        Task<bool> ActualizarFaseAsync(int idSession, int faseNueva);
        Task<bool> ActualizarPuntajeAsync(int idSession, int puntajeAñadido);
        Task<bool> FinalizarSesionAsync(int idSession);
    }
}
