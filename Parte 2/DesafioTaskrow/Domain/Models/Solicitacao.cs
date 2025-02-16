using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Domain.Models;

public class Solicitacao : Entidades.Solicitacao
{
    public virtual GrupoSolicitante GrupoSolicitante { get; set; }
    public virtual TipoSolicitacao TipoSolicitacao { get; set; }
    
    public static void Setup(ModelBuilder builder)
    {
        builder.Entity<Solicitacao>().ToTable("Solicitacoes");
        builder.Entity<Solicitacao>().HasKey(x => x.Id);
        builder.Entity<Solicitacao>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<Solicitacao>().HasIndex(x => x.Id);

        builder.Entity<Solicitacao>()
            .HasOne(p => p.GrupoSolicitante)
            .WithMany(p => p.Solicitacoes)
            .IsRequired(true)
            .HasForeignKey(p => p.GrupoSolicitanteId);
        
        builder.Entity<Solicitacao>()
            .HasOne(p => p.TipoSolicitacao)
            .WithMany(p => p.Solicitacoes)
            .IsRequired(true)
            .HasForeignKey(p => p.TipoSolicitacaoId);
    }
}