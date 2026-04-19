using APISPROYECTOYATCHAY.Models;

namespace APISPROYECTOYATCHAY.Repositories.Interfaces
{
    public interface ISimulationContentRepository
    {
        Task<SimulationContent?> ObtenerPorIdAsync(int idContent);
        Task<SimulationContent?> ObtenerPorFaseAsync(int fase);
        Task<List<SimulationContent>> ObtenerTodosPorFaseAsync(int fase);
        Task<List<SimulationContent>> ObtenerTodosAsync();
        Task<int> InsertarAsync(SimulationContent content);
    }
}
