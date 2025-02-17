namespace DesafioTaskrow.Application.Interfaces;

public interface IVerificacaoGrupoSolicitanteService
{
    Task<int> CalcularNivelHierarquia(Guid grupoPaiId);
    Task<bool> ExisteCicloNaHierarquia(Guid grupoPaiId);
}