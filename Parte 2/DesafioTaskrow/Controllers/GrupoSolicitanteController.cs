﻿using DesafioTaskrow.Domain;
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
    
    public GrupoSolicitanteController(Contexto contexto, IGrupoSolicitanteService grupoSolicitanteService, ILogService logService)
    {
        _context = contexto;
        _grupoSolicitanteService = grupoSolicitanteService;
        _logService = logService;
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
        // catch (HierarquiaCiclicaException ex)
        // {
        //     await _logService.LogError("CriarGrupoSolicitante", "Erro ao criar grupo solicitante: " + ex.Message);
        //     return BadRequest(new { mensagem = ex.Message });
        // }
        catch (Exception ex)
        {
            await _logService.LogError("CriarGrupoSolicitante", "Erro inesperado ao criar grupo solicitante: " + ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensagem = "Erro interno no servidor." });
        }
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
            await _logService.LogError("ObterGruposSolicitantes", "Houve um erro ao consultar grupos solicitantes: " + ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> ObterGrupoSolicitantePorId(Guid id)
    {
        var grupo = await _grupoSolicitanteService.ObterPorId(id);
        
        if (grupo == null)
            return NotFound(new { mensagem = "Grupo não encontrado." });

        return Ok(grupo);
    }
    
}