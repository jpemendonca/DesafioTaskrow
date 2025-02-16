using DesafioTaskrow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Domain;

public class Contexto(DbContextOptions<Contexto> options) : DbContext(options)
{
    public DbSet<Log> Logs { get; set; }
    public DbSet<GrupoSolicitante> GruposSolicitantes { get; set; }
    public DbSet<TipoSolicitacao> TiposSolicitacoes { get; set; }
    public DbSet<Solicitacao> Solicitacoes { get; set; }
    public DbSet<LimiteGrupo> LimitesGrupos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        Models.GrupoSolicitante.Setup(builder);
        Models.TipoSolicitacao.Setup(builder);
        Models.Solicitacao.Setup(builder);
        Models.LimiteGrupo.Setup(builder);
        Models.Log.Setup(builder);
    }
}