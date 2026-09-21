using FluentResults;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.GerarCertificadoZip;

public sealed record GerarZipCertificadosCommand(
    Guid SolicitacaoId
) : IRequest<Result>;

public sealed class GerarZipCertificadosCommandHandler(
    IRepositorioSolitacaoCertificados repositorioSolicitacoes,
    GeradorDeZipCertificados geradorZip
) : IRequestHandler<GerarZipCertificadosCommand, Result>
{
    public async Task<Result> Handle(
        GerarZipCertificadosCommand command,
        CancellationToken cancellationToken)
    {
        var solicitacao = await repositorioSolicitacoes.SelecionarPorIdAsync(
            command.SolicitacaoId,
            cancellationToken
        );

        if (solicitacao is null)
            return Result.Fail("Solicitação não encontrada.");

        var caminhos = solicitacao.Certificados
            .Where(x => !string.IsNullOrWhiteSpace(x.CaminhoArquivo))
            .Select(x => x.CaminhoArquivo!)
            .ToList();

        if (caminhos.Count == 0)
            return Result.Fail("Nenhum certificado foi gerado.");

        solicitacao.IniciarGeracaoZip();

        await repositorioSolicitacoes.EditarAsync(
           solicitacao.Id,
           solicitacao,
           cancellationToken
       );

        var zip = geradorZip.Gerar(caminhos);

        var pasta = Path.Combine(
            AppContext.BaseDirectory,
            "Certificados"
        );

        Directory.CreateDirectory(pasta);

        var nomeArquivo = $"certificados-{solicitacao.Id}.zip";

        var caminhoZip = Path.Combine(
            pasta,
            nomeArquivo
        );

        await File.WriteAllBytesAsync(
            caminhoZip,
            zip,
            cancellationToken
        );

        solicitacao.MarcarComoConcluida(caminhoZip);

        await repositorioSolicitacoes.EditarAsync(
            solicitacao.Id,
            solicitacao,
            cancellationToken
        );

        return Result.Ok();
    }
}