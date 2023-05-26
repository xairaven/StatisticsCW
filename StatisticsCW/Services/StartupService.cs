using System;
using System.Windows;
using QuestPDF.Infrastructure;
using StatisticsCW.Data;
using StatisticsCW.Models;

namespace StatisticsCW.Services;

public static class StartupService
{
    public static SettingsContext ConfigureSettings(Window main)
    {
        SettingsContext context = null!;
        
        try
        {
            context = new SettingsContext();
        }
        catch (Exception e)
        {
            MessageBoxCustom.Error(e.Message);
            main.Close();
        }

        return context;
    }

    public static void ConfigureLicenses()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }
}