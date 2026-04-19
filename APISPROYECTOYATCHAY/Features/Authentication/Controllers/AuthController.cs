using APISPROYECTOYATCHAY.Features.Authentication.Dtos;
using APISPROYECTOYATCHAY.Features.Authentication.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Cryptography;
using System.Text;

namespace APISPROYECTOYATCHAY.Features.Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _repo;

        public AuthController(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        private string HashearContrasena(string contrasena)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private IActionResult RespuestaValidacionFallida(ModelStateDictionary modelState)
        {
            var errores = modelState.Values
                .SelectMany(v => v.Errors)
                .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(new
            {
                exito = 0,
                mensaje = "Validación fallida",
                errores = errores.Any() ? errores : new List<string> { "Error de validación" }
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registro([FromBody] RegisterRequestDto registro)
        {
            if (registro == null)
                return BadRequest(new { exito = 0, mensaje = "El cuerpo de la solicitud es requerido" });

            if (!ModelState.IsValid)
                return RespuestaValidacionFallida(ModelState);

            try
            {
                var contraseñaValida = await _repo.ContraseñaEsValidaAsync(registro.Contrasena);
                if (!contraseñaValida)
                    return BadRequest(new { exito = 0, mensaje = "La contraseña debe tener al menos 6 caracteres, incluir una mayúscula, un número y un carácter especial (#$%&@)" });

                var correoExiste = await _repo.CorreoExisteAsync(registro.Correo);
                if (correoExiste)
                    return BadRequest(new { exito = 0, mensaje = $"El correo '{registro.Correo}' ya está registrado en el sistema" });

                var dniValido = await _repo.DNIEsValidoAsync(registro.DNI);
                if (!dniValido)
                    return BadRequest(new { exito = 0, mensaje = "El DNI debe tener exactamente 8 dígitos y no estar registrado" });

                var contrasenaHash = HashearContrasena(registro.Contrasena);
                var resultado = await _repo.RegistrarAsync(registro, contrasenaHash);

                if (resultado <= 0)
                    return BadRequest(new { exito = 0, mensaje = "Error al registrar el usuario" });

                return Ok(new { exito = 1, mensaje = "Usuario registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = 0, mensaje = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto login)
        {
            if (login == null)
                return BadRequest(new { exito = 0, mensaje = "El cuerpo de la solicitud es requerido" });

            if (!ModelState.IsValid)
                return RespuestaValidacionFallida(ModelState);

            try
            {
                var contrasenaHash = HashearContrasena(login.Contrasena);

                var usuario = await _repo.LoginAsync(login.Correo, contrasenaHash);

                if (usuario == null)
                    return Unauthorized(new { exito = 0, mensaje = "Credenciales inválidas" });

                return Ok(new { exito = 1, mensaje = "Login exitoso", datos = usuario });
            }
            catch (Exception ex)
            {
                return BadRequest(new { exito = 0, mensaje = ex.Message });
            }
        }
    }
}
