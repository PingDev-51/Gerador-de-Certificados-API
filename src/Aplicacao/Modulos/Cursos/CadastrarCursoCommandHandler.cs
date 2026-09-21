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