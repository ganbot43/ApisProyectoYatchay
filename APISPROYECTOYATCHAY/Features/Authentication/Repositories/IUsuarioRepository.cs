using APISPROYECTOYATCHAY.Features.Authentication.Dtos;

namespace APISPROYECTOYATCHAY.Features.Authentication.Repositories
{
    public interface IUsuarioRepository
    {
        Task<int> RegistrarAsync(RegisterRequestDto registro, string contrasenaHash);
        Task<LoginResponseDto?> LoginAsync(string correo, string contrasenaHash);
        Task<bool> CorreoExisteAsync(string correo);
        Task<bool> ContraseñaEsValidaAsync(string contrasena);
        Task<bool> DNIEsValidoAsync(string dni);
    }
}
