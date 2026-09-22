using GeradorCertificados.Aplicacao.Modulos.Cursos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeradorCertificados.WebApi.Modulos.Cursos;

[ApiController]
[Route("cursos")]
public sealed class CursosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CadastrarCursoResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CadastrarCursoResponse>> Cadastrar(
        CadastrarCursoRequest request,
        CancellationToken cancellationToken
    )
    {
        var resultado = await mediator.Send(
            new CadastrarCursoCommand(
                request.Nome,
                request.Descricao,
                request.CargaHoraria,
                request.DataConclusao
            ),
            cancellationToken
        );

        if (resultado.IsFailed)
            return BadRequest(resultado.Errors);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { cursoId = resultado.Value },
            new CadastrarCursoResponse(
                resultado.Value,
                request.Nome
            )
        );
    }

    [HttpGet("{cursoId:guid}")]
    [ProducesResponseType<CursoResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<CursoResponse>> ObterPorId(
        Guid cursoId,
        CancellationToken cancellationToken
    )
    {
        var resultado = await mediator.Send(
            new ObterCursoPorIdQuery(cursoId),
            cancellationToken
        );

        if (resultado.IsFailed)
            return NotFound(resultado.Errors);

        var curso = resultado.Value;

        return Ok(new CursoResponse(
            curso.Id,
            curso.Nome,
            curso.Descricao,
            curso.CargaHoraria,
            curso.DataConclusao
        ));
    }
}