namespace APISPROYECTOYATCHAY.Features.Simulation.Models
{
    public class Decision
    {
        public int IdDecision { get; set; }
        public int IdSession { get; set; }
        public int IdContent { get; set; }
        public int IdOption { get; set; }
        public decimal PuntajeObtenido { get; set; }
        public DateTime DecididoAt { get; set; }
    }
}
