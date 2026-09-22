using FluentResults;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Util;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;

public sealed record ListarCertificadosQuery(
    Guid CursoId
) : IRequest<Result<List<CertificadoDto>>>;

public sealed class ListarCertificadosQueryHandler(
    IRepositorioCertificados repositorioCertificados
) : IRequestHandler<ListarCertificadosQuery, Result<List<CertificadoDto>>>
{
    public async Task<Result<List<CertificadoDto>>> Handle(
        ListarCertificadosQuery query,
        CancellationToken cancellationToken = default)
    {
        var certificados = await repositorioCertificados
            .SelecionarPorCursoIdAsync(
                query.CursoId,
                cancellationToken
            );

        var certificadosDto = certificados
            .Select(certificado => new CertificadoDto(
                certificado.NomeAluno,
                certificado.NomeCurso,
                certificado.CargaHoraria,
                certificado.DataConclusao,
                certificado.CaminhoArquivo,
                certificado.DataGeracao,
                certificado.Status
            ))
            .ToList();

        return Result.Ok(certificadosDto);
    }
}
