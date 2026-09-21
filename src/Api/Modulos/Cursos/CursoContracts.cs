using System;

namespace GeradorCertificados.WebApi.Modulos.Cursos;

public sealed record CadastrarCursoRequest(
    string Nome,
    string? Descricao,
    uint CargaHoraria,
    DateTime DataConclusao
);

public sealed record CadastrarCursoResponse(
    Guid Id
);

