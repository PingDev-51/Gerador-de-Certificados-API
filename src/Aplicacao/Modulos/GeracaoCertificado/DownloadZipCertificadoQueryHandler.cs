using FluentResults;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;

public sealed record DownloadZipCertificadosQuery(Guid CursoId)
    : IRequest<Result<ArquivoResponse>>;

public sealed class DownloadZipCertificadosQueryHandler(
    IRepositorioSolitacaoCertificados repositorio
) : IRequestHandler<DownloadZipCertificadosQuery, Result<ArquivoResponse>>
{
    public async Task<Result<ArquivoResponse>> Handle(
        DownloadZipCertificadosQuery query,
        CancellationToken cancellationToken)
    {
        var solicitacao = await repositorio.BuscarPorCursoIdAsync(
            query.CursoId,
            cancellationToken
        );

        if (solicitacao is null)
            return Result.Fail("Nenhuma solicitação de certificados encontrada para este curso.");

        if (string.IsNullOrWhiteSpace(solicitacao.CaminhoZip))
            return Result.Fail("O ZIP dos certificados ainda não foi gerado.");

        if (!File.Exists(solicitacao.CaminhoZip))
            return Result.Fail("O arquivo ZIP não foi encontrado.");

        var conteudo = await File.ReadAllBytesAsync(
            solicitacao.CaminhoZip,
            cancellationToken
        );

        var nomeArquivo = Path.GetFileName(solicitacao.CaminhoZip);

        return Result.Ok(
            new ArquivoResponse(conteudo, nomeArquivo)
        );
    }
}