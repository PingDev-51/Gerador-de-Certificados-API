using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.Aplicacao.Modulos.Usuarios;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using GeradorCertificados.WebApi.Compartilhado.Http;
using GeradorCertificados.WebApi.Modulos.Usuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeradorCertificados.WebApi.Modulos.GeracaoCertificados;

[ApiController]
[Route("api/certificados")] // ajustar caminho após a implementação da rota de cursos para: /cursos/{cursoId}/certificados
public sealed class CertificadoController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("{certificadoId:guid}")]
    [ProducesResponseType<CertificadoResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<CertificadoResponse>> ObterPorId(
            Guid certificadoId,
            CancellationToken cancellationToken
        )
    {
        var resultado = await mediator.Send(
            new ObterCertificadoQuery(certificadoId),
            cancellationToken
        );

        if (resultado.IsFailed)
            return this.ProblemDetails(resultado);


        return Ok(resultado);
    }

    [HttpPost("cadastro")]
    [ProducesResponseType<CertificadoResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CertificadoResponse>> Cadastrar(
       CadastrarCertificadoRequest request,
       CancellationToken cancellationToken
   )
    {
        var resultado = await mediator.Send(new CadastrarCertificadoCommand(
        request.Aluno,
        request.NomeCurso,
        request.CargaHoraria,
        request.DataConclusao,
        request.CaminhoArquivo,
        request.DataGeracao,
        request.Status
        ), cancellationToken
        );

        return StatusCode(201, resultado.Value);
    }
}
