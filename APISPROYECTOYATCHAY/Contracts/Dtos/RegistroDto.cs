namespace APISPROYECTOYATCHAY.Contracts.Dtos
{
    public class RegistroDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string DNI { get; set; } = string.Empty;
        public int IdRol { get; set; } = 1;
    }
}
