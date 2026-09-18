using FluentResults;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Util;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;

public sealed record ObterCertificadoQuery(
    Guid CertificadoId
) : IRequest<Result<CertificadoDto>>;

public sealed class ObterCertificadoPorIdQueryHandler(
    IRepositorioCertificados repositorioCertificados
) : IRequestHandler<ObterCertificadoQuery, Result<CertificadoDto>>
{
    public async Task<Result<CertificadoDto>> Handle(
        ObterCertificadoQuery query,
        CancellationToken cancellationToken = default
    )
    {
        Certificado? certificado = await repositorioCertificados.SelecionarPorIdAsync(
         query.CertificadoId,
         cancellationToken
        );

        if(certificado is null)
            return Result.Fail(ErrosDeCertificado.NaoEncontrado(query.CertificadoId));


        return Result.Ok(new CertificadoDto(
            certificado.NomeAluno,
            certificado.NomeCurso,
            certificado.CargaHoraria,
            certificado.DataConclusao,
            certificado.CaminhoArquivo,
            certificado.DataGeracao,
            certificado.Status
        ));
    }
}