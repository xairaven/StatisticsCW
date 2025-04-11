using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net.Http;
using System.Threading.Tasks;
using GenboxImage = Genbox.WolframAlpha.Objects.Image;

namespace StatisticsCW.Services;

public static class BitmapService
{
    [Obsolete("Use ByGenboxImageAsync instead")]
    public static Bitmap ByGenboxImage(GenboxImage image)
    {
        using var client = new HttpClient();
        
        var url = image.Src;
        var response = client.GetAsync(url).Result;

        response.EnsureSuccessStatusCode();

        var stream = response.Content.ReadAsStream();
        
        var bitmap = new Bitmap(stream);

        return bitmap;
    }
    
    public static async Task<Bitmap> ByGenboxImageAsync(GenboxImage image)
    {
        using var client = new HttpClient();
    
        var url = image.Src;
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
    
        var bitmap = new Bitmap(stream);

        return bitmap;
    }

    public static Bitmap ByText(string text, int size, FontStyle fontStyle)
    {
        Bitmap bitmap = new(1, 1);
        Graphics graphics = Graphics.FromImage(bitmap);

        Font font = new("Arial", size, fontStyle, GraphicsUnit.Pixel);

        var width = (int)graphics.MeasureString(text, font).Width;
        var height = (int)graphics.MeasureString(text, font).Height;

        bitmap = new Bitmap(bitmap, new Size(width, height));
        graphics = Graphics.FromImage(bitmap);

        graphics.Clear(Color.White);
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

        graphics.DrawString(text, font, new SolidBrush(Color.Black), 0, 0);

        graphics.Flush();
        graphics.Dispose();

        return bitmap;
    }
}