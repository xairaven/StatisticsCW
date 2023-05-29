using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;

namespace StatisticsCW.View;

public partial class LoadingWindow : Window
{
    private byte[] _pdf;
    private DispatcherTimer _timer;
    
    public LoadingWindow()
    {
        InitializeComponent();

        _pdf = Array.Empty<byte>();

        _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(500),
        };

        _timer.Tick += AnimateTitle!;
        _timer.Start();
    }
    
    private void AnimateTitle(object sender, EventArgs e)
    {
        var step = this.Title.Length - "Creating PDF".Length;

        this.Title = step switch
        {
            1 => "Creating PDF..",
            2 => "Creating PDF...",
            3 => "Creating PDF.",
            _ => "Creating PDF."
        };
    }

    public void UpdateProgress(int progress)
    {
        ProgressBar.Value = progress;

        if (progress == 100)
        {
            _timer.Stop();
            Title = "Creating PDF: DONE";
        }
    }

    public void GetPDF(byte[] bytes)
    {
        _pdf = bytes;
    }

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        var filter = "PDF files (*.pdf)|*.pdf;|All files (*.*)|*.*";

        var saveDialog = new SaveFileDialog()
        {
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
            Filter = filter
        };

        if (saveDialog.ShowDialog() != true)
        {
            return;
        }

        var fileInfo = new FileInfo(saveDialog.FileName);

        File.WriteAllBytes(fileInfo.FullName, _pdf);
    }

    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        _pdf = Array.Empty<byte>();

        Close();
    }
}