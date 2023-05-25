using System.Windows;

namespace StatisticsCW.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void GeneratePDF(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Work In Progress!");
    }

    private void OpenSettings(object sender, RoutedEventArgs e)
    {
        new Settings().Show();
    }
}