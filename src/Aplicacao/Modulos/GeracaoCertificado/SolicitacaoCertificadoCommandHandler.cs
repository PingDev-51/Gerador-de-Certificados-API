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
    IRepositorioCurso repositorioCurso,
    IRepositorioSolitacaoCertificados repositorioSolicitacoes,
    IPublishEndpoint publishEndpoint
) : IRequestHandler<SolicitarCertificadosCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        SolicitarCertificadosCommand command,
        CancellationToken cancellationToken)
    {
        var curso = await repositorioCurso.SelecionarPorIdAsync(
            command.CursoId,
            cancellationToken
        );

        if (curso is null)
            return Result.Fail("Curso não encontrado.");

        if (command.NomesAlunos.Count == 0)
            return Result.Fail(
                "A lista de alunos deve possuir pelo menos um aluno."
            );

        var solicitacao = new SolicitacaoCertificados(
            curso.Id,
            command.NomesAlunos
        );

        await repositorioSolicitacoes.CadastrarAsync(
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