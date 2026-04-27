using APISPROYECTOYATCHAY.Features.Simulation.Models;

namespace APISPROYECTOYATCHAY.Features.Simulation.Dtos
{
    public class SimulationStartRequestDto
    {
        public int IdUsuario { get; set; }
    }

    public class DecisionRequestDto
    {
        public int IdSession { get; set; }
        public int IdContent { get; set; }
        public int IdOption { get; set; }
    }

    public class SimulationSessionResponseDto
    {
        public int IdSession { get; set; }
        public int IdUsuario { get; set; }
        public string FaseActual { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal PuntajeTotal { get; set; }
        public DateTime InicialoAt { get; set; }
        public DateTime? FinalizadoAt { get; set; }
    }

    public class SimulationContentDto
    {
        public int IdContent { get; set; }
        public string IdContentExterno { get; set; } = string.Empty;
        public string Fase { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Intro { get; set; }
        public string Pregunta { get; set; } = string.Empty;
        public int Orden { get; set; }
        public List<SimulationOption>? Opciones { get; set; }
    }

    public class SimulationOptionDto
    {
        public int IdOption { get; set; }
        public string IdOptionExterno { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public decimal Puntaje { get; set; }
        public string? Nivel { get; set; }
        public string? Feedback { get; set; }
        public string? Resultado { get; set; }
        public string? Insight { get; set; }
        public string? SiguientePreguntaId { get; set; }
    }

    public class FeedbackResponseDto
    {
        public int IdDecision { get; set; }
        public decimal PuntajeObtenido { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
        public decimal PuntajeTotalActualizado { get; set; }
        public bool PuedeSiguiente { get; set; }
    }

    public class SimulationStatusDto
    {
        public int IdSession { get; set; }
        public string FaseActual { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal PuntajeTotal { get; set; }
        public List<DecisionHistoryDto> DecisionesPrevias { get; set; } = new();
        public int TotalDecisiones { get; set; }
    }

    public class DecisionHistoryDto
    {
        public int IdDecision { get; set; }
        public int IdOption { get; set; }
        public decimal PuntajeObtenido { get; set; }
        public DateTime DecididoAt { get; set; }
    }
}
