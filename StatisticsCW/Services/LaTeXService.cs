using System.Collections.Generic;
using System.Drawing;
using System.IO;
using WpfMath;
using WpfMath.Parsers;

namespace StatisticsCW.Services;

public static class LaTeXService
{
    public static Bitmap RenderToPng(string latexCode)
    {
        var parser = WpfTeXFormulaParser.Instance;
        var result = parser.Parse(latexCode);
        
        var pngBytes = result.RenderToPng(20.0, 0.0, 0.0, "Arial");

        using var ms = new MemoryStream(pngBytes);
        var bitmap = new Bitmap(ms);

        return bitmap;
    }

    public static string ToLaTeXFormat(string str)
    {
        // Division
        str = Division(str);
        
        return str;
    }

    private static string Division(string str)
    {
        var separators = new[] { '+', '-', '*', '=' };
        
        var splitByOperators = str.Split(separators);

        var toReplace = new Dictionary<string, string>();
        
        foreach (var operand in splitByOperators)
        {
            if (!operand.Contains('/')) continue;
            var split = operand.Split("/");
            
            split[0] = split[0].Replace('(', ' ').Replace(')', ' ').Trim();
            split[1] = split[1].Replace('(', ' ').Replace(')', ' ').Trim();

            var resultOperand = @"\frac{" + split[0] + @"}{" + split[1] + @"}";
                
            toReplace.Add(operand.Trim(), resultOperand);
        }

        var result = str;
        foreach (var key in toReplace.Keys)
        {
            result = result.Replace(key, toReplace[key]);
        }
        
        return result;
    }
}