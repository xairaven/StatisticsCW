using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using QuestPDF.Infrastructure;
using StatisticsCW.Models;
using StatisticsCW.Services;

namespace StatisticsCW
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            SetupExceptionHandling();
            ConfigureLicenses();
        }

        private void SetupExceptionHandling()
        {
            DispatcherUnhandledException += (s, e) =>
            {
                MessageBoxCustom.Error(e.Exception.Message);
                MessageBoxCustom.Error(e.Exception.StackTrace!);
                e.Handled = true;
            };
        }
        
        private static void ConfigureLicenses()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }
    }
}