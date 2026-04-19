using APISPROYECTOYATCHAY.Models;
using APISPROYECTOYATCHAY.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APISPROYECTOYATCHAY.Repositories
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

        public async Task<List<SimulationContent>> ObtenerTodosAsync()
        {
            using var conn = new SqlConnection(_connectionString);
            var resultados = await conn.QueryAsync<SimulationContent>(
                "SELECT * FROM SimulationContent ORDER BY fase, orden");
            return resultados.ToList();
        }

        public async Task<int> InsertarAsync(SimulationContent content)
        {
            using var conn = new SqlConnection(_connectionString);
            var idContent = await conn.ExecuteScalarAsync<int>(
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
            return idContent;
        }
    }
}
