using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using StatisticsCW.Enum;
using StatisticsCW.Services;

namespace StatisticsCW.Solve;

internal class Calculations
{
    private readonly SolverService _solver;

    private int _a;
    private int _b;
    
    private readonly Dictionary<string, string> _results;
    
    public Calculations(string appId)
    {
        _solver = new SolverService(appId);

        // Initializing fields
        _results = new Dictionary<string, string>();
    }
    
    public void Solve(int a, int b)
    {
        _a = a;
        _b = b;

        _results.Add("Line1", "0");
        
        // Line 2 result
        var query = $"Line equation, points ({_a},0), (0,a)";
        var result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results.Add("Line2", result);
        
        // Line 3 result
        query = $"Line equation, points (0, a), ({_b}, 0)";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results.Add("Line3", result);

        _results.Add("Line4", "0");
        
        // FINDING A
        
        // First integral
        query = $"Integrate({_results["Line1"]}) from x=-inf to x={_a}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results.Add("A1Integral", result);
        
        // Second integral
        query = $"Integrate({_results["Line2"]}) from x={_a} to x=0";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results.Add("A2Integral", result);
        
        // Third integral
        query = $"Integrate({_results["Line3"]}) from x=0 to x={_b}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results.Add("A3Integral", result);
        
        // Fourth integral
        query = $"Integrate({_results["Line4"]}) from x={_b} to x=inf";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results.Add("A4Integral", result);
        
        // Sum
        query = $"{_results["A1Integral"]} + {_results["A2Integral"]} " +
                $"+ {_results["A3Integral"]} + {_results["A4Integral"]}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results.Add("ASum", result);

        // A
        query = $"Solve {_results["ASum"]} = 1"; 
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results.Add("A", result);
    }

    public List<Bitmap> Render()
    {
        var images = new List<Bitmap>();

        // All equations in Latex format
        var results 
            = _results.Keys.ToDictionary(key => key, key => LaTeXService.ToLaTeXFormat(_results[key]));

        string latex;
        string query;
        string text;
        
        // Input render
        latex = @"\text{Input: } A = " + _a + @", B = " + _b + @" \\";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Line equations formula
        latex = @"\text{Equation of a straight line: } \frac{x-x_0}{x_1-x_0}=\frac{y-y_0}{y_1-y_0}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Line 2 title and result
        query = $"Line equation, points ({_a},0), (0,a)";
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query)));
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query, PodId.Result)));
        
        // Line 3 title and result
        query = $"Line equation, points (0, a), ({_b}, 0)";
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query)));
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query, PodId.Result)));
        
        // f(x) system (lines)
        latex = @"f(x) = \left\{\matrix{" 
                + results["Line1"] +  @", & x \leq  " + _a + @" \\ " 
                + results["Line2"] + @", & " + _a + @" < x \leq  0 \\ " 
                + results["Line3"] + @", & 0 < x \leq  " + _b + @" \\ " 
                + results["Line4"] + @", & x \geq  " + _b 
                + @"}\right.";
        images.Add(LaTeXService.RenderToPng(latex));

        // A TITLE
        text = "Find a:";
        images.Add(BitmapService.ByText(text, 24, FontStyle.Underline));

        // Formula A title
        latex = @"\text{Formula:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula A
        latex = @"\int_{-\infty}^{\infty}f(x)dx = 1 \Rightarrow";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Formula A extended
        latex = @"\star \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"dx + \int_{" + _a +
                @"}^{0}(" + results["Line2"] + @")dx + \int_{0}^{" + _b + @"}(" + results["Line3"] + @")dx + \int_{" + _b +
                @"}^{\infty}" + results["Line4"] + @"dx = ... = 1? ";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Integrals Title
        latex = @"\text{Each integral separately:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // First integral
        latex = @"\bullet \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"dx = " + $"{results["A1Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Second integral
        latex = @"\bullet \int_{" + _a + @"}^{0}(" + results["Line2"] + @")dx = " + $"{results["A2Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Third integral
        latex = @"\bullet \int_{0}^{" + _b + @"}(" + results["Line3"] + @")dx = " + $"{results["A3Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Fourth integral
        latex = @"\bullet \int_{" + _b + @"}^{\infty}" + results["Line4"] + @"dx = " + $"{results["A4Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Sum Title
        latex = @"\star \text{Sum:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Sum
        latex = $"{results["A1Integral"]} + {results["A2Integral"]} + {results["A3Integral"]} + {results["A4Integral"]}" +
                $" = {results["ASum"]} " + @"\Rightarrow " + results["ASum"] + " = 1";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // A
        latex = $"a = {LaTeXService.ToLaTeXFormat(results["A"])}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // A TITLE
        text = "Find F(x):";
        images.Add(BitmapService.ByText(text, 24, FontStyle.Underline));
        
        // Formula F(x) title
        latex = @"\text{Formula:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula F(x)
        latex = @"F(x) = \int_{-\infty}^{x}f(x)dx";
        images.Add(LaTeXService.RenderToPng(latex));
        
        return images;
    }
}