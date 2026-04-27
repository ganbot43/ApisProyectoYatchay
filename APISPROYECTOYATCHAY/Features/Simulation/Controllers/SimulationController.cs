using APISPROYECTOYATCHAY.Features.Simulation.Dtos;
using APISPROYECTOYATCHAY.Features.Simulation.Services;
using Microsoft.AspNetCore.Mvc;

namespace APISPROYECTOYATCHAY.Features.Simulation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimulationController : ControllerBase
    {
        private readonly ISimulationService _simulationService;

        public SimulationController(ISimulationService simulationService)
        {
            _simulationService = simulationService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] SimulationStartRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { exito = 0, mensaje = "Datos inválidos" });

            try
            {
                var sesion = await _simulationService.IniciarSesionAsync(request.IdUsuario);
                return Ok(new { exito = 1, mensaje = "Sesión iniciada", datos = sesion });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = 0, mensaje = ex.Message });
            }
        }

        [HttpGet("status/{idSession}")]
        public async Task<IActionResult> ObtenerEstado(int idSession)
        {
            try
            {
                var estado = await _simulationService.ObtenerEstadoAsync(idSession);
                if (estado == null)
                    return NotFound(new { exito = 0, mensaje = "Sesión no encontrada" });

                return Ok(new { exito = 1, mensaje = "Estado obtenido", datos = estado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = 0, mensaje = ex.Message });
            }
        }

        [HttpGet("content/{idSession}/{idContent}")]
        public async Task<IActionResult> ObtenerContenido(int idSession, int idContent)
        {
            try
            {
                var contenido = await _simulationService.ObtenerContenidoAsync(idSession, idContent);
                if (contenido == null)
                    return NotFound(new { exito = 0, mensaje = "Contenido no encontrado" });

                return Ok(new { exito = 1, mensaje = "Contenido obtenido", datos = contenido });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { exito = 0, mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = 0, mensaje = ex.Message });
            }
        }

        [HttpPost("decide")]
        public async Task<IActionResult> Decidir([FromBody] DecisionRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { exito = 0, mensaje = "Datos inválidos" });

            try
            {
                var feedback = await _simulationService.GuardarDecisionAsync(request);
                return Ok(new { exito = 1, mensaje = "Decisión guardada", datos = feedback });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new { exito = 0, mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = 0, mensaje = ex.Message });
            }
        }

        [HttpGet("history/{idSession}")]
        public async Task<IActionResult> ObtenerHistorial(int idSession)
        {
            try
            {
                var historial = await _simulationService.ObtenerHistorialAsync(idSession);
                if (historial == null)
                    return NotFound(new { exito = 0, mensaje = "Sesión no encontrada" });

                return Ok(new { exito = 1, mensaje = "Historial obtenido", datos = historial });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = 0, mensaje = ex.Message });
            }
        }
    }
}
