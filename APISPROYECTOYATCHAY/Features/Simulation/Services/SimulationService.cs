using APISPROYECTOYATCHAY.Features.Simulation.Dtos;
using APISPROYECTOYATCHAY.Features.Simulation.Models;
using APISPROYECTOYATCHAY.Features.Simulation.Repositories;

namespace APISPROYECTOYATCHAY.Features.Simulation.Services
{
    public class SimulationService : ISimulationService
    {
        private readonly ISimulationSessionRepository _sessionRepo;
        private readonly ISimulationContentRepository _contentRepo;
        private readonly ISimulationOptionRepository _optionRepo;
        private readonly IDecisionRepository _decisionRepo;
        private readonly ILogger<SimulationService> _logger;

        public SimulationService(
            ISimulationSessionRepository sessionRepo,
            ISimulationContentRepository contentRepo,
            ISimulationOptionRepository optionRepo,
            IDecisionRepository decisionRepo,
            ILogger<SimulationService> logger)
        {
            _sessionRepo = sessionRepo;
            _contentRepo = contentRepo;
            _optionRepo = optionRepo;
            _decisionRepo = decisionRepo;
            _logger = logger;
        }

        public async Task<SimulationSessionResponseDto> IniciarSesionAsync(int idUsuario)
        {
            try
            {
                var sesionActiva = await _sessionRepo.ObtenerActivaPorUsuarioAsync(idUsuario);
                if (sesionActiva != null)
                {
                    _logger.LogInformation($"Usuario {idUsuario} ya tiene sesión activa: {sesionActiva.IdSession}");
                    return MapearASesionDto(sesionActiva);
                }

                var nuevaSesion = new SimulationSession
                {
                    IdUsuario = idUsuario,
                    FaseActual = "inicio",
                    Estado = "EN_PROGRESO",
                    PuntajeTotal = 0,
                    InicialoAt = DateTime.Now
                };

                var idSession = await _sessionRepo.InsertarAsync(nuevaSesion);
                _logger.LogInformation($"Nueva sesión creada: {idSession} para usuario {idUsuario}");

                nuevaSesion.IdSession = idSession;
                return MapearASesionDto(nuevaSesion);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al iniciar sesión: {ex.Message}");
                throw;
            }
        }

        public async Task<SimulationSessionResponseDto?> ObtenerEstadoAsync(int idSession)
        {
            try
            {
                var sesion = await _sessionRepo.ObtenerPorIdAsync(idSession);
                return sesion == null ? null : MapearASesionDto(sesion);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener estado: {ex.Message}");
                throw;
            }
        }

        public async Task<SimulationContentDto?> ObtenerContenidoAsync(int idSession, int fase)
        {
            try
            {
                var sesion = await _sessionRepo.ObtenerPorIdAsync(idSession);
                if (sesion == null)
                    throw new InvalidOperationException($"Sesión {idSession} no encontrada");

                var contenido = await _contentRepo.ObtenerPorIdAsync(fase);
                if (contenido == null)
                    throw new InvalidOperationException($"Contenido para fase {fase} no encontrado");

                var opciones = await _optionRepo.ObtenerTodasPorContenidoAsync(contenido.IdContent);
                
                var contentDto = MapearAContenidoDto(contenido);
                contentDto.Opciones = opciones;

                return contentDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener contenido: {ex.Message}");
                throw;
            }
        }

