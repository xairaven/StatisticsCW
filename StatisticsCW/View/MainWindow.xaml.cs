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
        (int a, int b) numbers;
        try
        {
            numbers = ValidateNumbers(AField.Text.Trim(), BField.Text.Trim());
            var genboxImages = Calculations(numbers.a, numbers.b);
            GeneratePDF(genboxImages);
        }
        catch (Exception exception)
        {
            MessageBoxCustom.Error(exception.Message);
        }
    }

    private void GeneratePDF(List<GenboxImage> genboxImages)
    {
        var images = new List<Bitmap>();
        foreach (var image in genboxImages)
        {
            images.Add(BitmapService.ByGenboxImage(image));
        }

        var document = PDFService.Combine(images, ImageFormat.Gif);
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

    private List<GenboxImage> Calculations(int a, int b)
    {
        var solver = new SolverService(_context.Settings["AppId"]);
        var genboxImages = new List<GenboxImage>();

        var query = "Lines Equation: (x-x_0)/(x_1-x_0)=(y-y_0)/(y_1-y_0)";
        genboxImages.Add(solver.Image(query));
        
        query = $"Line equation, points ({a},0), (0,a)";
        genboxImages.Add(solver.Image(query));
        genboxImages.Add(solver.Image(query, PodId.Result));
        MessageBox.Show(solver.PlainText(query, PodId.Result));
        
        query = $"Line equation, points (0, a), ({b}, 0)";
        genboxImages.Add(solver.Image(query));
        genboxImages.Add(solver.Image(query, PodId.Result));
        MessageBox.Show(solver.PlainText(query, PodId.Result));

        query = "integrate (f(x)dx) from x=-inf to inf = 1";
        genboxImages.Add(solver.Image(query));
        
        return genboxImages;
    }
}