

using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados;

[TestClass]
public class SolicitacaoCertificadosTests
{
    [TestMethod]
    public void DeveCriarSolicitacaoComoPendente()
    {
        var cursoId = Guid.NewGuid();

        var solicitacao = new SolicitacaoCertificados(
            cursoId,
            "C# Full Stack",
            40,
            DateTime.UtcNow,
            ["Kauan", "João"]
        );

        Assert.AreEqual(cursoId, solicitacao.CursoId);
        Assert.AreEqual(
            StatusGeracaoCertificados.Pendente,
            solicitacao.Status
        );

        Assert.AreEqual(2, solicitacao.Certificados.Count);
    }

    [TestMethod]
    public void DeveCriarUmCertificadoParaCadaAluno()
    {
        var solicitacao = new SolicitacaoCertificados(
            Guid.NewGuid(),
            "C# Full Stack",
            40,
            DateTime.UtcNow,
            ["Kauan", "João", "Arthur"]
        );

        Assert.AreEqual(3, solicitacao.Certificados.Count);

        CollectionAssert.AreEqual(
            new[] { "Kauan", "João", "Arthur" },
            solicitacao.Certificados
                .Select(x => x.NomeAluno)
                .ToArray()
        );
    }

    [TestMethod]
    public void DeveIniciarGeracao()
    {
        var solicitacao = CriarSolicitacao();

        solicitacao.IniciarGeracao();

        Assert.AreEqual(
            StatusGeracaoCertificados.GerandoCertificados,
            solicitacao.Status
        );
    }

    [TestMethod]
    public void DeveIniciarGeracaoDoZip()
    {
        var solicitacao = CriarSolicitacao();

        solicitacao.IniciarGeracaoZip();

        Assert.AreEqual(
            StatusGeracaoCertificados.GerandoZip,
            solicitacao.Status
        );
    }

    [TestMethod]
    public void DeveMarcarSolicitacaoComoConcluida()
    {
        var solicitacao = CriarSolicitacao();
        var caminhoZip = @"C:\Certificados\certificados.zip";

        solicitacao.MarcarComoConcluida(caminhoZip);

        Assert.AreEqual(
            StatusGeracaoCertificados.Concluido,
            solicitacao.Status
        );

        Assert.AreEqual(caminhoZip, solicitacao.CaminhoZip);
        Assert.IsNotNull(solicitacao.DataConclusao);
    }

    [TestMethod]
    public void DeveMarcarSolicitacaoComoFalha()
    {
        var solicitacao = CriarSolicitacao();

        solicitacao.MarcarComoFalha();

        Assert.AreEqual(
            StatusGeracaoCertificados.Falha,
            solicitacao.Status
        );

        Assert.IsNotNull(solicitacao.DataConclusao);
    }

    [TestMethod]
    public void DeveIndicarQueSolicitacaoEstaProcessando()
    {
        var solicitacao = CriarSolicitacao();

        Assert.IsTrue(solicitacao.EstaProcessando());

        solicitacao.IniciarGeracao();

        Assert.IsTrue(solicitacao.EstaProcessando());

        solicitacao.IniciarGeracaoZip();

        Assert.IsTrue(solicitacao.EstaProcessando());
    }

    [TestMethod]
    public void NaoDeveIndicarProcessamentoQuandoConcluida()
    {
        var solicitacao = CriarSolicitacao();

        solicitacao.MarcarComoConcluida("certificados.zip");

        Assert.IsFalse(solicitacao.EstaProcessando());
    }

    [TestMethod]
    public void NaoDeveIndicarProcessamentoQuandoFalhar()
    {
        var solicitacao = CriarSolicitacao();

        solicitacao.MarcarComoFalha();

        Assert.IsFalse(solicitacao.EstaProcessando());
    }

    private static SolicitacaoCertificados CriarSolicitacao()
    {
        return new SolicitacaoCertificados(
            Guid.NewGuid(),
            "C# Full Stack",
            40,
            DateTime.UtcNow,
            ["Kauan"]
        );
    }
}
