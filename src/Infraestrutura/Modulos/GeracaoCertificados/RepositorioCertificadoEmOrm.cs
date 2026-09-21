using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using GeradorCertificados.Infraestrutura.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace GeradorCertificados.Infraestrutura.Modulos.GeracaoCertificados;

public sealed class RepositorioCertificadoEmOrm(GeradorCertificadosDbContext dbContext) : RepositorioBaseEmOrm<Certificado>(dbContext), IRepositorioCertificados
{
    public async Task<List<Certificado>> SelecionarPorCursoIdAsync(Guid cursoId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Certificado>()
            .Where(certificado =>
                dbContext.Set<SolicitacaoCertificados>()
                    .Any(solicitacao =>
                        solicitacao.Id == certificado.SolicitacaoId &&
                        solicitacao.CursoId == cursoId
                    )
            )
            .ToListAsync(cancellationToken);
    }
}
