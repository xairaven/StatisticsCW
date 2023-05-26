using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuestPDF.Fluent;
using StatisticsCW.Data;
using StatisticsCW.Enum;
using StatisticsCW.Models;
using StatisticsCW.Services;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using GenboxImage = Genbox.WolframAlpha.Objects.Image;

namespace StatisticsCW.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly SettingsContext _context;

    public MainWindow()
    {
        _context = StartupService.ConfigureSettings(this);

        InitializeComponent();
    }

    private void SolveTask(object sender, RoutedEventArgs e)
    {
        try
        {
            var numbers = ValidateNumbers(AField.Text.Trim(), BField.Text.Trim());
            var images = Calculations(numbers.a, numbers.b);
            GeneratePDF(images);
        }
        catch (Exception exception)
        {
            MessageBoxCustom.Error(exception.Message);
        }
    }

    private void GeneratePDF(List<Bitmap> images)
    {
        var document = PDFService.Combine(images, ImageFormat.Png);
        document.GeneratePdf("output.pdf");
    }

    private void OpenSettings(object sender, RoutedEventArgs e)
    {
        new Settings(_context).Show();
    }

    private void NumericOnly(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !MyRegex().IsMatch(e.Text);
    }

    [GeneratedRegex(@"^[0-9\-]+$")]
    private static partial Regex MyRegex();

    private void IsButtonEnabled()
    {
        var aIsDefined = !AField.Text.Trim().Equals("");
        var bIsDefined = !BField.Text.Trim().Equals("");

        SolveButton.IsEnabled = aIsDefined && bIsDefined;
    }

    private void AField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        IsButtonEnabled();
    }

    private void BField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        IsButtonEnabled();
    }

    private (int a, int b) ValidateNumbers(string aStr, string bStr)
    {
        var a = int.Parse(aStr);
        var b = int.Parse(bStr);

        if (a >= b)
            throw new ArgumentException("A must be lower than B");

        if (a >= 0)
            throw new ArgumentException("A have to be lower than 0");
        
        if (b <= 0)
            throw new ArgumentException("B have to be lower than 0");
        
        return (a, b);
    }

    private List<Bitmap> Calculations(int a, int b)
    {
        var solver = new SolverService(_context.Settings["AppId"]);
        var images = new List<Bitmap>();

        var latex = @"\text{Input: } A = " + a + @", B = " + b + @" \\";
        images.Add(LaTeXService.RenderToPng(latex));
        
        latex = @"\text{Equation of a straight line: } \frac{x-x_0}{x_1-x_0}=\frac{y-y_0}{y_1-y_0}";
        images.Add(LaTeXService.RenderToPng(latex));

        var query = $"Line equation, points ({a},0), (0,a)";
        images.Add(BitmapService.ByGenboxImage(solver.Image(query)));
        images.Add(BitmapService.ByGenboxImage(solver.Image(query, PodId.Result)));
        var line1 = solver.PlainText(query, PodId.Result);

        query = $"Line equation, points (0, a), ({b}, 0)";
        images.Add(BitmapService.ByGenboxImage(solver.Image(query)));
        images.Add(BitmapService.ByGenboxImage(solver.Image(query, PodId.Result)));
        var line2 = solver.PlainText(query, PodId.Result);

        latex = @"f(x) = \left\{\matrix{0, & x \leq  " + $"{a}" + @" \\ " + $"{LaTeXService.ToLaTeXFormat(line1)}" 
                + @", & " + $"{a}" + @" < x \leq  0 \\ " + $"{LaTeXService.ToLaTeXFormat(line2)}" + @", & 0 < x \leq  " 
                + $"{b}" + @" \\ 0, & x \geq  " + $"{b}" + @"}\right. \\ \\";
        images.Add(LaTeXService.RenderToPng(latex));

        latex = @"\text{(Formula) a:}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        query = "integrate (f(x)dx) from x=-inf to inf = 1";
        images.Add(BitmapService.ByGenboxImage(solver.Image(query)));

        return images;
    }
}