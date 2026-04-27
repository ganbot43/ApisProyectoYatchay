namespace APISPROYECTOYATCHAY.Features.Simulation.Models
{
    public class SimulationOption
    {
        public int IdOption { get; set; }
        public string IdOptionExterno { get; set; } = string.Empty;
        public int IdContent { get; set; }
        public string Texto { get; set; } = string.Empty;
        public decimal Puntaje { get; set; }
        public string? Nivel { get; set; }
        public string? Feedback { get; set; }
        public string? Resultado { get; set; }
        public string? Insight { get; set; }
        public string? SiguientePreguntaId { get; set; }
        public int Orden { get; set; }
        public DateTime CreadoAt { get; set; }
        public DateTime? ActualizadoAt { get; set; }
    }
}
