using APISPROYECTOYATCHAY.Models;
using APISPROYECTOYATCHAY.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISPROYECTOYATCHAY.Repositories
{
    public class SimulationSessionRepository : ISimulationSessionRepository
    {
        private readonly string _connectionString;

        public SimulationSessionRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")!;
        }

        public async Task<SimulationSession?> ObtenerPorIdAsync(int idSession)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<SimulationSession>(
                @"SELECT 
                    id_session AS IdSession,
                    id_usuario AS IdUsuario,
                    fase_actual AS FaseActual,
                    estado AS Estado,
                    puntaje_total AS PuntajeTotal,
                    iniciado_at AS InicialoAt,
                    finalizado_at AS FinalizadoAt
                  FROM SimulationSession 
                  WHERE id_session = @IdSession",
                new { IdSession = idSession });
        }

        public async Task<SimulationSession?> ObtenerActivaPorUsuarioAsync(int idUsuario)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<SimulationSession>(
                @"SELECT 
                    id_session AS IdSession,
                    id_usuario AS IdUsuario,
                    fase_actual AS FaseActual,
                    estado AS Estado,
                    puntaje_total AS PuntajeTotal,
                    iniciado_at AS InicialoAt,
                    finalizado_at AS FinalizadoAt
                  FROM SimulationSession 
                  WHERE id_usuario = @IdUsuario AND estado = 'EN_PROGRESO'
                  ORDER BY iniciado_at DESC",
                new { IdUsuario = idUsuario });
        }

        public async Task<List<SimulationSession>> ObtenerTodosPorUsuarioAsync(int idUsuario)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultados = await conn.QueryAsync<SimulationSession>(
                @"SELECT 
                    id_session AS IdSession,
                    id_usuario AS IdUsuario,
                    fase_actual AS FaseActual,
                    estado AS Estado,
                    puntaje_total AS PuntajeTotal,
                    iniciado_at AS InicialoAt,
                    finalizado_at AS FinalizadoAt
                  FROM SimulationSession 
                  WHERE id_usuario = @IdUsuario
                  ORDER BY iniciado_at DESC",
                new { IdUsuario = idUsuario });
            return resultados.ToList();
        }

        public async Task<int> InsertarAsync(SimulationSession session)
        {
            using var conn = new SqlConnection(_connectionString);
            var idSession = await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO SimulationSession (id_usuario, fase_actual, estado, puntaje_total)
                  VALUES (@IdUsuario, @FaseActual, @Estado, @PuntajeTotal);
                  SELECT CAST(SCOPE_IDENTITY() as int);",
                new
                {
                    session.IdUsuario,
                    session.FaseActual,
                    session.Estado,
                    session.PuntajeTotal
                });
            return idSession;
        }

        public async Task<bool> ActualizarFaseAsync(int idSession, int faseNueva)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultado = await conn.ExecuteAsync(
                "UPDATE SimulationSession SET fase_actual = @FaseNueva WHERE id_session = @IdSession",
                new { FaseNueva = faseNueva, IdSession = idSession });
            return resultado > 0;
        }

        public async Task<bool> ActualizarPuntajeAsync(int idSession, int puntajeAñadido)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultado = await conn.ExecuteAsync(
                "UPDATE SimulationSession SET puntaje_total = puntaje_total + @PuntajeAñadido WHERE id_session = @IdSession",
                new { PuntajeAñadido = puntajeAñadido, IdSession = idSession });
            return resultado > 0;
        }

        public async Task<bool> FinalizarSesionAsync(int idSession)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultado = await conn.ExecuteAsync(
                "UPDATE SimulationSession SET estado = 'COMPLETADO', finalizado_at = GETDATE() WHERE id_session = @IdSession",
                new { IdSession = idSession });
            return resultado > 0;
        }
    }
}
