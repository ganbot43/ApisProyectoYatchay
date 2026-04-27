namespace APISPROYECTOYATCHAY.Features.Simulation.Models
{
    public class SimulationContent
    {
        public int IdContent { get; set; }
        public string IdContentExterno { get; set; } = string.Empty;
        public string Fase { get; set; } = string.Empty;
        public string? Categoria { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Intro { get; set; }
        public string Pregunta { get; set; } = string.Empty;
        public bool Requerida { get; set; }
        public bool ShuffleOpciones { get; set; }
        public int Orden { get; set; }
        public DateTime CreadoAt { get; set; }
        public DateTime? ActualizadoAt { get; set; }
    }
}
