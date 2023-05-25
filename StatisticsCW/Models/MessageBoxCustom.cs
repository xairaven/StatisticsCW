using System.Windows;

namespace StatisticsCW.Models;

public static class MessageBoxCustom
{
    public static void Error(string message)
    {
        MessageBox.Show(messageBoxText: message,
            caption: "Error!",
            button: MessageBoxButton.OK,
            icon: MessageBoxImage.Error);
    }
}