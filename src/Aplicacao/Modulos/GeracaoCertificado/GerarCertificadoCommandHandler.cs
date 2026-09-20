using FluentResults;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;

public sealed record GerarCertificadoCommand(
    Guid CertificadoId
) : IRequest<Result>;

public sealed class GerarCertificadoCommandHandler(
    IRepositorioCertificados repositorioCertificados,
    GeradorPdfCertificao geradorPdf
) : IRequestHandler<GerarCertificadoCommand, Result>
{
    public async Task<Result> Handle(
        GerarCertificadoCommand command,
        CancellationToken cancellationToken)
    {
        var certificado = await repositorioCertificados.SelecionarPorIdAsync(
            command.CertificadoId,
            cancellationToken
        );

        if (certificado is null)
            return Result.Fail("Certificado não encontrado.");

        try
        {
            var pdf = geradorPdf.Gerar(
                certificado.NomeAluno,
                certificado.NomeCurso,
                certificado.CargaHoraria,
                certificado.DataConclusao
            );

            var pasta = Path.Combine(
                AppContext.BaseDirectory,
                "Certificados"
            );

            Directory.CreateDirectory(pasta);

            var nomeArquivo = $"{certificado.Id}.pdf";

            var caminhoArquivo = Path.Combine(
                pasta,
                nomeArquivo
            );

            await File.WriteAllBytesAsync(
                caminhoArquivo,
                pdf,
                cancellationToken
            );

            certificado.MarcarComoGerado(caminhoArquivo);

            await repositorioCertificados.EditarAsync(
                certificado.Id,
                certificado,
                cancellationToken
            );

            return Result.Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO AO GERAR PDF: {ex}");

            certificado.MarcarComoFalha();

            await repositorioCertificados.EditarAsync(
                certificado.Id,
                certificado,
                cancellationToken
            );

            return Result.Fail("Não foi possível gerar o certificado.");
        }
    }
}