namespace GeradorCertificados.WebApi.Modulos.Usuario;

public sealed record CadastrarUsuarioRequest(
    string Email,
    string Senha
);

public sealed record CadastrarUsuarioResponse(
    Guid Id
);

public sealed record AutenticarUsuarioRequest(string Email, string Senha);

public sealed record AutenticacaoUsuarioResponse(
    Guid UsuarioId,
    string AccessToken,
    DateTime DataExpiracaoEmUtc
);

public sealed record UsuarioResponse(
    Guid Id,
    string Email
);