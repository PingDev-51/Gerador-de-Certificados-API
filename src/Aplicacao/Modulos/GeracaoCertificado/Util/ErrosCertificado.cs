
using FluentResults;
using GeradorCertificados.Aplicacao.Compartilhado;
using GeradorCertificados.Dominio.Compartilhado;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Util;

public static class ErrosDeCertificado
{
    public static Error NaoAutorizado()
    {
        return new Error("É necessário estar autenticado com o perfil adequado para acessar pedidos.")
            .WithMetadata(nameof(TipoErro), TipoErro.NaoAutorizado);
    }

    public static Error NaoEncontrado(Guid idCliente)
    {
        return new Error("O certificado com este ID não foi encontrado.")
            .WithMetadata(nameof(TipoErro), TipoErro.NaoEncontrado)
            .WithMetadata("IdCliente", idCliente);
    }

    public static Error NomeDuplicado()
    {
        return new Error("Já existe um certificado com este Nome.")
            .WithMetadata(nameof(TipoErro), TipoErro.Conflito);
    }

    public static Error CadastroDuplicado()
    {
        return new Error("Já existe um certificado com esse mesmo aluno e curso")
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