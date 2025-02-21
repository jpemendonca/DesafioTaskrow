using DesafioTaskrow.Domain;
using Microsoft.EntityFrameworkCore;

namespace Parte3DesafioTaskrow.Queries._1_ListagemSolicitacoes;

public class ListagemSolicitacoesQuery
{
    private readonly Contexto _context;
    
    public ListagemSolicitacoesQuery(Contexto context)
    {
        _context = context;
    }
    
    public async Task ListagemSolicitacoes(int mes, int ano)
    {
        var query = await _context.Solicitacoes
            .Where(s => s.DataCriacao >= DateTime.Now.AddMonths(-24))
            .GroupBy(s => new { s.DataCriacao.Year, s.DataCriacao.Month })
            .Select(g => new
            {
                Ano = ano,
                Mes = mes,
                NumeroSolicitacoes = g.Count(),
                GruposMaisAtivos = g.GroupBy(s => s.GrupoSolicitanteId)
                    .Select(grp => new
                    {
                        GrupoId = grp.Key,
                        TotalSolicitacoes = grp.Count()
                    })
                    .OrderByDescending(grp => grp.TotalSolicitacoes)
                    .Take(2)
                    .ToList()
            }).ToListAsync();
    }
}