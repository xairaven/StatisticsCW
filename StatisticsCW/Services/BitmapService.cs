using System.Drawing;
using System.Net.Http;
using GenboxImage = Genbox.WolframAlpha.Objects.Image;

namespace StatisticsCW.Services;

public static class BitmapService
{
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
}