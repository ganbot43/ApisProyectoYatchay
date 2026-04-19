using APISPROYECTOYATCHAY.Features.Simulation.Dtos;

namespace APISPROYECTOYATCHAY.Features.Simulation.Services
{
    public interface ISimulationService
    {
        Task<SimulationSessionResponseDto> IniciarSesionAsync(int idUsuario);
        Task<SimulationSessionResponseDto?> ObtenerEstadoAsync(int idSession);
        Task<SimulationContentDto?> ObtenerContenidoAsync(int idSession, int fase);
        Task<FeedbackResponseDto> GuardarDecisionAsync(DecisionRequestDto request);
        Task<SimulationStatusDto?> ObtenerHistorialAsync(int idSession);
    }
}
