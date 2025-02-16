using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Domain.Models;

public class GrupoSolicitante : Entidades.GrupoSolicitante
{
    public virtual List<GrupoSolicitante> GrupoSolicitanteFilhos { get; set; }
    public virtual GrupoSolicitante GruposSolicitantePai { get; set; }
    public virtual List<Solicitacao> Solicitacoes { get; set; }
    public virtual List<LimiteGrupo> LimitesGrupos { get; set; }
    
    public static void Setup(ModelBuilder builder)
    {
        builder.Entity<GrupoSolicitante>().ToTable("GruposSolicitantes");
        builder.Entity<GrupoSolicitante>().HasKey(x => x.Id);
        builder.Entity<GrupoSolicitante>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<GrupoSolicitante>().HasIndex(x => x.Id);

        builder.Entity<GrupoSolicitante>()
            .HasOne(p => p.GruposSolicitantePai)
            .WithMany(p => p.GrupoSolicitanteFilhos)
            .IsRequired(false)
            .HasForeignKey(p => p.GrupoSolicitantePaiId);
    }
}