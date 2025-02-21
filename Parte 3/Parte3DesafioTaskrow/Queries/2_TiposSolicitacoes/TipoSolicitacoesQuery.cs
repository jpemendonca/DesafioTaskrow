using DesafioTaskrow.Domain;
using Microsoft.EntityFrameworkCore;

namespace Parte3DesafioTaskrow.Queries._2_TiposSolicitacoes;

public class TipoSolicitacoesQuery
{
    private readonly Contexto _context;

    public TipoSolicitacoesQuery(Contexto context)
    {
        _context = context;
    }

    public async Task ObterTipoSolicitacoesPorMes(int mes, int ano)
    {
        await _context.Solicitacoes
            .Where(x => x.DataCriacao.Month == mes && x.DataCriacao.Year == ano)
            .GroupBy(x => x.TipoSolicitacaoId)
            .Select(g => new
            {
                TipoSolicitacaoId = g.Key,
                TotalSolicitacoes = g.Count(),
                GrupoMaisFrequente = g.GroupBy(s => s.GrupoSolicitanteId)
                    .OrderByDescending(grp => grp.Count())
                    .Select(grp => new
                    {
                        GrupoId = grp.Key,
                        TotalGrupo = grp.Count()
                    })
                    .FirstOrDefault()
            })
            .OrderByDescending(g => g.TotalSolicitacoes)
            .Take(5).ToListAsync();
    }
}