using GeradorCertificados.Aplicacao.Compartilhado;
using GeradorCertificados.Dominio.Compartilhado;
using FluentResults;

namespace GeradorCertificados.Aplicacao.Modulos.Usuarios.Util;

public static class ErrosDeUsuario
{
    public static Error CredenciaisInvalidas()
    {
        return new Error("O endereço de email ou senha informados são inválidos.")
            .WithMetadata(nameof(TipoErro), TipoErro.Validacao)
            .WithMetadata("Campo", "Credenciais");
    }

    public static Error NaoAutorizado(Guid idUsuario)
    {
        return new Error("O usuário não tem autorização para esta operação.")
            .WithMetadata(nameof(TipoErro), TipoErro.NaoAutorizado)
            .WithMetadata("IdUsuario", idUsuario);
    }

    public static Error NaoEncontrado(Guid idUsuario)
    {
        return new Error("O usuário com este ID não foi encontrado.")
            .WithMetadata(nameof(TipoErro), TipoErro.NaoEncontrado)
            .WithMetadata("IdUsuario", idUsuario);
    }

    public static Error CadastroDuplicado()
    {
        return new Error("Já existe um usuário cadastrado com este email.")
            .WithMetadata(nameof(TipoErro), TipoErro.Conflito);
    }

    public static IEnumerable<Error> Validacao(IEnumerable<ErroValidacao> erros)
    {
        return erros.Select(erro => new Error(erro.Mensagem)
            .WithMetadata(nameof(TipoErro), TipoErro.Validacao)
            .WithMetadata("Campo", erro.Campo));
    }

    public static Error ConflitoDeIdentidade(string mensagem)
    {
        return new Error(mensagem)
            .WithMetadata(nameof(TipoErro), TipoErro.Conflito);
    }

    public static Error ValidacaoDeIdentidade(string campo, string mensagem)
    {
        return new Error(mensagem)
            .WithMetadata(nameof(TipoErro), TipoErro.Validacao)
            .WithMetadata("Campo", campo);
    }
}