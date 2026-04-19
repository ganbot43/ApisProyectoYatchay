using System.ComponentModel.DataAnnotations;

namespace APISPROYECTOYATCHAY.Features.Authentication.Dtos
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [RegularExpression(@"^i\d{9}@cibertec\.edu\.pe$", ErrorMessage = "El correo debe ser formato: i#########@cibertec.edu.pe")]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string? Contrasena { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener exactamente 8 dígitos")]
        public string? DNI { get; set; }

        public int IdRol { get; set; } = 1; // estudiante = 1
    }
}
