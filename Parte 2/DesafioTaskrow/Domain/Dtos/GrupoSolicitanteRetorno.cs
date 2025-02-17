namespace DesafioTaskrow.Domain.Dtos;

public class GrupoSolicitanteRetorno
{
    public string Nome { get; set; }
    public Guid Id { get; set; }
    public Guid? GrupoPaiId { get; set; }
}