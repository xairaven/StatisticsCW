using System.Collections.Generic;

namespace StatisticsCW.Enum;

public static class PodTitle
{
    private static readonly Dictionary<PodId, string> Map;

    static PodTitle()
    {
        Map = new Dictionary<PodId, string>
        {
            { PodId.Input, "Input" },
            { PodId.Result, "Result" },
            { PodId.DefiniteIntegral, "Definite Integral" },
            { PodId.IndefiniteIntegral, "Indefinite integral" },
            { PodId.AlternateForm, "Alternate forms" },
            { PodId.ExpandedForm, "Expanded form" },
            { PodId.Root, "Roots" },
            { PodId.Plot, "Plot" },
            { PodId.RootPlot, "Root plot" },
            { PodId.NumberLine, "Number line" },
            { PodId.ContourPlot, "Contour plot" },
            { PodId.ThirdDimensionPlot, "3D plot" }
        };
    }

    public static string Get(PodId id)
    {
        return Map[id];
    }
}