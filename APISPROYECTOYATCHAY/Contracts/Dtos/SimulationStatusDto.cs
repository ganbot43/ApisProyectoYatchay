namespace APISPROYECTOYATCHAY.Contracts.Dtos
{
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
