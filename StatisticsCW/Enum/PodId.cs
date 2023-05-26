using System.ComponentModel;

namespace StatisticsCW.Enum;

public enum PodId
{
    Input,
    Result,
    IndefiniteIntegral,
    RootPlot,
    NumberLine,
    ContourPlot,
    
    [Description("3DPlot")]
    ThirdDimensionPlot
}