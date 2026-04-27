using APISPROYECTOYATCHAY.Features.Simulation.Models;

namespace APISPROYECTOYATCHAY.Features.Simulation.Repositories
{
    public interface ISimulationContentRepository
    {
        Task<SimulationContent?> ObtenerPorIdAsync(int idContent);
        Task<SimulationContent?> ObtenerPorIdExternoAsync(string idContentExterno);
        Task<SimulationContent?> ObtenerPorFaseAsync(string fase);
        Task<List<SimulationContent>> ObtenerTodosPorFaseAsync(string fase);
        Task<int> InsertarAsync(SimulationContent content);
    }

    public interface ISimulationOptionRepository
    {
        Task<SimulationOption?> ObtenerPorIdAsync(int idOption);
        Task<List<SimulationOption>> ObtenerTodasPorContenidoAsync(int idContent);
        Task<SimulationOption?> ObtenerPorIdExternoAsync(string idOptionExterno);
        Task<int> InsertarAsync(SimulationOption option);
    }

    public interface ISimulationSessionRepository
    {
        Task<SimulationSession?> ObtenerPorIdAsync(int idSession);
        Task<SimulationSession?> ObtenerActivaPorUsuarioAsync(int idUsuario);
        Task<int> InsertarAsync(SimulationSession session);
        Task<bool> ActualizarFaseAsync(int idSession, string faseNueva);
        Task<bool> ActualizarPuntajeAsync(int idSession, decimal puntajeAñadido);
    }

    public interface IDecisionRepository
    {
        Task<Decision?> ObtenerPorIdAsync(int idDecision);
        Task<List<Decision>> ObtenerTodosPorSesionAsync(int idSession);
        Task<int> InsertarAsync(Decision decision);
        Task<bool> ExisteAsync(int idSession, int idContent);
    }
}
