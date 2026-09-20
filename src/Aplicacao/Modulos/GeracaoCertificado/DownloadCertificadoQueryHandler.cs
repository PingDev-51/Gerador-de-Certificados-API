using FluentResults;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;

public sealed record DownloadCertificadoQuery(
    Guid CertificadoId
) : IRequest<Result<ArquivoResponse>>;

public sealed class DownloadCertificadoQueryHandler(
    IRepositorioCertificados repositorio
) : IRequestHandler<
    DownloadCertificadoQuery,
    Result<ArquivoResponse>>
{
    public async Task<Result<ArquivoResponse>> Handle(
        DownloadCertificadoQuery query,
        CancellationToken cancellationToken)
    {
        var certificado = await repositorio.SelecionarPorIdAsync(
            query.CertificadoId,
            cancellationToken
        );

        if (certificado is null)
            return Result.Fail("Certificado não encontrado.");

        if (string.IsNullOrWhiteSpace(certificado.CaminhoArquivo))
            return Result.Fail("O PDF deste certificado ainda não foi gerado.");

        if (!File.Exists(certificado.CaminhoArquivo))
            return Result.Fail("O arquivo do certificado não foi encontrado.");

        var conteudo = await File.ReadAllBytesAsync(
            certificado.CaminhoArquivo,
            cancellationToken
        );

        var nomeArquivo = Path.GetFileName(
            certificado.CaminhoArquivo
        );

        return Result.Ok(
            new ArquivoResponse(
                conteudo,
                nomeArquivo
            )
        );
    }
}