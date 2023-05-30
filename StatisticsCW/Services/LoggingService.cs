using System;
using StatisticsCW.View;

namespace StatisticsCW.Services;

public class LoggingService
{
    private LoadingWindow _window;
    
    public LoggingService(LoadingWindow window)
    {
        _window = window;
    }

    public void LogStatus(int progress, string text, TimeSpan ts)
    {
        _window.Dispatcher.Invoke((Action)(() =>
        {
            _window.UpdateProgress(progress);
            _window.LogBox.Text += Text(text, ts);
        }));
    }


    private string Text(string str, TimeSpan ts)
    {
        return $"{Time(ts)} {str}\n";
    }
    
    private string Time(TimeSpan ts)
    {
        return string.Format("({0:00}.{1:00} m.)", 
            ts.Minutes, ts.Seconds);
    } 
}