using DesafioTaskrow.Domain;
using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain.Dtos;
using DesafioTaskrow.Domain.Exceptions;
using DesafioTaskrow.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTaskrow.Controllers;

[ApiController]
[Route("[controller]")]
public class GrupoSolicitanteController : ControllerBase
{
    private readonly Contexto _context;
    private readonly IGrupoSolicitanteService _grupoSolicitanteService;
    private readonly ILogService _logService;

    public GrupoSolicitanteController(Contexto contexto, IGrupoSolicitanteService grupoSolicitanteService,
        ILogService logService)
    {
        _context = contexto;
        _grupoSolicitanteService = grupoSolicitanteService;
        _logService = logService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GrupoSolicitanteRetorno>))]
    public async Task<IActionResult> ObterGruposSolicitantes([FromQuery] string? nome)
    {
        try
        {
            await _logService.LogInfo("ObterGruposSolicitantes", "Nova tentativa de consulta de grupos solicitantes");

            var retorno = await _grupoSolicitanteService.ObterGruposSolicitantes(nome);

            return Ok(retorno);
        }
        catch (Exception ex)
        {
            await _logService.LogError("ObterGruposSolicitantes",
                "Houve um erro ao consultar grupos solicitantes: " + ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GrupoSolicitanteRetorno))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterGrupoSolicitantePorId(Guid id)
    {
        try
        {
            await _logService.LogInfo("ObterGrupoSolicitantePorId",
                "Nova tentativa de consulta do grupo solicitante " + id);

            var grupo = await _grupoSolicitanteService.ObterPorId(id);

            if (grupo == null)
                return NotFound(new { mensagem = "Grupo não encontrado." });

            return Ok(grupo);
        }
        catch (Exception ex)
        {
            await _logService.LogError("ObterGrupoSolicitantePorId",
                $"Houve um erro ao obter o grupo solicitante {id}: " + ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor");
        }
    }

    [HttpGet("ObterFilhos/{idPai}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GrupoSolicitanteRetorno>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterGrupoSolicitantesFilhos(Guid idPai)
    {
        try
        {
            var filhos = await _grupoSolicitanteService.ObterGruposSolicitantesFilhos(idPai);

            return Ok(filhos);
        }
        catch (Exception ex)
        {
            await _logService.LogError("ObterGrupoSolicitantesFilhos",
                $"Houve um erro ao obter os filhos do grupo {idPai}: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor");
        }
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CriarGrupoSolicitante([FromBody] GrupoSolicitanteDto grupoSolicitanteDto)
    {
        await _logService.LogInfo("CriarGrupoSolicitante", "Nova tentativa de criar grupo solicitante");

        try
        {
            var id = await _grupoSolicitanteService.CriarGrupoSolicitante(grupoSolicitanteDto);
            return CreatedAtAction(nameof(ObterGrupoSolicitantePorId), new { id }, new { id });
        }
        catch (NomeObrigatorioException ex)
        {
            await _logService.LogError("CriarGrupoSolicitante", "Erro ao criar grupo solicitante: " + ex.Message);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (GrupoPaiNaoEncontradoException ex)
        {
            await _logService.LogError("CriarGrupoSolicitante", "Erro ao criar grupo solicitante: " + ex.Message);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (HierarquiaMaximaException ex)
        {
            await _logService.LogError("CriarGrupoSolicitante", "Erro ao criar grupo solicitante: " + ex.Message);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (HierarquiaCiclicaException ex)
        {
            await _logService.LogError("CriarGrupoSolicitante", "Erro ao criar grupo solicitante: " + ex.Message);
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            await _logService.LogError("CriarGrupoSolicitante",
                "Erro inesperado ao criar grupo solicitante: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditarGrupoSolicitante(Guid id, [FromBody] GrupoSolicitanteDto grupoSolicitanteDto)
    {
        await _logService.LogInfo("EditarGrupoSolicitante", $"Tentativa de edição do grupo {id}");

        try
        {
            await _grupoSolicitanteService.EditarGrupoSolicitante(id, grupoSolicitanteDto);
            return NoContent();
        }
        catch (NomeObrigatorioException ex)
        {
            await _logService.LogError("EditarGrupoSolicitante", $"Erro ao editar grupo {id}: {ex.Message}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (GrupoNaoEncontradoException ex)
        {
            await _logService.LogError("EditarGrupoSolicitante", $"Erro ao editar grupo {id}: {ex.Message}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (GrupoPaiNaoEncontradoException ex)
        {
            await _logService.LogError("EditarGrupoSolicitante", $"Erro ao editar grupo {id}: {ex.Message}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (HierarquiaMaximaException ex)
        {
            await _logService.LogError("EditarGrupoSolicitante", $"Erro ao editar grupo {id}: {ex.Message}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (HierarquiaCiclicaException ex)
        {
            await _logService.LogError("EditarGrupoSolicitante", $"Erro ao editar grupo {id}: {ex.Message}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            await _logService.LogError("EditarGrupoSolicitante", $"Erro inesperado ao editar grupo {id}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletarGrupoSolicitante(Guid id)
    {
        await _logService.LogInfo("DeletarGrupoSolicitante", $"Tentativa de deletar grupo solicitante de Id {id}");

        try
        {
            await _grupoSolicitanteService.RemoverGrupoSolicitante(id);
            await _logService.LogInfo("DeletarGrupoSolicitante", $"Grupo de ID {id} deletado com sucesso.");
            return NoContent();
        }
        catch (GrupoNaoEncontradoException ex)
        {
            await _logService.LogError("EditarGrupoSolicitante", $"Erro ao remover grupo {id}: {ex.Message}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            await _logService.LogError("DeletarGrupoSolicitante",
                $"Erro inesperado ao deletar grupo solicitante: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
    }
}