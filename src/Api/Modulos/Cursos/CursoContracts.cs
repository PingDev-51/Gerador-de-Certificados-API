namespace GeradorCertificados.WebApi.Modulos.Cursos;

public sealed record CadastrarCursoRequest(
    string Nome,
    string? Descricao,
    uint CargaHoraria,
    DateTime DataConclusao
);

public sealed record CadastrarCursoResponse(
    Guid Id,
    string Nome
);

public sealed record CursoResponse(
    Guid Id,
    string Nome,
    string? Descricao,
    uint? CargaHoraria,
    DateTime? DataConclusao
);