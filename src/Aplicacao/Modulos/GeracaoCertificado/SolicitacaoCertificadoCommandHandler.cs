using FluentResults;
using GeradorCertificados.Aplicacao.Compartilhado;
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
            return Result.Fail(
                TipoErro.NaoEncontrado.ObterMetadados(
                    "",
                    "Curso não encontrado."
                )
            );

        if (command.NomesAlunos.Count == 0)
            return Result.Fail(
                TipoErro.Validacao.ObterMetadados(
                    nameof(command.NomesAlunos),
                    "A lista de alunos deve possuir pelo menos um aluno."
                )
            );

        var solicitacaoExistente =
            await repositorioSolicitacoes.BuscarPorCursoIdAsync(
                command.CursoId,
                cancellationToken
            );

        if (solicitacaoExistente is not null && solicitacaoExistente.EstaProcessando())
            return Result.Fail(
                TipoErro.Conflito.ObterMetadados(
                    "",
                    "Já existe uma solicitação de certificados em processamento para este curso."
                )
            );

        var solicitacao = new SolicitacaoCertificados(
             curso.Id,
             curso.Nome,
             curso.CargaHoraria!.Value,
             curso.DataConclusao!.Value,
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