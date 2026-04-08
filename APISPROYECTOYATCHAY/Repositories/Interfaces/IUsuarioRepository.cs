using APISPROYECTOYATCHAY.Contracts.Dtos;

namespace APISPROYECTOYATCHAY.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<int> RegistrarAsync(RegistroDto registro, string contrasenaHash);
        Task<LoginResponseDto?> LoginAsync(string correo, string contrasenaHash);
        Task<bool> CorreoExisteAsync(string correo);
        Task<bool> ContraseñaEsValidaAsync(string contrasena);
        Task<bool> DNIEsValidoAsync(string dni);
    }
}
