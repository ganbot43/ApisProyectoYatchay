using System.ComponentModel.DataAnnotations;

namespace APISPROYECTOYATCHAY.Contracts.Dtos
{
    public class SimulationStartRequestDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int IdUsuario { get; set; }
    }
}
