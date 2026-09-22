using FluentAssertions;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado.Mensageria;
using GeradorCertificados.Dominio.Modulos.Cursos;
using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;
using MassTransit;
using Moq;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados.Servico;

[TestClass]
public class SolicitarCertificadosCommandHandlerTests
{
    [TestMethod]
    public async Task DeveCriarSolicitacaoComSucesso()
    {
        var curso = new Curso(
            "C# Full Stack",
            40,
            DateTime.UtcNow
        );

        var repositorioCurso = new Mock<IRepositorioCurso>();
        var repositorioSolicitacoes = new Mock<IRepositorioSolitacaoCertificados>();
        var publishEndpoint = new Mock<IPublishEndpoint>();

        repositorioCurso
            .Setup(x => x.SelecionarPorIdAsync(
                curso.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(curso);

        repositorioSolicitacoes
            .Setup(x => x.BuscarPorCursoIdAsync(
                curso.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((SolicitacaoCertificados?)null);

        var handler = new SolicitarCertificadosCommandHandler(
            repositorioCurso.Object,
            repositorioSolicitacoes.Object,
            publishEndpoint.Object
        );

        var resultado = await handler.Handle(
            new SolicitarCertificadosCommand(
                curso.Id,
                ["Kauan", "João"]
            ),
            CancellationToken.None
        );

        resultado.IsSuccess.Should().BeTrue();
        resultado.Value.Should().NotBeEmpty();

        repositorioSolicitacoes.Verify(
            x => x.CadastrarAsync(
                It.Is<SolicitacaoCertificados>(s =>
                    s.CursoId == curso.Id &&
                    s.Certificados.Count == 2
                ),
                It.IsAny<CancellationToken>()),
            Times.Once
        );

        publishEndpoint.Verify(
            x => x.Publish(
                It.Is<GerarCertificadosMessage>(m =>
                    m.SolicitacaoId == resultado.Value
                ),
                It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [TestMethod]
    public async Task DeveFalharQuandoCursoNaoExistir()
    {
        var cursoId = Guid.NewGuid();

        var repositorioCurso = new Mock<IRepositorioCurso>();
        var repositorioSolicitacoes = new Mock<IRepositorioSolitacaoCertificados>();
        var publishEndpoint = new Mock<IPublishEndpoint>();

        repositorioCurso
            .Setup(x => x.SelecionarPorIdAsync(
                cursoId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Curso?)null);

        var handler = new SolicitarCertificadosCommandHandler(
            repositorioCurso.Object,
            repositorioSolicitacoes.Object,
            publishEndpoint.Object
        );

        var resultado = await handler.Handle(
            new SolicitarCertificadosCommand(
                cursoId,
                ["Kauan"]
            ),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();

        repositorioSolicitacoes.Verify(
            x => x.CadastrarAsync(
                It.IsAny<SolicitacaoCertificados>(),
                It.IsAny<CancellationToken>()),
            Times.Never
        );

        publishEndpoint.Verify(
            x => x.Publish(
                It.IsAny<GerarCertificadosMessage>(),
                It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [TestMethod]
    public async Task DeveFalharQuandoNaoHouverAlunos()
    {
        var curso = new Curso(
            "C# Full Stack",
            40,
            DateTime.UtcNow
        );

        var repositorioCurso = new Mock<IRepositorioCurso>();
        var repositorioSolicitacoes = new Mock<IRepositorioSolitacaoCertificados>();
        var publishEndpoint = new Mock<IPublishEndpoint>();

        repositorioCurso
            .Setup(x => x.SelecionarPorIdAsync(
                curso.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(curso);

        var handler = new SolicitarCertificadosCommandHandler(
            repositorioCurso.Object,
            repositorioSolicitacoes.Object,
            publishEndpoint.Object
        );

        var resultado = await handler.Handle(
            new SolicitarCertificadosCommand(
                curso.Id,
                []
            ),
            CancellationToken.None
        );

        resultado.IsFailed.Should().BeTrue();

        repositorioSolicitacoes.Verify(
            x => x.CadastrarAsync(
                It.IsAny<SolicitacaoCertificados>(),
                It.IsAny<CancellationToken>()),
            Times.Never
        );
    }
}