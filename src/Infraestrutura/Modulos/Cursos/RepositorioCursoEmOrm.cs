using System;
using GeradorCertificados.Dominio.Modulos.Cursos;
using GeradorCertificados.Infraestrutura.Compartilhado.Orm;

namespace GeradorCertificados.Infraestrutura.Modulos.Cursos;

public class RepositorioCursoEmOrm : RepositorioBaseEmOrm<Curso>, IRepositorioCurso
{
    public RepositorioCursoEmOrm(GeradorCertificadosDbContext dbContext) : base(dbContext)
    {
    }
}