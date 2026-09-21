using System.IO.Compression;

namespace GeradorCertificados.Aplicacao.Modulos.GerarCertificadoZip;

public sealed class GeradorDeZipCertificados
{
    public byte[] Gerar(IEnumerable<string> caminhosArquivos)
    {
        var stream = new MemoryStream(); // uma parte da memoria que podemos manipular para virar um arquivo

        using (var arquivoZip = new ZipArchive(
          stream,
          ZipArchiveMode.Create,
          leaveOpen: true))
        {
            foreach (var caminhoArquivo in caminhosArquivos)
            {
                if (!File.Exists(caminhoArquivo))
                    continue;

                var nomeArquivo = Path.GetFileName(caminhoArquivo);

                arquivoZip.CreateEntryFromFile(
                    caminhoArquivo,
                    nomeArquivo
                );

            }
        }
        return stream.ToArray();
    }
}