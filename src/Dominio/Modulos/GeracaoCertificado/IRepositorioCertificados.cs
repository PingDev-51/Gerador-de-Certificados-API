using GeradorCertificados.Dominio.Compartilhado;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

public interface IRepositorioCertificados : IRepositorio<Certificado>
{
    Task<List<Certificado>> SelecionarPorCursoIdAsync(
     Guid cursoId,
     CancellationToken cancellationToken = default
    );
}