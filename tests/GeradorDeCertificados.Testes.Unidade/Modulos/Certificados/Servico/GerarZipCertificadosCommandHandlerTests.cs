using FluentAssertions;
using GeradorCertificados.Aplicacao.Modulos.GerarCertificadoZip;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using Moq;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados.Servico;

[TestClass]
public class GerarZipCertificadosCommandHandlerTests
{
    [TestMethod]
    public async Task DeveGerarZipComSucesso()
    {
        var solicitacao = new SolicitacaoCertificados(
            Guid.NewGuid(),
            "C# Full Stack",
            40,
            DateTime.UtcNow,
            ["Kauan"]
        );

        solicitacao.Certificados[0].MarcarComoGerado(
            Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.pdf"
            )
        );

        File.WriteAllBytes(
            solicitacao.Certificados[0].CaminhoArquivo!,
            [1, 2, 3]
        );

        var repositorio = new Mock<IRepositorioSolitacaoCertificados>();

        repositorio
            .Setup(x => x.SelecionarPorIdAsync(
                solicitacao.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        var gerador = new GeradorDeZipCertificados();

        var handler = new GerarZipCertificadosCommandHandler(
            repositorio.Object,
            gerador
        );

        var resultado = await handler.Handle(
            new GerarZipCertificadosCommand(solicitacao.Id),
            CancellationToken.None
        );

        resultado.IsSuccess.Should().BeTrue();
        solicitacao.Status.Should().Be(
            StatusGeracaoCertificados.Concluido
        );

        repositorio.Verify(
            x => x.EditarAsync(
                solicitacao.Id,
                solicitacao,
                It.IsAny<CancellationToken>()),
            Times.AtLeastOnce
        );

        File.Delete(
            solicitacao.Certificados[0].CaminhoArquivo!
        );
    }
}