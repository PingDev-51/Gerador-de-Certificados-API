using System;
using FluentResults;
using GeradorCertificados.Dominio.Modulos.Cursos;
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
        var curso = new Curso(
            command.Nome,
            command.CargaHoraria,
            command.DataConclusao,
            command.Descricao
        );

        var erros = curso.Validar();

        if (erros.Count > 0)
        {
            // tratar erros
        }

        await repositorioCurso.CadastrarAsync(
            curso,
            cancellationToken
        );

        return Result.Ok(curso.Id);
    }
}