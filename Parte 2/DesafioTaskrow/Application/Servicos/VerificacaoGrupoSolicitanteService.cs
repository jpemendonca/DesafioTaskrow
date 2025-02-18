using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain;
using DesafioTaskrow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Application.Servicos;

public class VerificacaoGrupoSolicitanteService : IVerificacaoGrupoSolicitanteService
{
    private readonly Contexto _contexto;

    public VerificacaoGrupoSolicitanteService(Contexto contexto)
    {
        _contexto = contexto;
    }

    public async Task VerificarNivelHierarquia(Guid grupoPaiId)
    {
        int nivel = 1;
        var grupoAtual = await _contexto.GruposSolicitantes.FirstOrDefaultAsync(x => x.Id == grupoPaiId);

        while (grupoAtual?.GruposSolicitantePai != null)
        {
            nivel++;
            grupoAtual =
                await _contexto.GruposSolicitantes.FirstOrDefaultAsync(g => g.Id == grupoAtual.GrupoSolicitantePaiId);
        }

        if (nivel >= 5)
        {
            throw new HierarquiaMaximaException("A hierarquia não pode ter mais de 5 níveis.");
        }
    }

    public async Task VerificarExisteCicloNaHierarquia(Guid grupoPaiId)
    {
        var grupoAtual = await _contexto.GruposSolicitantes.FindAsync(grupoPaiId);

        while (grupoAtual is not null)
        {
            if (grupoAtual.GrupoSolicitantePaiId is null)
            {
                break;
            }

            if (grupoAtual.GrupoSolicitantePaiId == grupoPaiId) // Existe ciclo
            {
                throw new HierarquiaCiclicaException("A hierarquia de grupos não pode conter ciclos.");
            }

            grupoAtual = await _contexto.GruposSolicitantes.FindAsync(grupoAtual.GrupoSolicitantePaiId);
        }
    }

    public async Task VerificarHierarquia(Guid grupoPaiId)
    {
        await VerificarNivelHierarquia(grupoPaiId);
        await VerificarExisteCicloNaHierarquia(grupoPaiId);
    }

    public async Task VerificarGrupoPaiExiste(Guid grupoPaiId)
    {
        var grupoPai = await _contexto.GruposSolicitantes.FindAsync(grupoPaiId);

        if (grupoPai is null)
        {
            throw new GrupoPaiNaoEncontradoException("O grupo pai informado não existe.");
        }
    }
}