using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

public record CertificadoDto(
    string Aluno,
    string NomeCurso,
    uint CargaHoraria,
    DateTime DataConclusao,
    string? CaminhoArquivo,
    DateTime? DataGeracao,
    StatusCertificado Status
);

public record StatusCertificadosResponse(
    Guid SolicitacaoId,
    StatusGeracaoCertificados Status,
    DateTime DataSolicitacao,
    DateTime? DataConclusao,
    string? CaminhoZip
);

public record ArquivoResponse(
    byte[] Conteudo,
    string NomeArquivo
);


// public Guid SolicitacaoId { get; private set; }

// public string NomeAluno { get; private set; } = string.Empty;

// public string NomeCurso { get; private set; } = string.Empty;

// public uint CargaHoraria { get; private set; }

// public DateTime DataConclusao { get; private set; }

// public string? CaminhoArquivo { get; private set; }

// public DateTime? DataGeracao { get; private set; }

// public StatusCertificado Status { get; private set; }
