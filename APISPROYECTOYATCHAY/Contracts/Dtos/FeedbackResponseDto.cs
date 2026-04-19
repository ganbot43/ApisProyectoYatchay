namespace APISPROYECTOYATCHAY.Contracts.Dtos
{
    public class FeedbackResponseDto
    {
        public int IdDecision { get; set; }
        public int PuntajeObtenido { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string Resultado { get; set; } = string.Empty; // éxito, parcial, error
        public int PuntajeTotalActualizado { get; set; }
        public bool PuedeSiguiente { get; set; }
    }
}
