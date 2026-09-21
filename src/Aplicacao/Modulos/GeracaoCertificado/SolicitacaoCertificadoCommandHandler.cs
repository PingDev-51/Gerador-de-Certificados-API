using FluentResults;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Mensageria;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using MassTransit;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;

public sealed record SolicitarCertificadosCommand(
    Guid CursoId,
    List<string> NomesAlunos
) : IRequest<Result<Guid>>;

public sealed class SolicitarCertificadosCommandHandler(
    IRepositorioSolitacaoCertificados repositorio,
    IPublishEndpoint publishEndpoint
) : IRequestHandler<SolicitarCertificadosCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        SolicitarCertificadosCommand command,
        CancellationToken cancellationToken)
    {
        var solicitacao = new SolicitacaoCertificados(
            command.CursoId,
            command.NomesAlunos
        );

        await repositorio.CadastrarAsync(
            solicitacao,
            cancellationToken
        );

        await publishEndpoint.Publish(
            new GerarCertificadosMessage(
                solicitacao.Id,
                DateTimeOffset.UtcNow
            ),
            cancellationToken
        );

        return Result.Ok(solicitacao.Id);
    }
}