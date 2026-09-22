using GeradorCertificados.Dominio.Modulos.GeracaoCertificado;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados;

[TestClass]
public class CertificadoTests
{
    [TestMethod]
    public void DeveCriarCertificadoComStatusPendente()
    {
        var solicitacaoId = Guid.NewGuid();
        var dataConclusao = DateTime.UtcNow;

        var certificado = new Certificado(
            solicitacaoId,
            "Kauan",
            "C# Full Stack",
            40,
            dataConclusao
        );

        Assert.AreEqual(solicitacaoId, certificado.SolicitacaoId);
        Assert.AreEqual("Kauan", certificado.NomeAluno);
        Assert.AreEqual("C# Full Stack", certificado.NomeCurso);
        Assert.AreEqual((uint)40, certificado.CargaHoraria);
        Assert.AreEqual(dataConclusao, certificado.DataConclusao);
        Assert.AreEqual(StatusCertificado.Pendente, certificado.Status);
    }

    [TestMethod]
    public void DeveMarcarCertificadoComoGerado()
    {
        var certificado = new Certificado(
            Guid.NewGuid(),
            "Kauan",
            "C# Full Stack",
            40,
            DateTime.UtcNow
        );

        var caminho = @"C:\Certificados\certificado.pdf";

        certificado.MarcarComoGerado(caminho);

        Assert.AreEqual(StatusCertificado.Gerado, certificado.Status);
        Assert.AreEqual(caminho, certificado.CaminhoArquivo);
        Assert.IsNotNull(certificado.DataGeracao);
    }

    [TestMethod]
    public void DeveMarcarCertificadoComoFalha()
    {
        var certificado = new Certificado(
            Guid.NewGuid(),
            "Kauan",
            "C# Full Stack",
            40,
            DateTime.UtcNow
        );

        certificado.MarcarComoFalha();

        Assert.AreEqual(StatusCertificado.Falha, certificado.Status);
    }
}