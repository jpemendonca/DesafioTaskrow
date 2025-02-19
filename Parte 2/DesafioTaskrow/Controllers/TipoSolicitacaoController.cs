using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain;
using DesafioTaskrow.Application.Dtos;
using DesafioTaskrow.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTaskrow.Controllers;

[ApiController]
[Route("[controller]")]
public class TipoSolicitacaoController : ControllerBase
{
    private readonly Contexto _context;
    private readonly ILogService _logService;
    private readonly ITipoSolicitacaoService _service;

    public TipoSolicitacaoController(Contexto contexto, ILogService logService, ITipoSolicitacaoService service)
    {
        _context = contexto;
        _logService = logService;
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoSolicitacaoRetorno>))]
    public async Task<IActionResult> ObterTiposSolicitacao()
    {
        try
        {
            await _logService.LogInfo("ObterTiposSolicitacao", "Nova tentativa de consulta de tipos de solicitação");

            var retorno = await _service.ObterTiposSolicitacao();

            return Ok(retorno);
        }
        catch (Exception ex)
        {
            await _logService.LogError("ObterTiposSolicitacao",
                "Houve um erro ao consultar tipos de solicitação: " + ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor");
        }
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CriarTiposSolicitacao([FromQuery] string nome)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                await _logService.LogWarning("CriarTiposSolicitacao", "Tentativa de criação sem nome informado.");
                return BadRequest(new { mensagem = "O nome do tipo de solicitação é obrigatório." });
            }

            Guid id = await _service.CriarTiposSolicitacao(nome);
            
            await _logService.LogInfo("CriarTiposSolicitacao", $"Tipo de solicitação '{nome}' id = {id} criado com sucesso.");
        
            return CreatedAtAction(nameof(CriarTiposSolicitacao), new { id }, new { id });
        }
        catch (Exception ex)
        {
            await _logService.LogError("CriarTiposSolicitacao", $"Erro ao criar tipo de solicitação. Erro = {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
    }


[HttpPut("{id:guid}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public async Task<IActionResult> EditarTiposSolicitacao(Guid id, string nome, bool ativo)
{
    try
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            await _logService.LogWarning("EditarTiposSolicitacao", $"Tentativa de edição do ID {id} sem nome informado.");
            return BadRequest(new { mensagem = "O nome do tipo de solicitação é obrigatório." });
        }

        await _service.EditarTiposSolicitacao(id, nome, ativo);
        await _logService.LogInfo("EditarTiposSolicitacao", $"Tipo de solicitação {id} atualizado para '{nome}', ativo: {ativo}.");
        return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
        await _logService.LogWarning("EditarTiposSolicitacao", $"Tentativa de edição falhou. Tipo de solicitação {id} não encontrado.");
        return NotFound(new { mensagem = ex.Message });
    }
    catch (Exception ex)
    {
        await _logService.LogError("EditarTiposSolicitacao", $"Erro ao editar tipo de solicitação {id}. Erro = {ex.Message}");
        return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
    }
}

[HttpDelete("{id:guid}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public async Task<IActionResult> ExcluirTiposSolicitacao(Guid id)
{
    try
    {
        await _service.ExcluirTiposSolicitacao(id);
        await _logService.LogInfo("ExcluirTiposSolicitacao", $"Tipo de solicitação {id} excluído com sucesso.");
        return NoContent();
    }
    catch (KeyNotFoundException ex)
    {
        await _logService.LogWarning("ExcluirTiposSolicitacao", $"Tentativa de exclusão falhou. Tipo de solicitação {id} não encontrado.");
        return NotFound(new { mensagem = ex.Message });
    }
    catch (TipoSolicitacaoTemSolicitacaoException ex)
    {
        await _logService.LogWarning("ExcluirTiposSolicitacao", $"Tentativa de exclusão falhou. Tipo de solicitação {id} possui solicitações associadas.");
        return BadRequest(new { mensagem = ex.Message });
    }
    catch (Exception ex)
    {
        await _logService.LogError("ExcluirTiposSolicitacao", $"Erro ao excluir tipo de solicitação {id}. Erro = {ex.Message}");
        return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
    }
}

}