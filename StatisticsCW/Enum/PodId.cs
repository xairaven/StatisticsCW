using System.ComponentModel;

namespace StatisticsCW.Enum;

public enum PodId
{
    Input,
    Result,
    DefiniteIntegral,
    IndefiniteIntegral,
    AlternateForm,
    ExpandedForm,
    Root,
    Plot,
    RootPlot,
    NumberLine,
    ContourPlot,
    
    [Description("3DPlot")]
    ThirdDimensionPlot
}