using DesafioTaskrow.Domain;
using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTaskrow.Controllers;

[ApiController]
[Route("[controller]")]
public class GrupoSolicitanteController : ControllerBase
{
    private readonly Contexto _context;
    private readonly IGrupoSolicitanteService _grupoSolicitanteService;

    public GrupoSolicitanteController(Contexto contexto, IGrupoSolicitanteService grupoSolicitanteService)
    {
        _context = contexto;
        _grupoSolicitanteService = grupoSolicitanteService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GrupoSolicitante>))]
    public async Task<IActionResult> ObterGruposSolicitantes([FromQuery] string? nome)
    {
        try
        {
            var retorno = await _grupoSolicitanteService.ObterGruposSolicitantes(nome);

            return Ok(retorno);
        }
        catch (Exception ex)
        {
            // TODO: LOG
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro no servidor");
        }
    }
}