namespace APISPROYECTOYATCHAY.Models
{
    public class SimulationContent
    {
        public int IdContent { get; set; }
        public int Fase { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Opciones { get; set; } = string.Empty; // JSON
        public string Feedback { get; set; } = string.Empty; // JSON
        public int Orden { get; set; }
        public DateTime CreadoAt { get; set; }
        public DateTime? ActualizadoAt { get; set; }
    }
}
