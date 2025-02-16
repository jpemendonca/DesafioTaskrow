using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Domain.Models;

public class TipoSolicitacao : Entidades.TipoSolicitacao
{
    public virtual List<Solicitacao> Solicitacoes { get; set; }
    public virtual List<LimiteGrupo> LimitesGrupos { get; set; }
    public static void Setup(ModelBuilder builder)
    {
        builder.Entity<TipoSolicitacao>().ToTable("TiposSolicitacoes");
        builder.Entity<TipoSolicitacao>().HasKey(x => x.Id);
        builder.Entity<TipoSolicitacao>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<TipoSolicitacao>().HasIndex(x => x.Id);
    }
}