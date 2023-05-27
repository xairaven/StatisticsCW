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
using StatisticsCW.Solve;
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

            var calculator = new Calculations(_context.Settings["AppId"]);
            calculator.Solve(numbers.a, numbers.b);
            var images = calculator.Render();
            
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
}