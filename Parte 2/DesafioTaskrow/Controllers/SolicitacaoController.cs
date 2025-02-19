using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain.Dtos;
using DesafioTaskrow.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTaskrow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitacoesController : ControllerBase
{
    private readonly ISolicitacaoService _solicitacaoService;
    private readonly ILogService _logService;

    public SolicitacoesController(ISolicitacaoService solicitacaoService, ILogService logService)
    {
        _solicitacaoService = solicitacaoService;
        _logService = logService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterSolicitacoes()
    {
        try
        {
            var solicitacoes = await _solicitacaoService.ObterSolicitacoes();
            return Ok(solicitacoes);
        }
        catch (Exception ex)
        {
            await _logService.LogError("ObterSolicitacoes", $"Erro ao obter solicitações. Erro = {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CriarSolicitacao([FromBody] SolicitacaoDto solicitacao)
    {
        try
        {
            Guid id = await _solicitacaoService.CriarSolicitacao(solicitacao);
            await _logService.LogInfo("CriarSolicitacao", "Solicitação criada com sucesso.");
            return CreatedAtAction(nameof(CriarSolicitacao), new { id }, new { id });
        }
        catch (LimiteSolicitacaoExcedidoException ex)
        {
            await _logService.LogWarning("CriarSolicitacao", ex.Message);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            await _logService.LogWarning("CriarSolicitacao", ex.Message);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            await _logService.LogError("CriarSolicitacao", $"Erro ao criar solicitação. Erro = {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditarSolicitacao(Guid id, [FromBody] SolicitacaoDto solicitacao)
    {
        try
        {
            await _solicitacaoService.EditarSolicitacao(id, solicitacao);
            await _logService.LogInfo("EditarSolicitacao", $"Solicitação {id} editada com sucesso.");
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            await _logService.LogWarning("EditarSolicitacao", ex.Message);
            return NotFound();
        }
        catch (LimiteSolicitacaoExcedidoException ex)
        {
            await _logService.LogWarning("EditarSolicitacao", ex.Message);
            return NotFound(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            await _logService.LogError("EditarSolicitacao", $"Erro ao editar solicitação {id}. Erro = {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoverSolicitacao(Guid id)
    {
        try
        {
            await _solicitacaoService.RemoverSolicitacao(id);
            await _logService.LogInfo("RemoverSolicitacao", $"Solicitação {id} removida com sucesso.");
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            await _logService.LogWarning("RemoverSolicitacao", ex.Message);
            return NotFound(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            await _logService.LogError("RemoverSolicitacao", $"Erro ao remover solicitação {id}. Erro = {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
    }
}
