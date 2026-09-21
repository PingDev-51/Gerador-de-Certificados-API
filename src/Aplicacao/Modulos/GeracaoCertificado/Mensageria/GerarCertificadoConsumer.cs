using GeradorCertificados.Aplicacao.Modulos.GerarCertificadoZip;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Mensageria;

public sealed class GerarCertificadosConsumer(
    IRepositorioSolitacaoCertificados repositorioSolicitacoes,
    IMediator mediator,
    ILogger<GerarCertificadosConsumer> logger
) : IConsumer<GerarCertificadosMessage>
{
    public async Task Consume(
    ConsumeContext<GerarCertificadosMessage> context)
    {
        var mensagem = context.Message;

        var solicitacao = await repositorioSolicitacoes.SelecionarPorIdAsync(
            mensagem.SolicitacaoId,
            context.CancellationToken
        );

        if (solicitacao is null)
        {
            logger.LogInformation(
                "Solicitação {SolicitacaoId} não encontrada.",
                mensagem.SolicitacaoId
            );

            return;
        }

        solicitacao.IniciarGeracao();

        await repositorioSolicitacoes.EditarAsync(
            solicitacao.Id,
            solicitacao,
            context.CancellationToken
        );

        foreach (var certificado in solicitacao.Certificados)
        {
            var resultado = await mediator.Send(
                new GerarCertificadoCommand(certificado.Id),
                context.CancellationToken
            );

            if (resultado.IsFailed)
            {
                solicitacao.MarcarComoFalha();

                await repositorioSolicitacoes.EditarAsync(
                    solicitacao.Id,
                    solicitacao,
                    context.CancellationToken
                );

                logger.LogInformation(
                    "Falha ao gerar o certificado {CertificadoId} da solicitação {SolicitacaoId}.",
                    certificado.Id,
                    solicitacao.Id
                );

                return;
            }
        }

        var resultadoZip = await mediator.Send(
            new GerarZipCertificadosCommand(solicitacao.Id),
            context.CancellationToken
        );

        if (resultadoZip.IsFailed)
        {
            solicitacao.MarcarComoFalha();

            await repositorioSolicitacoes.EditarAsync(
                solicitacao.Id,
                solicitacao,
                context.CancellationToken
            );

            logger.LogInformation(
                "Falha ao gerar o ZIP da solicitação {SolicitacaoId}.",
                solicitacao.Id
            );

            return;
        }

        logger.LogInformation(
            "Processamento da solicitação {SolicitacaoId} concluído.",
            solicitacao.Id
        );
    }
}
