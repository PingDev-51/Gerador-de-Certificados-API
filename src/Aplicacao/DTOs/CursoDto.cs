namespace GeradorCertificados.Aplicacao.DTOs;

public sealed record CadastrarCursoDto(
    string Nome,
    string? Descricao,
    uint CargaHoraria,
    DateTime DataConclusao
);
