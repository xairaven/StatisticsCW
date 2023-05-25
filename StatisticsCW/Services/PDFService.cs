using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Aspose.Words;

namespace StatisticsCW.Services;

public static class PDFService
{
    public static Document Combine(List<Bitmap> images, ImageFormat format)
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
    }
}