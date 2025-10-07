using System;

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using System.IO;

using Preventech.Core.Models;

namespace Preventech.Core.Services;

public class RelatorioGeneratorService
{
    private readonly HttpClient _httpClient;

    public RelatorioGeneratorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public byte[] GerarRelatorioPDF (OrdemServico os)
    {
       using (var stream = new MemoryStream())
        {
            PdfWriter writer = new PdfWriter(stream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            
            document.Add(new Paragraph("RELATÓRIO DE ORDEM DE SERVIÇO")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(16)
                .SetBold()
                .SetMarginBottom(15));
                
            document.Add(new Paragraph($"Título: {os.Titulo}")
                .SetFontSize(20));
            document.Add(new Paragraph($"ID da OS: {os.Id}")
                .SetFontSize(12));
            document.Add(new Paragraph($"Status: {os.Status}")
                .SetFontSize(12)
                .SetFontColor(os.Status == StatusOS.Concluida ? ColorConstants.GREEN : ColorConstants.ORANGE)
                .SetMarginBottom(10));   

            document.Add(new Paragraph($"Equipamento: " + (os.Equipamento != null ? os.Equipamento.Nome : "N/A"))
                .SetFontSize(12)
                .SetFontColor(os.Status == StatusOS.Concluida ? ColorConstants.GREEN : ColorConstants.ORANGE)
                .SetMarginBottom(10));   
                
            document.Add(new Paragraph("Descrição:")
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetPadding(5)
                .SetBold());

            document.Add(new Paragraph($"Descição fornecida: {os.Descricao}")
                .SetMarginBottom(20));

            document.Add(new Paragraph($"Observações: {os.Observacoes}")
                .SetMarginBottom(20));

            document.Add(new Paragraph($"Data de abertura: {os.Abertura}")
                .SetMarginBottom(20));

            document.Add(new Paragraph($"Requisitante: " + (os.Requisitante != null ?
                os.Requisitante.Nome : "N/A"))
                    .SetMarginBottom(20));

            document.Add(new Paragraph("Ações:")
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                .SetPadding(5)
                .SetBold());

            if (!string.IsNullOrWhiteSpace(os.DescricaoSolucao))
            {
                document.Add(new Paragraph(os.DescricaoSolucao));
            }
            else
            {
                document.Add(new Paragraph("Nenhuma descrição de solução fornecida.")
                    .SetFontColor(ColorConstants.RED));
            }
            
            document.Add(new Paragraph($"Técnico Responsável: {os.TecnicoResponsavel?.Nome ?? "Não Atribuído"}"));
            document.Add(new Paragraph($"Data de Conclusão: {os.DataConclusão?.ToShortDateString() ?? "N/A"}"));
            
            document.Add(new Paragraph("Powered by Preventech"))
                .SetFontSize(20)
                .SetFontColor(ColorConstants.CYAN);   

            document.Close();
            
            return stream.ToArray();
        }
    }
}
