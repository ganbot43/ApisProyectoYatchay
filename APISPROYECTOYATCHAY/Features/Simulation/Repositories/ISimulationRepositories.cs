using APISPROYECTOYATCHAY.Features.Simulation.Models;

namespace APISPROYECTOYATCHAY.Features.Simulation.Repositories
{
    public interface ISimulationContentRepository
    {
        Task<SimulationContent?> ObtenerPorIdAsync(int idContent);
        Task<SimulationContent?> ObtenerPorFaseAsync(int fase);
        Task<List<SimulationContent>> ObtenerTodosPorFaseAsync(int fase);
        Task<int> InsertarAsync(SimulationContent content);
    }

    public interface ISimulationSessionRepository
    {
        Task<SimulationSession?> ObtenerPorIdAsync(int idSession);
        Task<SimulationSession?> ObtenerActivaPorUsuarioAsync(int idUsuario);
        Task<int> InsertarAsync(SimulationSession session);
        Task<bool> ActualizarFaseAsync(int idSession, int faseNueva);
        Task<bool> ActualizarPuntajeAsync(int idSession, int puntajeAñadido);
    }

    public interface IDecisionRepository
    {
        Task<Decision?> ObtenerPorIdAsync(int idDecision);
        Task<List<Decision>> ObtenerTodosPorSesionAsync(int idSession);
        Task<int> InsertarAsync(Decision decision);
        Task<bool> ExisteAsync(int idSession, int idContent);
    }
}
