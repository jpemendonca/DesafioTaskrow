using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain;
using DesafioTaskrow.Application.Dtos;
using DesafioTaskrow.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DesafioTaskrow.Application.Servicos;

public class VerificacaoSolicitacaoService : IVerificacaoSolicitacaoService
{
    private readonly Contexto _context;

    public VerificacaoSolicitacaoService(Contexto context)
    {
        _context = context;
    }
    
    public async Task VerificarSolicitacao(SolicitacaoDto solicitacao)
    {
        await VerificarGrupoSolicitante(solicitacao.GrupoSolicitanteId);
        await VerificarTipoSolicitacao(solicitacao.TipoSolicitacaoId);
        await VerificarLimiteMensal(solicitacao.GrupoSolicitanteId);
    }

    public async Task VerificarGrupoSolicitante(Guid grupoId)
    {
        bool existeGrupoSolicitante =
            await _context.GruposSolicitantes.AnyAsync(x => x.Id == grupoId);

        if (!existeGrupoSolicitante)
        {
            throw new KeyNotFoundException("Grupo solicitante não encontrado.");
        }
    }

    public async Task VerificarTipoSolicitacao(Guid tipoId)
    {
        bool existeTipoSolicitacao =
            await _context.TiposSolicitacoes.AnyAsync(x => x.Id == tipoId);

        if (!existeTipoSolicitacao)
        {
            throw new KeyNotFoundException("Tipo de solicitação não encontrado.");
        }
    }

    public async Task VerificarLimiteMensal(Guid grupoId)
    {
        int mes = DateTime.Now.Month;
        int ano = DateTime.Now.Year;

        var limiteMensal = _context.LimitesGrupos
            .FirstOrDefault(x => x.GrupoSolicitanteId == grupoId && x.Ano == ano && x.Mes == mes)
            ?.LimiteMensal;
        
        if (limiteMensal is null)
        {
            Guid? grupoPaiId = _context.GruposSolicitantes.FirstOrDefault(x => x.Id == grupoId)
                ?.GrupoSolicitantePaiId;

            if (grupoPaiId is not null)
            {
                limiteMensal = await _context.LimitesGrupos
                    .Where(x => x.GrupoSolicitante.GrupoSolicitantePaiId == grupoPaiId && x.Ano == ano && x.Mes == mes)
                    .Select(x => x.LimiteMensal)
                    .FirstOrDefaultAsync();
            }
        }
        
        if (limiteMensal is null)
        {
            throw new KeyNotFoundException("Você precisa cadastrar um limite mensal para essa solicitação");
        }
        
        int quantidadeMensalSolicitacaoGrupo = await _context.Solicitacoes
            .CountAsync(x => x.GrupoSolicitanteId == grupoId
                             && x.DataCriacao.Year == ano 
                             && x.DataCriacao.Month == mes);

        if (quantidadeMensalSolicitacaoGrupo >= limiteMensal)
        {
            throw new LimiteSolicitacaoExcedidoException("O limite de solicitações criadas para esse grupo no mês atual já foi excedido");
        }
    }
}