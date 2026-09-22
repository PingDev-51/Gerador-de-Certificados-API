using FluentAssertions;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using Moq;
using QuestPDF.Infrastructure;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados.Servico;

[TestClass]
public class GerarCertificadoCommandHandlerTests
{
    [TestMethod]
    public async Task DeveGerarCertificadoComSucesso()
    {
        QuestPDF.Settings.License = LicenseType.Evaluation;

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

        var geradorPdf = new GeradorPdfCertificao();

        var handler = new GerarCertificadoCommandHandler(
            repositorio.Object,
            geradorPdf
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

        if (certificado.CaminhoArquivo is not null &&
            File.Exists(certificado.CaminhoArquivo))
        {
            File.Delete(certificado.CaminhoArquivo);
        }
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

        var geradorPdf = new GeradorPdfCertificao();

        var handler = new GerarCertificadoCommandHandler(
            repositorio.Object,
            geradorPdf
        );

        var resultado = await handler.Handle(
            new GerarCertificadoCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();
    }
}