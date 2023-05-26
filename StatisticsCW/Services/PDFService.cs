using System.Collections.Generic;
using System.Drawing;
using System.IO;
//using Aspose.Words;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ImageFormat = System.Drawing.Imaging.ImageFormat;

namespace StatisticsCW.Services;

public static class PDFService
{
    public static Document Combine(List<Bitmap> images, ImageFormat format)
    {
        const string header = "Theory of Probability and Mathematical Statistics.\nControl Work.";
        
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.Margin(1, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Item().AlignCenter().Text(header).SemiBold().FontSize(16);
                        x.Spacing(10);

                        foreach (var image in images)
                        {
                            using var stream = new MemoryStream();
                            image.Save(stream, format);
            
                            var bytes = stream.ToArray();
                            
                            x.Item().Width(image.Width).Height(image.Height).Image(bytes);
                        }
                    });

                page.Footer()
                    .AlignRight()
                    .Text(x =>
                    {
                        x.CurrentPageNumber();
                    });
            });
        });

        return document;
    }

    /*public static Document Combine(List<Bitmap> images, ImageFormat format)
    {
        var document = new Document();
        var builder = new DocumentBuilder(document);

        foreach (var image in images)
        {
            using var stream = new MemoryStream();
            image.Save(stream, format);
            
            var bytes = stream.ToArray();

            builder.InsertImage(bytes);
            
            // Insert a paragraph break to avoid overlapping images.
            builder.Writeln();
        }

        return document;
    }*/
}