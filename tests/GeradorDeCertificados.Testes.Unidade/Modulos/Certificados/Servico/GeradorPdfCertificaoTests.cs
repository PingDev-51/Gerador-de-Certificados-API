
using GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;
using QuestPDF.Infrastructure;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados.Servico;

[TestClass]
public class GeradorPdfCertificaoTests
{
    [TestMethod]
    public void DeveGerarPdf()
    {
        QuestPDF.Settings.License = LicenseType.Evaluation;

        var gerador = new GeradorPdfCertificao();

        var resultado = gerador.Gerar(
            "Kauan",
            "C# Full Stack",
            40,
            DateTime.UtcNow
        );

        Assert.IsNotNull(resultado);
        Assert.IsNotEmpty(resultado);
    }
}