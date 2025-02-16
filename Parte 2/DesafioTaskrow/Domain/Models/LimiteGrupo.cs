using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Domain.Models;

public class LimiteGrupo : Entidades.LimiteGrupo
{
    public virtual GrupoSolicitante GrupoSolicitante { get; set; }
    public virtual TipoSolicitacao TipoSolicitacao { get; set; }
    
    public static void Setup(ModelBuilder builder)
    {
        builder.Entity<LimiteGrupo>().ToTable("LimitesGrupos");
        builder.Entity<LimiteGrupo>().HasKey(x => x.Id);
        builder.Entity<LimiteGrupo>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<LimiteGrupo>().HasIndex(x => x.Id);

        builder.Entity<LimiteGrupo>()
            .HasOne(p => p.GrupoSolicitante)
            .WithMany(p => p.LimitesGrupos)
            .IsRequired(true)
            .HasForeignKey(p => p.GrupoSolicitanteId);
        
        builder.Entity<LimiteGrupo>()
            .HasOne(p => p.TipoSolicitacao)
            .WithMany(p => p.LimitesGrupos)
            .IsRequired(true)
            .HasForeignKey(p => p.TipoSolicitacaoId);
    }
}