using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QuestPDF.Fluent;
using StatisticsCW.Data;
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

    private void Solve_OnClick(object sender, RoutedEventArgs e)
    {
        var loadingWindow = new LoadingWindow();
        
        var a = AField.Text.Trim();
        var b = BField.Text.Trim();
        
        loadingWindow.Show();
        Task.Run(() => Solve(loadingWindow, a, b));
    }

    private void Solve(LoadingWindow loadingWindow, string a, string b)
    {
        var logger = new LoggingService(loadingWindow);
        
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        try
        {
            logger.LogStatus(0, "Loading...", stopWatch.Elapsed);

            logger.LogStatus(5, "Parsing numbers...", stopWatch.Elapsed);

            var numbers = ValidateNumbers(a, b);
            var calculator = new Calculations(_context.Settings["AppId"]);
            
            logger.LogStatus(10, "Solving task...", stopWatch.Elapsed);

            calculator.Solve(numbers.a, numbers.b);
            
            logger.LogStatus(50, "Rendering pictures...", stopWatch.Elapsed);

            var images = calculator.Render();
            
            logger.LogStatus(80, "Generating PDF...", stopWatch.Elapsed);

            var bytes = GeneratePDF(images);

            Dispatcher.Invoke(() =>
            {
                loadingWindow.SaveButton.IsEnabled = true;
                loadingWindow.CloseButton.IsEnabled = true;
                loadingWindow.GetPDF(bytes);
            });

            stopWatch.Stop();
            
            logger.LogStatus(100, "Done!", stopWatch.Elapsed);
            MessageBoxCustom.Sound();
        }
        catch (Exception exception)
        {
            MessageBoxCustom.Error(exception.Message);
            Dispatcher.Invoke(loadingWindow.Close);
        }
    }

    private byte[] GeneratePDF(List<Bitmap> images)
    {
        var document = PDFService.Combine(images, ImageFormat.Png);
        return document.GeneratePdf();
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
            throw new ArgumentException("B have to be greater than 0");
        
        return (a, b);
    }
    
    private void OpenSettings(object sender, RoutedEventArgs e)
    {
        new Settings(_context).Show();
    }
    
    private void Info(object sender, RoutedEventArgs e)
    {
        new AboutWindow().Show();
    }
}