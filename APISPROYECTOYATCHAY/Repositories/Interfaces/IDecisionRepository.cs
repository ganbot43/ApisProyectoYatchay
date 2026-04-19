using APISPROYECTOYATCHAY.Models;

namespace APISPROYECTOYATCHAY.Repositories.Interfaces
{
    public interface IDecisionRepository
    {
        Task<Decision?> ObtenerPorIdAsync(int idDecision);
        Task<Decision?> ObtenerPorSesionYContenidoAsync(int idSession, int idContent);
        Task<List<Decision>> ObtenerTodosPorSesionAsync(int idSession);
        Task<int> InsertarAsync(Decision decision);
        Task<bool> ExisteAsync(int idSession, int idContent);
    }
}
