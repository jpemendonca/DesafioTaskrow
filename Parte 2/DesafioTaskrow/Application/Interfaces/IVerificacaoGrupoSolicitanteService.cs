namespace DesafioTaskrow.Application.Interfaces;

public interface IVerificacaoGrupoSolicitanteService
{
    Task VerificarNivelHierarquia(Guid grupoPaiId);
    Task VerificarExisteCicloNaHierarquia(Guid grupoPaiId);
    Task VerificarHierarquia(Guid grupoPaiId);
    Task VerificarGrupoPaiExiste(Guid grupoPaiId);
}