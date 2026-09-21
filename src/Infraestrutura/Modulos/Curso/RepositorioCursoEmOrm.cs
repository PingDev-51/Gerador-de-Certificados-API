using GeradorCertificados.Dominio.Modulos.ModuloCurso;
using GeradorCertificados.Infraestrutura.Compartilhado.Orm;

public sealed class RepositorioCursoEmOrm(GeradorCertificadosDbContext dbContext) : RepositorioBaseEmOrm<Curso>(dbContext), IRepositorioCurso;
