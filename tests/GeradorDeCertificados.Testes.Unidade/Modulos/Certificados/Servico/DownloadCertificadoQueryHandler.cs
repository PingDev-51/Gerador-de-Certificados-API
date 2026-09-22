using FluentAssertions;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using Moq;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados;

[TestClass]
public class DownloadCertificadoQueryHandlerTests
{
    [TestMethod]
    public async Task DeveBaixarCertificadoComSucesso()
    {
        var caminhoArquivo = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.pdf"
        );

        var certificado = new Certificado(
            Guid.NewGuid(),
            "Kauan",
            "C# Full Stack",
            40,
            DateTime.UtcNow
        );

        certificado.MarcarComoGerado(caminhoArquivo);

        File.WriteAllBytes(caminhoArquivo, [1, 2, 3]);

        var repositorio = new Mock<IRepositorioCertificados>();

        repositorio
            .Setup(x => x.SelecionarPorIdAsync(
                certificado.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(certificado);

        var handler = new DownloadCertificadoQueryHandler(
            repositorio.Object
        );

        var resultado = await handler.Handle(
            new DownloadCertificadoQuery(certificado.Id),
            CancellationToken.None
        );

        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Conteudo.Should().Equal([1, 2, 3]);
        resultado.Value.NomeArquivo.Should()
            .Be(Path.GetFileName(caminhoArquivo));

        File.Delete(caminhoArquivo);
    }

    [TestMethod]
    public async Task DeveFalharQuandoCertificadoNaoExistir()
    {
        var certificadoId = Guid.NewGuid();

        var repositorio = new Mock<IRepositorioCertificados>();

        repositorio
            .Setup(x => x.SelecionarPorIdAsync(
                certificadoId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Certificado?)null);

        var handler = new DownloadCertificadoQueryHandler(
            repositorio.Object
        );

        var resultado = await handler.Handle(
            new DownloadCertificadoQuery(certificadoId),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();
    }

    [TestMethod]
    public async Task DeveFalharQuandoArquivoNaoExistir()
    {
        var caminhoArquivo = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.pdf"
        );

        var certificado = new Certificado(
            Guid.NewGuid(),
            "Kauan",
            "C# Full Stack",
            40,
            DateTime.UtcNow
        );

        certificado.MarcarComoGerado(caminhoArquivo);

        var repositorio = new Mock<IRepositorioCertificados>();

        repositorio
            .Setup(x => x.SelecionarPorIdAsync(
                certificado.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(certificado);

        var handler = new DownloadCertificadoQueryHandler(
            repositorio.Object
        );

        var resultado = await handler.Handle(
            new DownloadCertificadoQuery(certificado.Id),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();
    }
}