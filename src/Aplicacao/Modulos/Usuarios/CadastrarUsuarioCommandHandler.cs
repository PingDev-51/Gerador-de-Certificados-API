using FluentResults;
using GeradorCertificados.Dominio.Compartilhado.Auth;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.Usuarios;

public sealed record CadastrarUsuarioCommand(
    string Email,
    string Senha
) : IRequest<Result<Guid>>;

public sealed class CadastrarUsuarioCommandHandler(
    IGerenciadorDeIdentidade gerenciadorDeIdentidade
) : IRequestHandler<CadastrarUsuarioCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CadastrarUsuarioCommand command,
        CancellationToken cancellationToken = default
    )
    {
        Guid usuarioId = Guid.CreateVersion7();

        try
        {
            UsuarioDto usuario = await gerenciadorDeIdentidade.CadastrarAsync(
                usuarioId,
                command.Email,
                command.Senha
            );

            return Result.Ok(usuarioId);
        }
        catch (ValidacaoDeIdentidadeException ex)
        {
            return Result.Fail(
                new Error(ex.Message)
                    .WithMetadata("Campo", ex.Campo)
            );
        }
        catch (ConflitoDeIdentidadeException ex)
        {
            return Result.Fail(
                new Error(ex.Message)
            );
        }
    }
}