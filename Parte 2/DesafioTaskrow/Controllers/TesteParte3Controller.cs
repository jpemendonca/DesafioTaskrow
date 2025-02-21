using DesafioTaskrow.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteParte3Controller : ControllerBase
    {
        private readonly Contexto _context;

        public TesteParte3Controller(Contexto context)
        {
            _context = context;
        }

        // As apis aqui usam a mesma query que fiz na parte 3. Estão aqui para fins de teste

        // 3.1 - Listagem de solicitações
        [HttpGet("ListagemSolicitacoes")]
        public async Task<IActionResult> ListagemSolicitacoes(int mes, int ano)
        {
            try
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

                return Ok(query);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensagem = "Erro interno no servidor." });
            }
        }
        
        // 3.2 - Tipos de solicitações mais frequentes
        [HttpGet("SolicitacoesMaisFrequentes")]
        public async Task<IActionResult> SolicitacoesMaisFrequentes(int mes, int ano)
        {
            try
            {
                var query = await _context.Solicitacoes
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

                return Ok(query);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { mensagem = "Erro interno no servidor." });
            }
        }
    }
}