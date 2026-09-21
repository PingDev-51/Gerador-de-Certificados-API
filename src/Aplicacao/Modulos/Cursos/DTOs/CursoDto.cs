namespace GeradorCertificados.Aplicacao.Modulos.Cursos.DTOs;

public sealed record CursoDto(
    Guid Id,
    string Nome,
    string? Descricao,
    uint CargaHoraria,
    DateTime DataConclusao
);