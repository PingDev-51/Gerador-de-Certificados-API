using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

public sealed record CadastrarCertificadoRequest(
    string Aluno,
    string NomeCurso,
    uint CargaHoraria,
    DateTime DataConclusao
);

public sealed record CertificadoResponse(
    Guid Id,
    string NomeCurso
);



