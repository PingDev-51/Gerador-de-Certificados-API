using FluentResults;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using MassTransit.DependencyInjection;
using MediatR;

public sealed record ConsultarStatusCertificadoQuery(
    Guid CursoId
) : IRequest<Result<StatusCertificadosResponse>>;

public sealed class ConsultarStatusCertificadosQueryHandler(
    IRepositorioSolitacaoCertificados repositorio
) : IRequestHandler<
    ConsultarStatusCertificadoQuery,
    Result<StatusCertificadosResponse>>
{
    public async Task<Result<StatusCertificadosResponse>> Handle(
        ConsultarStatusCertificadoQuery query,
        CancellationToken cancellationToken)
    {
        var solicitacao = await repositorio.BuscarPorCursoIdAsync(
            query.CursoId,
            cancellationToken
        );

        if (solicitacao is null)
            return Result.Fail("Nenhuma solicitação de certificados encontrada para este curso.");

        var response = new StatusCertificadosResponse(
            solicitacao.Id,
            solicitacao.Status,
            solicitacao.DataSolicitacao,
            solicitacao.DataConclusao,
            solicitacao.CaminhoZip
        );

        return Result.Ok(response);
    }
}