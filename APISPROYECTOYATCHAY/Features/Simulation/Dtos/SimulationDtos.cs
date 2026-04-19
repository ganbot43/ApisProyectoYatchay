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
        public int OpcionElegida { get; set; }
    }

    public class SimulationSessionResponseDto
    {
        public int IdSession { get; set; }
        public int IdUsuario { get; set; }
        public int FaseActual { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int PuntajeTotal { get; set; }
        public DateTime InicialoAt { get; set; }
        public DateTime? FinalizadoAt { get; set; }
    }

    public class SimulationContentDto
    {
        public int IdContent { get; set; }
        public int Fase { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Opciones { get; set; } = string.Empty;
        public string Feedback { get; set; } = string.Empty;
        public int Orden { get; set; }
    }

    public class FeedbackResponseDto
    {
        public int IdDecision { get; set; }
        public int PuntajeObtenido { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty;
        public int PuntajeTotalActualizado { get; set; }
        public bool PuedeSiguiente { get; set; }
    }

    public class SimulationStatusDto
    {
        public int IdSession { get; set; }
        public int FaseActual { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int PuntajeTotal { get; set; }
        public List<DecisionHistoryDto> DecisionesPrevias { get; set; } = new();
        public int TotalDecisiones { get; set; }
    }

    public class DecisionHistoryDto
    {
        public int IdDecision { get; set; }
        public int Fase { get; set; }
        public int OpcionElegida { get; set; }
        public int PuntajeObtenido { get; set; }
        public DateTime DecididoAt { get; set; }
    }
}
