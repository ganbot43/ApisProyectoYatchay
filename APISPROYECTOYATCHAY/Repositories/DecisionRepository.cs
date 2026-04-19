using APISPROYECTOYATCHAY.Models;
using APISPROYECTOYATCHAY.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISPROYECTOYATCHAY.Repositories
{
    public class DecisionRepository : IDecisionRepository
    {
        private readonly string _connectionString;

        public DecisionRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")!;
        }

        public async Task<Decision?> ObtenerPorIdAsync(int idDecision)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Decision>(
                @"SELECT 
                    id_decision AS IdDecision,
                    id_session AS IdSession,
                    id_content AS IdContent,
                    fase AS Fase,
                    opcion_elegida AS OpcionElegida,
                    puntaje_obtenido AS PuntajeObtenido,
                    decidido_at AS DecididoAt
                  FROM Decision 
                  WHERE id_decision = @IdDecision",
                new { IdDecision = idDecision });
        }

        public async Task<Decision?> ObtenerPorSesionYContenidoAsync(int idSession, int idContent)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Decision>(
                @"SELECT 
                    id_decision AS IdDecision,
                    id_session AS IdSession,
                    id_content AS IdContent,
                    fase AS Fase,
                    opcion_elegida AS OpcionElegida,
                    puntaje_obtenido AS PuntajeObtenido,
                    decidido_at AS DecididoAt
                  FROM Decision 
                  WHERE id_session = @IdSession AND id_content = @IdContent",
                new { IdSession = idSession, IdContent = idContent });
        }

        public async Task<List<Decision>> ObtenerTodosPorSesionAsync(int idSession)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultados = await conn.QueryAsync<Decision>(
                @"SELECT 
                    id_decision AS IdDecision,
                    id_session AS IdSession,
                    id_content AS IdContent,
                    fase AS Fase,
                    opcion_elegida AS OpcionElegida,
                    puntaje_obtenido AS PuntajeObtenido,
                    decidido_at AS DecididoAt
                  FROM Decision 
                  WHERE id_session = @IdSession
                  ORDER BY decidido_at",
                new { IdSession = idSession });
            return resultados.ToList();
        }

        public async Task<int> InsertarAsync(Decision decision)
        {
            using var conn = new SqlConnection(_connectionString);
            var idDecision = await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO Decision (id_session, id_content, fase, opcion_elegida, puntaje_obtenido)
                  VALUES (@IdSession, @IdContent, @Fase, @OpcionElegida, @PuntajeObtenido);
                  SELECT CAST(SCOPE_IDENTITY() as int);",
                new
                {
                    decision.IdSession,
                    decision.IdContent,
                    decision.Fase,
                    decision.OpcionElegida,
                    decision.PuntajeObtenido
                });
            return idDecision;
        }

        public async Task<bool> ExisteAsync(int idSession, int idContent)
        {
            using var conn = new SqlConnection(_connectionString);
            var existe = await conn.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM Decision WHERE id_session = @IdSession AND id_content = @IdContent",
                new { IdSession = idSession, IdContent = idContent });
            return existe > 0;
        }
    }
}
