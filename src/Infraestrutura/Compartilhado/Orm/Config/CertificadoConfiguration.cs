using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using GeradorCertificados.Dominio.Modulos.ModuloCurso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeradorCertificados.Infraestrutura.Compartilhado.Orm.Config;

public sealed class CertificadoConfiguration
    : IEntityTypeConfiguration<Certificado>
{
    public void Configure(EntityTypeBuilder<Certificado> builder)
    {
        builder.ToTable("TBCertificados");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedNever();

        builder.Property(c => c.NomeAluno)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.NomeCurso)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.CargaHoraria)
            .IsRequired();

        builder.Property(c => c.DataConclusao)
            .IsRequired();

        builder.Property(c => c.CaminhoArquivo)
            .HasMaxLength(500);

        builder.Property(c => c.DataGeracao);

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();
    }
}

public sealed class SolicitacaoCertificadosConfiguration
    : IEntityTypeConfiguration<SolicitacaoCertificados>
{
    public void Configure(EntityTypeBuilder<SolicitacaoCertificados> builder)
    {
        builder.ToTable("TBSolicitacoesCertificados");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.CursoId)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(s => s.CaminhoZip)
            .HasMaxLength(500);

        builder.Property(s => s.DataSolicitacao)
            .IsRequired();

        builder.Property(s => s.DataConclusao);

        builder.HasIndex(s => new
        {
            s.CursoId,
            s.Status
        });

        builder.HasOne<Curso>()
            .WithMany()
            .HasForeignKey(s => s.CursoId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}

