using System;
using System.Windows;
using StatisticsCW.Data;
using StatisticsCW.Models;

namespace StatisticsCW.View;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly SettingsContext _context = null!;
    
    public MainWindow()
    {
        try
        {
            _context = new SettingsContext();
        }
        catch (Exception e)
        {
            MessageBoxCustom.Error(e.Message);
            Close();
        }
        
        InitializeComponent();
    }

    private void GeneratePDF(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Work In Progress!");
    }

    private void OpenSettings(object sender, RoutedEventArgs e)
    {
        new Settings(_context).Show();
    }
}