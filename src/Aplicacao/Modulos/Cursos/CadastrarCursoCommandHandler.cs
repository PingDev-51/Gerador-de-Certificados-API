using FluentResults;
using GeradorCertificados.Aplicacao.Modulos.Cursos.Util;
using GeradorCertificados.Dominio.Compartilhado.Auth;
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
    IRepositorioCurso repositorioCurso,
    IProvedorDeUsuario provedorDeUsuario
) : IRequestHandler<CadastrarCursoCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CadastrarCursoCommand command,
        CancellationToken cancellationToken = default
    )
    {
        if (!provedorDeUsuario.Id.HasValue)
            return Result.Fail("Usuário não autenticado.");

        var curso = new Curso(
            command.Nome,
            command.CargaHoraria,
            command.DataConclusao,
            command.Descricao
        );

        curso.UsuarioId = provedorDeUsuario.Id.Value;

        var erros = curso.Validar();

        if (erros.Count > 0)
            return Result.Fail(ErrosDeCurso.Validacao(erros));

        await repositorioCurso.CadastrarAsync(
            curso,
            cancellationToken
        );

        return Result.Ok(curso.Id);
    }
}