using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Application.Dtos;
using DesafioTaskrow.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTaskrow.Controllers;

[ApiController]
[Route("[controller]")]
public class LimitesController : ControllerBase
{
    private readonly ILimiteService _limiteService;
    private readonly ILogService _logService;

    public LimitesController(ILimiteService limiteService, ILogService logService)
    {
        _limiteService = limiteService;
        _logService = logService;
    }

    [HttpGet]
    public async Task<IActionResult> ObterLimitesGrupos()
    {
        try
        {
            var limites = await _limiteService.ObterLimitesGrupos();
            return Ok(limites);
        }
        catch (Exception ex)
        {
            await _logService.LogError("ObterLimitesGrupos", $"Erro ao obter limites de grupos. {ex.Message}");
            return StatusCode(500, "Erro interno ao processar a solicitação.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CriarLimiteGrupo([FromBody] LimiteGrupoDto limiteGrupo)
    {
        try
        {
            var id = await _limiteService.CriarLimiteGrupo(limiteGrupo);
            return CreatedAtAction(nameof(ObterLimitesGrupos), new { id }, null);
        }
        catch (LimiteJaExisteException ex)
        {
            await _logService.LogError("CriarLimiteGrupo", $"Erro ao criar limite grupo. {ex.Message}");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            await _logService.LogError("CriarLimiteGrupo", $"Erro ao criar limite grupo. {ex.Message}");
            return StatusCode(500, "Erro interno ao processar a solicitação.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditarLimiteGrupo(Guid id, [FromBody] LimiteGrupoDto limiteGrupo)
    {
        try
        {
            await _limiteService.EditarLimiteGrupo(id, limiteGrupo);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            await _logService.LogError("EditarLimiteGrupo", $"Erro ao editar limite grupo. {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            await _logService.LogError("EditarLimiteGrupo", $"Erro ao editar limite grupo. {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            await _logService.LogError("EditarLimiteGrupo", $"Erro ao editar limite grupo. {ex.Message}");
            return StatusCode(500, "Erro interno ao processar a solicitação.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverLimiteGrupo(Guid id)
    {
        try
        {
            await _limiteService.RemoverLimiteGrupo(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            await _logService.LogError("RemoverLimiteGrupo", $"Erro ao remover limite grupo. {ex.Message}");
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            await _logService.LogError("RemoverLimiteGrupo", $"Erro ao remover limite grupo. {ex.Message}");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            await _logService.LogError("RemoverLimiteGrupo", $"Erro ao remover limite grupo. {ex.Message}");
            return StatusCode(500, "Erro interno ao processar a solicitação.");
        }
    }

    [HttpGet("{idGrupo}/{idTipo}/{ano}/{mes}")]
    public async Task<IActionResult> ObterLimiteGrupoPorTipoSolicitacaoMesEspecifico(Guid idGrupo, Guid idTipo, int ano, int mes)
    {
        try
        {
            var limites = await _limiteService.ObterLimiteGrupoPorTipoSolicitacaoMesEspecifico(idGrupo, idTipo, ano, mes);
            return Ok(limites);
        }
        catch (Exception ex)
        {
            await _logService.LogError("ObterLimiteGrupoPorTipoSolicitacaoMesEspecifico", $"Erro ao buscar limite grupo. {ex.Message}");
            return StatusCode(500, "Erro interno ao processar a solicitação.");
        }
    }
}
