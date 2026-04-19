using System.ComponentModel.DataAnnotations;

namespace APISPROYECTOYATCHAY.Contracts.Dtos
{
    public class DecisionRequestDto
    {
        [Required(ErrorMessage = "El ID de la sesión es requerido")]
        public int IdSession { get; set; }

        [Required(ErrorMessage = "El ID del contenido es requerido")]
        public int IdContent { get; set; }

        [Required(ErrorMessage = "La opción elegida es requerida")]
        public int OpcionElegida { get; set; }
    }
}
