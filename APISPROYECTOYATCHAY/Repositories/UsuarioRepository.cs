using APISPROYECTOYATCHAY.Contracts.Dtos;
using APISPROYECTOYATCHAY.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISPROYECTOYATCHAY.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")!;
        }

        // Registrar usuario
        public async Task<int> RegistrarAsync(RegistroDto registro, string contrasenaHash)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_Usuario_Insertar", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Nombre", registro.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", registro.Apellido);
            cmd.Parameters.AddWithValue("@Correo", registro.Correo);
            cmd.Parameters.AddWithValue("@ContrasenaHash", contrasenaHash);
            cmd.Parameters.AddWithValue("@ContrasenalLiteral", registro.Contrasena);
            cmd.Parameters.AddWithValue("@DNI", registro.DNI);
            cmd.Parameters.AddWithValue("@IdRol", registro.IdRol);

            await cmd.ExecuteNonQueryAsync();
            return 1;
        }

        // Login usuario
        public async Task<LoginResponseDto?> LoginAsync(string correo, string contrasenaHash)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new SqlCommand("sp_Usuario_Login", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Correo", correo);
            cmd.Parameters.AddWithValue("@ContrasenaHash", contrasenaHash);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new LoginResponseDto
                {
                    IdUsuario = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    IdRol = reader.GetInt32(2),
                    NombreRol = reader.GetString(3)
                };
            }

            return null;
        }
    }
}
