using FluentResults;
using MediatR;

namespace GeradorCertificados.Aplicacao.Compartilhado;

public sealed record AutenticarUsuarioCommand(
    string Email,
    string Senha
) : IRequest<Result<AccessTokenDoUsuarioDto>>;


public sealed record AccessTokenDoUsuarioDto(
    Guid UsuarioId,
    string Token,
    DateTime DataExpiracaoEmUtc
);
