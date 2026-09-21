using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.WebApi.Compartilhado.Http;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeradorCertificados.WebApi.Modulos.GeracaoCertificados;

[Authorize]
[ApiController]
[Route("cursos/{cursoId:guid}/certificados")]
public sealed class CertificadoController(
    IMediator mediator
) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Solicitar(
        Guid cursoId,
        SolicitarCertificadosRequest request,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(
            new SolicitarCertificadosCommand(
                cursoId,
                request.NomesAlunos
            ),
            cancellationToken
        );

        if (resultado.IsFailed)
            return this.ProblemDetails(resultado);

        return Accepted(new
        {
            solicitacaoId = resultado.Value
        });
    }


    [HttpGet("~/cursos/{cursoId:guid}/status")]
    [ProducesResponseType<StatusCertificadosResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ConsultarStatus(
        Guid cursoId,
        CancellationToken cancellationToken)
    {
        var resultado = await mediator.Send(
            new ConsultarStatusCertificadoQuery(cursoId),
            cancellationToken
        );

        if (resultado.IsFailed)
            return this.ProblemDetails(resultado);

        return Ok(resultado.Value);
    }
}