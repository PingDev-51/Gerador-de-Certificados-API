using GeradorCertificados.Aplicacao.Modulos.Usuarios.Util;
using GeradorCertificados.Dominio.Compartilhado.Auth;
using FluentResults;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.Usuarios;

public sealed record ObterUsuarioPorIdQuery(
    Guid UsuarioId
) : IRequest<Result<UsuarioDto>>;

public sealed class ObterUsuarioPorIdQueryHandler(
    IProvedorDeUsuario provedorDeUsuario
) : IRequestHandler<ObterUsuarioPorIdQuery, Result<UsuarioDto>>
{
    public async Task<Result<UsuarioDto>> Handle(
        ObterUsuarioPorIdQuery query,
        CancellationToken cancellationToken = default
    )
    {
        if (query.UsuarioId != provedorDeUsuario.Id)
            return Result.Fail(
                ErrosDeUsuario.NaoAutorizado(
                    provedorDeUsuario.Id!.Value
                )
            );

        if (string.IsNullOrWhiteSpace(provedorDeUsuario.Email))
            return Result.Fail(
                ErrosDeUsuario.NaoEncontrado(query.UsuarioId)
            );

        return Result.Ok(
            new UsuarioDto(
                query.UsuarioId,
                provedorDeUsuario.Email!,
                provedorDeUsuario.Senha!
            )
        );
    }
}