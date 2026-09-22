using GeradorCertificados.Aplicacao.Modulos.GerarCertificadoZip;

namespace GeradorDeCertificados.Testes.Unidade.Modulos.Certificados.Servico;

[TestClass]
public class GeradorDeZipCertificadosTests
{
    [TestMethod]
    public void DeveGerarZipComArquivo()
    {
        var caminhoArquivo = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}.pdf"
        );

        try
        {
            File.WriteAllBytes(caminhoArquivo, [1, 2, 3]);

            var gerador = new GeradorDeZipCertificados();

            var resultado = gerador.Gerar([caminhoArquivo]);

            Assert.IsNotNull(resultado);
            Assert.IsNotEmpty(resultado);

            using var stream = new MemoryStream(resultado);
            using var zip = new System.IO.Compression.ZipArchive(
                stream,
                System.IO.Compression.ZipArchiveMode.Read
            );

            Assert.AreEqual(1, zip.Entries.Count);
            Assert.AreEqual(
                Path.GetFileName(caminhoArquivo),
                zip.Entries[0].Name
            );
        }
        finally
        {
            if (File.Exists(caminhoArquivo))
                File.Delete(caminhoArquivo);
        }
    }

    [TestMethod]
    public void DeveIgnorarArquivoInexistente()
    {
        var gerador = new GeradorDeZipCertificados();

        var resultado = gerador.Gerar([
            @"C:\arquivo-que-nao-existe.pdf"
        ]);

        Assert.IsNotNull(resultado);

        using var stream = new MemoryStream(resultado);
        using var zip = new System.IO.Compression.ZipArchive(
            stream,
            System.IO.Compression.ZipArchiveMode.Read
        );

        Assert.AreEqual(0, zip.Entries.Count);
    }
}