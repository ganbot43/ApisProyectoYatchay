namespace APISPROYECTOYATCHAY.Models
{
    public class SimulationSession
    {
        public int IdSession { get; set; }
        public int IdUsuario { get; set; }
        public int FaseActual { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int PuntajeTotal { get; set; }
        public DateTime InicialoAt { get; set; }
        public DateTime? FinalizadoAt { get; set; }
    }
}
