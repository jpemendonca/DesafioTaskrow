using DesafioTaskrow.Domain;
using DesafioTaskrow.Application.Servicos;
using DesafioTaskrow.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add-Migration MigrationX -Context Contexto       dotnet ef migrations add MigracaoX
// Update-Database -Context Contexto                  dotnet ef database update

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("Connection");

builder.Services.AddDbContext<Contexto>(options => { options.UseSqlServer(connectionString); });

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IVerificacaoGrupoSolicitanteService, VerificacaoGrupoSolicitanteService>();
builder.Services.AddScoped<IVerificacaoSolicitacaoService, VerificacaoSolicitacaoService>();

builder.Services.AddScoped<IGrupoSolicitanteService, GrupoSolicitanteService>();
builder.Services.AddScoped<ITipoSolicitacaoService, TipoSolicitacaoService>();
builder.Services.AddScoped<ISolicitacaoService, SolicitacaoService>();
builder.Services.AddScoped<ILimiteService, LimiteService>();



var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference("/");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
