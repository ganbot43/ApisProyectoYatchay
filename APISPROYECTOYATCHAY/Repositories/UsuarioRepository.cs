using APISPROYECTOYATCHAY.Contracts.Dtos;
using APISPROYECTOYATCHAY.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace APISPROYECTOYATCHAY.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")!;
        }

        public async Task<int> RegistrarAsync(RegistroDto registro, string contrasenaHash)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<int>(
                "sp_Usuario_Insertar",
                new
                {
                    registro.Nombre,
                    registro.Apellido,
                    registro.Correo,
                    ContrasenaHash = contrasenaHash,
                    ContrasenalLiteral = registro.Contrasena,
                    registro.DNI,
                    registro.IdRol
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<LoginResponseDto?> LoginAsync(string correo, string contrasenaHash)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<LoginResponseDto>(
                "sp_Usuario_Login",
                new { Correo = correo, ContrasenaHash = contrasenaHash },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> CorreoExisteAsync(string correo)
        {
            using var conn = new SqlConnection(_connectionString);
            var existe = await conn.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM Usuario WHERE correo = @Correo",
                new { Correo = correo });
            return existe > 0;
        }

        public async Task<bool> ContraseñaEsValidaAsync(string contrasena)
        {
            if (contrasena.Length < 6)
                return false;

            if (!Regex.IsMatch(contrasena, @"[A-Z]"))
                return false;

            if (!Regex.IsMatch(contrasena, @"[0-9]"))
                return false;

            if (!Regex.IsMatch(contrasena, @"[#$%&@]"))
                return false;

            return await Task.FromResult(true);
        }

        public async Task<bool> DNIEsValidoAsync(string dni)
        {
            if (!Regex.IsMatch(dni, @"^\d{8}$"))
                return false;

            using var conn = new SqlConnection(_connectionString);
            var existe = await conn.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM Usuario WHERE dni = @DNI",
                new { DNI = dni });
            
            if (existe > 0)
                return false;

            return await Task.FromResult(true);
        }
    }
}
