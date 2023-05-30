using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        var indexes = Enumerable.Range(0, str.Length).Where(i => str[i] == '/').ToList();
        
        if (indexes.Count == 0) return str;

        string result = str;
        
        for (int i = indexes.Count - 1; i >= 0; i--)
        {
            int j, k;
            int bracketCounter = 0;
            
            // Left
            for (j = indexes[i] - 1; j >= 0; j--)
            {
                if (str[j].Equals(' ') && bracketCounter == 0)
                {
                    j++;
                    break;
                }

                if (str[j].Equals('(') && bracketCounter == 1)
                {
                    break;
                }

                if (str[j].Equals('(')) bracketCounter--;

                if (str[j].Equals(')')) bracketCounter++;

                if (j == 0) break;
            }

            bracketCounter = 0;
            for (k = indexes[i] + 1; k < str.Length; k++)
            {
                if (str[k].Equals(' ') && bracketCounter == 0)
                {
                    k--;
                    break;
                }

                if (str[k].Equals(')') && bracketCounter == 1)
                {
                    break;
                }

                if (str[k].Equals(')')) bracketCounter--;
                
                if (str[k].Equals('(')) bracketCounter++;
                
                if (k == str.Length - 1) break;
            }

            var sub = str.Substring(j, k + 1 - j);
            
            var split = sub.Split("/");

            split[0] = split[0].Replace('(', ' ').Replace(')', ' ').Trim();
            split[1] = split[1].Replace('(', ' ').Replace(')', ' ').Trim();

            var resultOperand = @"\frac{" + split[0] + @"}{" + split[1] + @"}";

            result = result.Remove(j, sub.Trim().Length).Insert(j, resultOperand.Trim());
            //result = result.Replace(sub.Trim(), resultOperand.Trim());
        }
        
        return result;
    }
}