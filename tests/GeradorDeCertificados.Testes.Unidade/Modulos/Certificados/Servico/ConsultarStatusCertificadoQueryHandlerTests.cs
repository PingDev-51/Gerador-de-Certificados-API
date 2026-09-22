using FluentAssertions;

using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using Moq;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados.Servico;

[TestClass]
public class ConsultarStatusCertificadoQueryHandlerTests
{
    [TestMethod]
    public async Task DeveConsultarStatusComSucesso()
    {
        var solicitacao = new SolicitacaoCertificados(
            Guid.NewGuid(),
            "C# Full Stack",
            40,
            DateTime.UtcNow,
            ["Kauan"]
        );

        var repositorio = new Mock<IRepositorioSolitacaoCertificados>();

        repositorio
            .Setup(x => x.BuscarPorCursoIdAsync(
                solicitacao.CursoId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(solicitacao);

        var handler = new ConsultarStatusCertificadosQueryHandler(
            repositorio.Object
        );

        var resultado = await handler.Handle(
            new ConsultarStatusCertificadoQuery(solicitacao.CursoId),
            CancellationToken.None
        );

        resultado.IsSuccess.Should().BeTrue();

        resultado.Value.SolicitacaoId
            .Should().Be(solicitacao.Id);

        resultado.Value.Status
            .Should().Be(solicitacao.Status);

        resultado.Value.DataSolicitacao
            .Should().Be(solicitacao.DataSolicitacao);
    }

    [TestMethod]
    public async Task DeveFalharQuandoNaoEncontrarSolicitacao()
    {
        var cursoId = Guid.NewGuid();

        var repositorio = new Mock<IRepositorioSolitacaoCertificados>();

        repositorio
            .Setup(x => x.BuscarPorCursoIdAsync(
                cursoId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((SolicitacaoCertificados?)null);

        var handler = new ConsultarStatusCertificadosQueryHandler(
            repositorio.Object
        );

        var resultado = await handler.Handle(
            new ConsultarStatusCertificadoQuery(cursoId),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();
    }
}