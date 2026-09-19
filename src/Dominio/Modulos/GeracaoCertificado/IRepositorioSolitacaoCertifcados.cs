using GeradorCertificados.Dominio.Compartilhado;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

public interface IRepositorioSolitacaoCertificados : IRepositorio<Certificado>
{
    Task<SolicitacaoCertificados?> BuscarPorCursoIdAsync(
    Guid cursoId,
    CancellationToken cancellationToken
);
}