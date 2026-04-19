using APISPROYECTOYATCHAY.Features.Simulation.Models;
using APISPROYECTOYATCHAY.Features.Simulation.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISPROYECTOYATCHAY.Features.Simulation.Repositories.Implementations
{
    public class SimulationContentRepository : ISimulationContentRepository
    {
        private readonly string _connectionString;

        public SimulationContentRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")!;
        }

        public async Task<SimulationContent?> ObtenerPorIdAsync(int idContent)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<SimulationContent>(
                "SELECT * FROM SimulationContent WHERE id_content = @IdContent",
                new { IdContent = idContent });
        }

        public async Task<SimulationContent?> ObtenerPorFaseAsync(int fase)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<SimulationContent>(
                "SELECT * FROM SimulationContent WHERE fase = @Fase ORDER BY orden",
                new { Fase = fase });
        }

        public async Task<List<SimulationContent>> ObtenerTodosPorFaseAsync(int fase)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultados = await conn.QueryAsync<SimulationContent>(
                "SELECT * FROM SimulationContent WHERE fase = @Fase ORDER BY orden",
                new { Fase = fase });
            return resultados.ToList();
        }

        public async Task<int> InsertarAsync(SimulationContent content)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO SimulationContent (fase, tipo, titulo, opciones, feedback, orden)
                  VALUES (@Fase, @Tipo, @Titulo, @Opciones, @Feedback, @Orden);
                  SELECT CAST(SCOPE_IDENTITY() as int);",
                new
                {
                    content.Fase,
                    content.Tipo,
                    content.Titulo,
                    content.Opciones,
                    content.Feedback,
                    content.Orden
                });
        }
    }

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
                  FROM SimulationSession WHERE id_session = @IdSession",
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
                  FROM SimulationSession WHERE id_usuario = @IdUsuario AND estado = 'EN_PROGRESO'
                  ORDER BY iniciado_at DESC",
                new { IdUsuario = idUsuario });
        }

        public async Task<int> InsertarAsync(SimulationSession session)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<int>(
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
    }

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
                  FROM Decision WHERE id_decision = @IdDecision",
                new { IdDecision = idDecision });
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
                  FROM Decision WHERE id_session = @IdSession
                  ORDER BY decidido_at",
                new { IdSession = idSession });
            return resultados.ToList();
        }

        public async Task<int> InsertarAsync(Decision decision)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<int>(
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
