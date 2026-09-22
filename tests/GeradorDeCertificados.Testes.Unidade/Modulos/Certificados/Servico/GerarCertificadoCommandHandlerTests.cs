using FluentAssertions;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using Moq;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados.Servico;

[TestClass]
public class GerarCertificadoCommandHandlerTests
{
    [TestMethod]
    public async Task DeveGerarCertificadoComSucesso()
    {
        var certificado = new Certificado(
            Guid.NewGuid(),
            "Kauan",
            "C# Full Stack",
            40,
            DateTime.UtcNow
        );

        var repositorio = new Mock<IRepositorioCertificados>();

        repositorio
            .Setup(x => x.SelecionarPorIdAsync(
                certificado.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(certificado);

        var geradorPdf = new Mock<GeradorPdfCertificao>();

        geradorPdf
            .Setup(x => x.Gerar(
                certificado.NomeAluno,
                certificado.NomeCurso,
                certificado.CargaHoraria,
                certificado.DataConclusao))
            .Returns([1, 2, 3]);

        var handler = new GerarCertificadoCommandHandler(
            repositorio.Object,
            geradorPdf.Object
        );

        var resultado = await handler.Handle(
            new GerarCertificadoCommand(certificado.Id),
            CancellationToken.None
        );

        resultado.IsSuccess.Should().BeTrue();
        certificado.Status.Should().Be(StatusCertificado.Gerado);
        certificado.CaminhoArquivo.Should().NotBeNull();

        repositorio.Verify(
            x => x.EditarAsync(
                certificado.Id,
                certificado,
                It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [TestMethod]
    public async Task DeveFalharQuandoCertificadoNaoExistir()
    {
        var repositorio = new Mock<IRepositorioCertificados>();

        repositorio
            .Setup(x => x.SelecionarPorIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Certificado?)null);

        var geradorPdf = new Mock<GeradorPdfCertificao>();

        var handler = new GerarCertificadoCommandHandler(
            repositorio.Object,
            geradorPdf.Object
        );

        var resultado = await handler.Handle(
            new GerarCertificadoCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();

        geradorPdf.Verify(
            x => x.Gerar(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<uint>(),
                It.IsAny<DateTime>()),
            Times.Never
        );
    }
}