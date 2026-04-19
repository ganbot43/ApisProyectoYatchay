using System.ComponentModel.DataAnnotations;

namespace APISPROYECTOYATCHAY.Features.Authentication.Dtos
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [RegularExpression(@"^i\d{9}@cibertec\.edu\.pe$", ErrorMessage = "El correo debe ser formato: i#########@cibertec.edu.pe")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Contrasena { get; set; }
    }
}
