using FluentAssertions;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using Moq;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados;

[TestClass]
public class DownloadZipCertificadosQueryHandlerTests
{
    [TestMethod]
    public async Task DeveBaixarZipComSucesso()
    {
        var caminhoZip = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.zip"
        );

        var solicitacao = new SolicitacaoCertificados(
            Guid.NewGuid(),
            "C# Full Stack",
            40,
            DateTime.UtcNow,
            ["Kauan"]
        );

        solicitacao.IniciarGeracaoZip();
        solicitacao.MarcarComoConcluida(caminhoZip);

        File.WriteAllBytes(caminhoZip, [1, 2, 3]);

        var repositorio =
            new Mock<IRepositorioSolitacaoCertificados>();

        repositorio
            .Setup(x => x.BuscarPorCursoIdAsync(
                solicitacao.CursoId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        var handler = new DownloadZipCertificadosQueryHandler(
            repositorio.Object
        );

        var resultado = await handler.Handle(
            new DownloadZipCertificadosQuery(solicitacao.CursoId),
            CancellationToken.None
        );

        resultado.IsSuccess.Should().BeTrue();

        resultado.Value.Conteudo
            .Should().Equal([1, 2, 3]);

        resultado.Value.NomeArquivo
            .Should().Be(Path.GetFileName(caminhoZip));

        File.Delete(caminhoZip);
    }

    [TestMethod]
    public async Task DeveFalharQuandoSolicitacaoNaoExistir()
    {
        var cursoId = Guid.NewGuid();

        var repositorio =
            new Mock<IRepositorioSolitacaoCertificados>();

        repositorio
            .Setup(x => x.BuscarPorCursoIdAsync(
                cursoId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((SolicitacaoCertificados?)null);

        var handler = new DownloadZipCertificadosQueryHandler(
            repositorio.Object
        );

        var resultado = await handler.Handle(
            new DownloadZipCertificadosQuery(cursoId),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();
    }

    [TestMethod]
    public async Task DeveFalharQuandoZipNaoExistir()
    {
        var caminhoZip = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.zip"
        );

        var solicitacao = new SolicitacaoCertificados(
            Guid.NewGuid(),
            "C# Full Stack",
            40,
            DateTime.UtcNow,
            ["Kauan"]
        );

        solicitacao.IniciarGeracaoZip();
        solicitacao.MarcarComoConcluida(caminhoZip);

        var repositorio =
            new Mock<IRepositorioSolitacaoCertificados>();

        repositorio
            .Setup(x => x.BuscarPorCursoIdAsync(
                solicitacao.CursoId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        var handler = new DownloadZipCertificadosQueryHandler(
            repositorio.Object
        );

        var resultado = await handler.Handle(
            new DownloadZipCertificadosQuery(solicitacao.CursoId),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();
    }
}