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
                @"SELECT 
                    id_content AS IdContent,
                    id_content_externo AS IdContentExterno,
                    fase AS Fase,
                    categoria AS Categoria,
                    tipo AS Tipo,
                    titulo AS Titulo,
                    intro AS Intro,
                    pregunta AS Pregunta,
                    requerida AS Requerida,
                    shuffle_opciones AS ShuffleOpciones,
                    orden AS Orden,
                    creado_at AS CreadoAt,
                    actualizado_at AS ActualizadoAt
                  FROM SimulationContent WHERE id_content = @IdContent",
                new { IdContent = idContent });
        }

        public async Task<SimulationContent?> ObtenerPorIdExternoAsync(string idContentExterno)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<SimulationContent>(
                @"SELECT 
                    id_content AS IdContent,
                    id_content_externo AS IdContentExterno,
                    fase AS Fase,
                    categoria AS Categoria,
                    tipo AS Tipo,
                    titulo AS Titulo,
                    intro AS Intro,
                    pregunta AS Pregunta,
                    requerida AS Requerida,
                    shuffle_opciones AS ShuffleOpciones,
                    orden AS Orden,
                    creado_at AS CreadoAt,
                    actualizado_at AS ActualizadoAt
                  FROM SimulationContent WHERE id_content_externo = @IdContentExterno",
                new { IdContentExterno = idContentExterno });
        }

        public async Task<SimulationContent?> ObtenerPorFaseAsync(string fase)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<SimulationContent>(
                @"SELECT 
                    id_content AS IdContent,
                    id_content_externo AS IdContentExterno,
                    fase AS Fase,
                    categoria AS Categoria,
                    tipo AS Tipo,
                    titulo AS Titulo,
                    intro AS Intro,
                    pregunta AS Pregunta,
                    requerida AS Requerida,
                    shuffle_opciones AS ShuffleOpciones,
                    orden AS Orden,
                    creado_at AS CreadoAt,
                    actualizado_at AS ActualizadoAt
                  FROM SimulationContent WHERE fase = @Fase ORDER BY orden",
                new { Fase = fase });
        }

        public async Task<List<SimulationContent>> ObtenerTodosPorFaseAsync(string fase)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultados = await conn.QueryAsync<SimulationContent>(
                @"SELECT 
                    id_content AS IdContent,
                    id_content_externo AS IdContentExterno,
                    fase AS Fase,
                    categoria AS Categoria,
                    tipo AS Tipo,
                    titulo AS Titulo,
                    intro AS Intro,
                    pregunta AS Pregunta,
                    requerida AS Requerida,
                    shuffle_opciones AS ShuffleOpciones,
                    orden AS Orden,
                    creado_at AS CreadoAt,
                    actualizado_at AS ActualizadoAt
                  FROM SimulationContent WHERE fase = @Fase ORDER BY orden",
                new { Fase = fase });
            return resultados.ToList();
        }

        public async Task<int> InsertarAsync(SimulationContent content)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO SimulationContent (id_content_externo, fase, categoria, tipo, titulo, intro, pregunta, requerida, shuffle_opciones, orden)
                  VALUES (@IdContentExterno, @Fase, @Categoria, @Tipo, @Titulo, @Intro, @Pregunta, @Requerida, @ShuffleOpciones, @Orden);
                  SELECT CAST(SCOPE_IDENTITY() as int);",
                new
                {
                    content.IdContentExterno,
                    content.Fase,
                    content.Categoria,
                    content.Tipo,
                    content.Titulo,
                    content.Intro,
                    content.Pregunta,
                    content.Requerida,
                    content.ShuffleOpciones,
                    content.Orden
                });
        }
    }

    public class SimulationOptionRepository : ISimulationOptionRepository
    {
        private readonly string _connectionString;

        public SimulationOptionRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default")!;
        }

        public async Task<SimulationOption?> ObtenerPorIdAsync(int idOption)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<SimulationOption>(
                @"SELECT 
                    id_option AS IdOption,
                    id_option_externo AS IdOptionExterno,
                    id_content AS IdContent,
                    texto AS Texto,
                    puntaje AS Puntaje,
                    nivel AS Nivel,
                    feedback AS Feedback,
                    resultado AS Resultado,
                    insight AS Insight,
                    siguiente_pregunta_id AS SiguientePreguntaId,
                    orden AS Orden,
                    creado_at AS CreadoAt,
                    actualizado_at AS ActualizadoAt
                  FROM SimulationOption WHERE id_option = @IdOption",
                new { IdOption = idOption });
        }

        public async Task<List<SimulationOption>> ObtenerTodasPorContenidoAsync(int idContent)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultados = await conn.QueryAsync<SimulationOption>(
                @"SELECT 
                    id_option AS IdOption,
                    id_option_externo AS IdOptionExterno,
                    id_content AS IdContent,
                    texto AS Texto,
                    puntaje AS Puntaje,
                    nivel AS Nivel,
                    feedback AS Feedback,
                    resultado AS Resultado,
                    insight AS Insight,
                    siguiente_pregunta_id AS SiguientePreguntaId,
                    orden AS Orden,
                    creado_at AS CreadoAt,
                    actualizado_at AS ActualizadoAt
                  FROM SimulationOption WHERE id_content = @IdContent ORDER BY orden",
                new { IdContent = idContent });
            return resultados.ToList();
        }

        public async Task<SimulationOption?> ObtenerPorIdExternoAsync(string idOptionExterno)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<SimulationOption>(
                @"SELECT 
                    id_option AS IdOption,
                    id_option_externo AS IdOptionExterno,
                    id_content AS IdContent,
                    texto AS Texto,
                    puntaje AS Puntaje,
                    nivel AS Nivel,
                    feedback AS Feedback,
                    resultado AS Resultado,
                    insight AS Insight,
                    siguiente_pregunta_id AS SiguientePreguntaId,
                    orden AS Orden,
                    creado_at AS CreadoAt,
                    actualizado_at AS ActualizadoAt
                  FROM SimulationOption WHERE id_option_externo = @IdOptionExterno",
                new { IdOptionExterno = idOptionExterno });
        }

        public async Task<int> InsertarAsync(SimulationOption option)
        {
            using var conn = new SqlConnection(_connectionString);
            return await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO SimulationOption (id_option_externo, id_content, texto, puntaje, nivel, feedback, resultado, insight, siguiente_pregunta_id, orden)
                  VALUES (@IdOptionExterno, @IdContent, @Texto, @Puntaje, @Nivel, @Feedback, @Resultado, @Insight, @SiguientePreguntaId, @Orden);
                  SELECT CAST(SCOPE_IDENTITY() as int);",
                new
                {
                    option.IdOptionExterno,
                    option.IdContent,
                    option.Texto,
                    option.Puntaje,
                    option.Nivel,
                    option.Feedback,
                    option.Resultado,
                    option.Insight,
                    option.SiguientePreguntaId,
                    option.Orden
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

        public async Task<bool> ActualizarFaseAsync(int idSession, string faseNueva)
        {
            using var conn = new SqlConnection(_connectionString);
            var resultado = await conn.ExecuteAsync(
                "UPDATE SimulationSession SET fase_actual = @FaseNueva WHERE id_session = @IdSession",
                new { FaseNueva = faseNueva, IdSession = idSession });
            return resultado > 0;
        }

        public async Task<bool> ActualizarPuntajeAsync(int idSession, decimal puntajeAñadido)
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
                    id_option AS IdOption,
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
                    id_option AS IdOption,
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
                @"INSERT INTO Decision (id_session, id_content, id_option, puntaje_obtenido)
                  VALUES (@IdSession, @IdContent, @IdOption, @PuntajeObtenido);
                  SELECT CAST(SCOPE_IDENTITY() as int);",
                new
                {
                    decision.IdSession,
                    decision.IdContent,
                    decision.IdOption,
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
