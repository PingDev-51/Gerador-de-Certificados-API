namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Mensageria;

public sealed record GerarCertificadosMessage(
    Guid SolicitacaoId,
    DateTimeOffset SolicitadoEmUtc
);