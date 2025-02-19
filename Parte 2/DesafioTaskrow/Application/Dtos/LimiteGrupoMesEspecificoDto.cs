namespace DesafioTaskrow.Application.Dtos;

public record LimiteGrupoMesEspecificoDto(Guid GrupoSolicitanteId, Guid TipoSolicitacaoId, int Ano, int Mes);