using APISPROYECTOYATCHAY.Contracts.Dtos;
using APISPROYECTOYATCHAY.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Cryptography;
using System.Text;

namespace APISPROYECTOYATCHAY.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _repo;

        public UsuariosController(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        // Hashear contraseña
        private string HashearContrasena(string contrasena)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Crear respuesta de validación
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

        // POST: api/usuarios/registro
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDto registro)
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

        // POST: api/usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
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
