using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Application.Servicos;

public class VerificacaoGrupoSolicitanteService : IVerificacaoGrupoSolicitanteService
{
    private readonly Contexto _contexto;
    
    public VerificacaoGrupoSolicitanteService(Contexto contexto)
    {
        _contexto = contexto;
    }
    
    public async Task<int> CalcularNivelHierarquia(Guid grupoPaiId)
    {
        int nivel = 1;
        var grupoAtual = await _contexto.GruposSolicitantes.FirstOrDefaultAsync(x => x.Id == grupoPaiId);

        while (grupoAtual?.GruposSolicitantePai != null)
        {
            nivel++;
            grupoAtual = await _contexto.GruposSolicitantes.FirstOrDefaultAsync(g => g.Id == grupoAtual.GrupoSolicitantePaiId);
        }

        return nivel;
    }

    public async Task<bool> ExisteCicloNaHierarquia(Guid grupoPaiId)
    {
        var grupoAtual = await _contexto.GruposSolicitantes.FindAsync(grupoPaiId);

        while (grupoAtual is not null)
        {
            if (grupoAtual.GrupoSolicitantePaiId is null)
            {
                return false; // chegou ao topo da hierarquia
            }

            if (grupoAtual.GrupoSolicitantePaiId == grupoPaiId)
            {
                return true; // Exuste ciclo
            }

            grupoAtual = await _contexto.GruposSolicitantes.FindAsync(grupoAtual.GrupoSolicitantePaiId);
        }

        return false;
    }
}