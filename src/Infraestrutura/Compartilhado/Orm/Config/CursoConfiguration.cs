using GeradorCertificados.Dominio.Modulos.Cursos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeradorCertificados.Infraestrutura.Compartilhado.Orm.Config;

public sealed class CursoConfiguration : IEntityTypeConfiguration<Curso>
{
    public void Configure(EntityTypeBuilder<Curso> builder)
    {
        builder.ToTable("TB_Curso");

        builder.HasKey(curso => curso.Id);

        builder.Property(curso => curso.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(curso => curso.Descricao)
            .HasMaxLength(500);

        builder.Property(curso => curso.CargaHoraria)
            .IsRequired();

        builder.Property(curso => curso.DataConclusao)
            .IsRequired();

        builder.Property(curso => curso.UsuarioId)
            .IsRequired();
    }
}