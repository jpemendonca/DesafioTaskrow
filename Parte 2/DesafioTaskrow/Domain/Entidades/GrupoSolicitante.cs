namespace DesafioTaskrow.Domain.Entidades;

public class GrupoSolicitante : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public Guid? GrupoSolicitantePaiId { get; set; }
}