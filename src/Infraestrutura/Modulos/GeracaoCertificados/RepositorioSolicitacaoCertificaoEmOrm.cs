using GeradorCertificados.Dominio.Compartilhado;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using GeradorCertificados.Infraestrutura.Compartilhado.Orm;
using Microsoft.EntityFrameworkCore;

namespace GeradorCertificados.Infraestrutura.Modulos.GeracaoCertificados;

public sealed class RepositorioSolitacaoCertificadosEmOrm(
    GeradorCertificadosDbContext dbContext
) : RepositorioBaseEmOrm<SolicitacaoCertificados>(dbContext),
    IRepositorioSolitacaoCertificados
{
    public new async Task<SolicitacaoCertificados?> SelecionarPorIdAsync(
        Guid idSelecionado,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<SolicitacaoCertificados>()
            .Include(x => x.Certificados)
            .FirstOrDefaultAsync(
                x => x.Id == idSelecionado,
                cancellationToken
            );
    }

    public async Task<SolicitacaoCertificados?> BuscarPorCursoIdAsync(
    Guid cursoId,
    CancellationToken cancellationToken)
    {
        return await dbContext.Set<SolicitacaoCertificados>()
            .Where(x => x.CursoId == cursoId)
            .OrderByDescending(x => x.DataSolicitacao)
            .FirstOrDefaultAsync(cancellationToken);
    }
}