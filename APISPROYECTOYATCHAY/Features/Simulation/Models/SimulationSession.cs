namespace APISPROYECTOYATCHAY.Features.Simulation.Models
{
    public class SimulationSession
    {
        public int IdSession { get; set; }
        public int IdUsuario { get; set; }
        public string FaseActual { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal PuntajeTotal { get; set; }
        public DateTime InicialoAt { get; set; }
        public DateTime? FinalizadoAt { get; set; }
    }
}