        public async Task<FeedbackResponseDto> GuardarDecisionAsync(DecisionRequestDto request)
        {
            try
            {
                var sesion = await _sessionRepo.ObtenerPorIdAsync(request.IdSession);
                if (sesion == null)
                    throw new InvalidOperationException($"Sesión {request.IdSession} no encontrada");

                var decisionExistente = await _decisionRepo.ExisteAsync(request.IdSession, request.IdContent);
                if (decisionExistente)
                {
                    _logger.LogWarning($"Intento de duplicar decisión: {request.IdSession}, {request.IdContent}");
                    throw new InvalidOperationException("Ya has respondido esta pregunta. Las decisiones son inmutables.");
                }

                var contenido = await _contentRepo.ObtenerPorIdAsync(request.IdContent);
                if (contenido == null)
                    throw new InvalidOperationException($"Contenido {request.IdContent} no encontrado");

                var opcionSeleccionada = await _optionRepo.ObtenerPorIdAsync(request.IdOption);
                if (opcionSeleccionada == null)
                    throw new InvalidOperationException($"Opción {request.IdOption} no válida");

                var decision = new Decision
                {
                    IdSession = request.IdSession,
                    IdContent = request.IdContent,
                    IdOption = request.IdOption,
                    PuntajeObtenido = opcionSeleccionada.Puntaje,
                    DecididoAt = DateTime.Now
                };

                var idDecision = await _decisionRepo.InsertarAsync(decision);
                await _sessionRepo.ActualizarPuntajeAsync(request.IdSession, opcionSeleccionada.Puntaje);

                var sesionActualizada = await _sessionRepo.ObtenerPorIdAsync(request.IdSession);

                _logger.LogInformation($"Decisión guardada: {idDecision}");

                return new FeedbackResponseDto
                {
                    IdDecision = idDecision,
                    PuntajeObtenido = opcionSeleccionada.Puntaje,
                    Titulo = opcionSeleccionada.Feedback ?? "Respuesta",
                    Texto = opcionSeleccionada.Resultado ?? "Procesada",
                    Resultado = opcionSeleccionada.Nivel ?? "completado",
                    PuntajeTotalActualizado = sesionActualizada?.PuntajeTotal ?? 0,
                    PuedeSiguiente = !string.IsNullOrEmpty(opcionSeleccionada.SiguientePreguntaId)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al guardar decisión: {ex.Message}");
                throw;
            }
        }

        public async Task<SimulationStatusDto?> ObtenerHistorialAsync(int idSession)
        {
            try
            {
                var sesion = await _sessionRepo.ObtenerPorIdAsync(idSession);
                if (sesion == null)
                    return null;

                var decisiones = await _decisionRepo.ObtenerTodosPorSesionAsync(idSession);

                return new SimulationStatusDto
                {
                    IdSession = sesion.IdSession,
                    FaseActual = sesion.FaseActual,
                    Estado = sesion.Estado,
                    PuntajeTotal = sesion.PuntajeTotal,
                    DecisionesPrevias = decisiones.Select(d => new DecisionHistoryDto
                    {
                        IdDecision = d.IdDecision,
                        IdOption = d.IdOption,
                        PuntajeObtenido = d.PuntajeObtenido,
                        DecididoAt = d.DecididoAt
                    }).ToList(),
                    TotalDecisiones = decisiones.Count
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener historial: {ex.Message}");
                throw;
            }
        }

        private SimulationSessionResponseDto MapearASesionDto(SimulationSession sesion)
        {
            return new SimulationSessionResponseDto
            {
                IdSession = sesion.IdSession,
                IdUsuario = sesion.IdUsuario,
                FaseActual = sesion.FaseActual,
                Estado = sesion.Estado,
                PuntajeTotal = sesion.PuntajeTotal,
                InicialoAt = sesion.InicialoAt,
                FinalizadoAt = sesion.FinalizadoAt
            };
        }

        private SimulationContentDto MapearAContenidoDto(SimulationContent contenido)
        {
            return new SimulationContentDto
            {
                IdContent = contenido.IdContent,
                IdContentExterno = contenido.IdContentExterno,
                Fase = contenido.Fase,
                Categoria = contenido.Categoria,
                Tipo = contenido.Tipo,
                Titulo = contenido.Titulo,
                Intro = contenido.Intro,
                Pregunta = contenido.Pregunta,
                Orden = contenido.Orden
            };
        }
    }
}
