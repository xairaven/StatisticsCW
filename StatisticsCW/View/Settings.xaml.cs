using System.Collections.Generic;
using System.Windows;
using StatisticsCW.Data;

namespace StatisticsCW.View;

public partial class Settings : Window
{
    private readonly SettingsContext _context;
    
    public Settings(SettingsContext context)
    {
        _context = context;
        
        InitializeComponent();
        InitializeFields();
    }

    private void InitializeFields()
    {
        AppIdField.Text = _context.Settings["AppId"];
    }

    private void SaveSettings(object sender, RoutedEventArgs e)
    {
        _context.Settings["AppId"] = AppIdField.Text;
        
        _context.UpdateJson();
        Close();
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}