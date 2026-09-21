using System;
using FluentResults;
using MediatR;

namespace GeradorCertificados.Aplicacao.Modulos.Cursos;

public sealed record CadastrarCursoCommand(
    string Nome,
    string? Descricao,
    uint CargaHoraria,
    DateTime DataConclusao
) : IRequest<Result<Guid>>;

public sealed class CadastrarCursoCommandHandler(
    IRepositorioCurso repositorioCurso
) : IRequestHandler<CadastrarCursoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CadastrarCursoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        // ...
    }
}