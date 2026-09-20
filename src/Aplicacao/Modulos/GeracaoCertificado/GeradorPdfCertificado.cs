using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GeradorCertificados.Aplicacao.Modulos.GeracaoCertificado;

public sealed class GeradorPdfCertificao
{
    public byte[] Gerar(
        string nomeAluno,
        string nomeCurso,
        uint cargaHoraria,
        DateTime dataConclusao
    )
    {
        return CriarDocumento(
                nomeAluno,
                nomeCurso,
                cargaHoraria,
                dataConclusao
            ).GeneratePdf();

    }

    public static IDocument CriarDocumento(
            string nomeAluno,
            string nomeCurso,
            uint cargaHoraria,
            DateTime dataConclusao
    )
    {
        return Document.Create(document =>
        {
            document.Page(pagina =>
            {
                pagina.Size(PageSizes.A4.Landscape());
                pagina.Margin(2, Unit.Centimetre);
                pagina.PageColor(Colors.White);

                pagina.DefaultTextStyle(style =>
                    style.FontSize(14));

                pagina.Content()
                    .AlignCenter()
                    .AlignMiddle()
                    .Column(content =>
                    {
                        content.Spacing(20);

                        content.Item()
                            .Text("CERTIFICADO")
                            .Bold()
                            .FontSize(32)
                            .FontColor(Colors.Purple.Darken2);

                        content.Item()
                            .Text("Certificamos que")
                            .FontSize(18);

                        content.Item()
                            .Text(nomeAluno)
                            .Bold()
                            .FontSize(26)
                            .FontColor(Colors.Purple.Darken2);

                        content.Item()
                            .Text(texto =>
                            {
                                texto.Span("concluiu o curso de ")
                                    .FontSize(18);

                                texto.Span(nomeCurso).Bold()
                                    .FontSize(18);

                                texto.Span($" com carga horária de {cargaHoraria} horas.")
                                    .FontSize(18);
                            });
                       
                        content.Item()
                            .PaddingTop(20)
                            .Text(
                                $"Data de conclusão: {dataConclusao:dd/MM/yyyy}"
                            )
                            .FontSize(14);
                    });

                pagina.Footer()
                    .AlignCenter()
                    .Text("Certificado de conclusão de curso");
            });
        });
    }
}