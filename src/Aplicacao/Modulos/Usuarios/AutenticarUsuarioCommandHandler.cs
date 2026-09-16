using FluentResults;
using GeradorCertificados.Aplicacao.Compartilhado;
using GeradorCertificados.Aplicacao.Modulos.Usuarios.Util;
using GeradorCertificados.Dominio.Compartilhado.Auth;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.Usuarios;

public sealed class AutenticarUsuarioCommandHandler(IGerenciadorDeIdentidade gerenciadorDeIdentidade,
    IEmissorDeTokens emissorDeTokens
) : IRequestHandler<AutenticarUsuarioCommand, Result<AccessTokenDoUsuarioDto>>
{
    public async Task<Result<AccessTokenDoUsuarioDto>> Handle(AutenticarUsuarioCommand request,
    CancellationToken cancellationToken = default)
    {
        var usuario = await gerenciadorDeIdentidade.ChecarValidadeDeSenhaAsync(
            request.Email,
            request.Senha
        );

        if (usuario is null)
        {
            return Result.Fail(ErrosDeUsuario.CredenciaisInvalidas());
        }

        Guid usuarioId = Guid.CreateVersion7();

        var accessToken = emissorDeTokens.CriarToken(
            usuarioId,
            request.Email,
            request.Senha
        );

        return Result.Ok(
            new AccessTokenDoUsuarioDto(
                usuarioId,
                accessToken.Token,
                accessToken.DataExpiracaoEmUtc
            )
        );

    }
}