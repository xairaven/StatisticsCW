using System.Collections.Generic;
using System.IO;
using System.Windows;
using StatisticsCW.Serializing;

namespace StatisticsCW.Data;

public class SettingsContext
{
    public Dictionary<string, string> Settings { get; private set; }
    private readonly string _settingsPath;
    
    public SettingsContext()
    {
        var executableFolder = Path.GetDirectoryName(System.Environment.ProcessPath!);
        _settingsPath = Json.GetFilePath(executableFolder!, "settings");

        if (!File.Exists(_settingsPath))
        {
            InitializeSettings();
        }
        
        Settings = Json.CustomDeserialize<Dictionary<string, string>>(_settingsPath);
    }
    
    private void InitializeSettings()
    {
        using (File.Create(_settingsPath)) {}

        var temp = new Dictionary<string, string>()
        {
            {"AppId", ""}
        };
            
        Json.CustomSerialize(_settingsPath, temp);
    }

    public void UpdateJson()
    {
        Json.CustomSerialize(_settingsPath, Settings);
    }
}