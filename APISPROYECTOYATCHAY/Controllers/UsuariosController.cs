using APISPROYECTOYATCHAY.Contracts.Dtos;
using APISPROYECTOYATCHAY.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        // POST: api/usuarios/registro
        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroDto registro)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var contrasenaHash = HashearContrasena(registro.Contrasena);
                
                // Pasar ambas: hash y literal
                await _repo.RegistrarAsync(registro, contrasenaHash);

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
