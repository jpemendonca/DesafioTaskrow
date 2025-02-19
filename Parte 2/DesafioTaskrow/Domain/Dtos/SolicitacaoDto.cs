using DesafioTaskrow.Domain.Enums;

namespace DesafioTaskrow.Domain.Dtos;

public record SolicitacaoDto(
    Guid GrupoSolicitanteId,
    Guid TipoSolicitacaoId,
    DateTime DataConclusao,
    EnumStatus Status,
    string Descricao);