using APISPROYECTOYATCHAY.Features.Authentication.Repositories;
using APISPROYECTOYATCHAY.Features.Simulation.Repositories;
using APISPROYECTOYATCHAY.Features.Simulation.Repositories.Implementations;
using APISPROYECTOYATCHAY.Features.Simulation.Services;
using APISPROYECTOYATCHAY.Common.Middleware;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Authentication Dependencies
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Register Simulation Dependencies
builder.Services.AddScoped<ISimulationContentRepository, SimulationContentRepository>();
builder.Services.AddScoped<ISimulationOptionRepository, SimulationOptionRepository>();
builder.Services.AddScoped<ISimulationSessionRepository, SimulationSessionRepository>();
builder.Services.AddScoped<IDecisionRepository, DecisionRepository>();
builder.Services.AddScoped<ISimulationService, SimulationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Middleware global para excepciones
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
