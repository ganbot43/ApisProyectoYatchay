namespace APISPROYECTOYATCHAY.Features.Simulation.Models
{
    public class Decision
    {
        public int IdDecision { get; set; }
        public int IdSession { get; set; }
        public int IdContent { get; set; }
        public int Fase { get; set; }
        public int OpcionElegida { get; set; }
        public int PuntajeObtenido { get; set; }
        public DateTime DecididoAt { get; set; }
    }
}
