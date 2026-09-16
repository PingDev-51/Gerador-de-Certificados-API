using GeradorCertificados.Aplicacao.Compartilhado;
using GeradorCertificados.Aplicacao.Modulos.Usuarios;
using GeradorCertificados.Dominio.Compartilhado.Auth;
using GeradorCertificados.WebApi.Compartilhado;
using GeradorCertificados.WebApi.Compartilhado.Http;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeradorCertificados.WebApi.Modulos.Usuario;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosController(
    IMediator mediator
) : ControllerBase
{
    [Authorize]
    [HttpGet("{usuarioId:guid}")]
    [ProducesResponseType<UsuarioResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<UsuarioResponse>> ObterPorId(
        Guid usuarioId,
        CancellationToken cancellationToken
    )
    {
        var resultado = await mediator.Send(
            new ObterUsuarioPorIdQuery(usuarioId),
            cancellationToken
        );

        if (resultado.IsFailed)
            return this.ProblemDetails(resultado);

        var response = new UsuarioResponse(
            resultado.Value.Id,
            resultado.Value.Email
        );

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("cadastro")]
    [ProducesResponseType<CadastrarUsuarioResponse>(
        StatusCodes.Status201Created)]
    public async Task<ActionResult<CadastrarUsuarioResponse>> Cadastrar(
        CadastrarUsuarioRequest request,
        CancellationToken cancellationToken
    )
    {
        var resultado = await mediator.Send(
            new CadastrarUsuarioCommand(
                request.Email,
                request.Senha
            ),
            cancellationToken
        );

        if (resultado.IsFailed)
            return this.ProblemDetails(resultado);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { usuarioId = resultado.Value },
            new CadastrarUsuarioResponse(
                resultado.Value
            )
        );
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AutenticacaoUsuarioResponse>> Autenticar(
       AutenticarUsuarioRequest request,
       CancellationToken cancellationToken
   )
    {
        var resultado = await mediator.Send(
            new AutenticarUsuarioCommand(
                request.Email,
                request.Senha
            ),
            cancellationToken
        );

        if (resultado.IsFailed)
            return this.ProblemDetails(resultado);

        return Ok(new AutenticacaoUsuarioResponse(
            resultado.Value.UsuarioId,
            resultado.Value.Token,
            resultado.Value.DataExpiracaoEmUtc
        ));
    }
}

