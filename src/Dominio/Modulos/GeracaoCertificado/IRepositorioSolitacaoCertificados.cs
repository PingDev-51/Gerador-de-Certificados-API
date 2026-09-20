using GeradorCertificados.Dominio.Compartilhado;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

public interface IRepositorioSolitacaoCertificados : IRepositorio<SolicitacaoCertificados>
{
    Task<SolicitacaoCertificados?> BuscarPorCursoIdAsync(
    Guid cursoId,
    CancellationToken cancellationToken
);
}