using APISPROYECTOYATCHAY.Repositories;
using APISPROYECTOYATCHAY.Repositories.Interfaces;
using APISPROYECTOYATCHAY.Services;
using APISPROYECTOYATCHAY.Services.Interfaces;
using APISPROYECTOYATCHAY.Exceptions;
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

// Registrar Repositories - Usuario
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Registrar Repositories - Simulación
builder.Services.AddScoped<ISimulationContentRepository, SimulationContentRepository>();
builder.Services.AddScoped<ISimulationSessionRepository, SimulationSessionRepository>();
builder.Services.AddScoped<IDecisionRepository, DecisionRepository>();

// Registrar Services
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

// Middleware para excepciones globales
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
