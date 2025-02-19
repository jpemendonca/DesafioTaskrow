namespace DesafioTaskrow.Domain.Dtos;

public record LimiteGrupoDto(Guid GrupoSolicitanteId, Guid TipoSolicitacaoId, int Ano, int Mes, int LimiteMensal);