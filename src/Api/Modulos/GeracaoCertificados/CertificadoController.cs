using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.WebApi.Compartilhado.Http;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeradorCertificados.WebApi.Modulos.GeracaoCertificados;

[ApiController]
[Route("api/certificados")] // ajustar caminho após a implementação da rota de cursos para: /cursos/{cursoId}/certificados
public sealed class CertificadoController(IMediator mediator) : ControllerBase
{
    [HttpPost("cadastro")]
    [ProducesResponseType<CadastrarCertificadoResponse>(StatusCodes.Status201Created)]
    public async Task<ActionResult<CadastrarCertificadoResponse>> Cadastrar(
       CadastrarCertificadoRequest request,
       CancellationToken cancellationToken
   )
    {
        await mediator.Send(new CadastrarCertificadoCommand(
        request.Aluno, 
        request.NomeCurso, 
        request.CargaHoraria, 
        request.DataConclusao, 
        request.CaminhoArquivo, 
        request.DataGeracao, 
        request.Status
        ), cancellationToken 
        );

        return StatusCode(StatusCodes.Status201Created);
    }
}
