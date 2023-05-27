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

    private string[] _lines;
    private List<string> _integralsResults;

    private string _density;
    
    public Calculations(string appId)
    {
        _solver = new SolverService(appId);

        // Initializing fields
        _lines = new string[4];
        _integralsResults = new List<string>();

        _density = string.Empty;
    }
    
    public void Solve(int a, int b)
    {
        _a = a;
        _b = b;
        
        // Line 2 result
        var query = $"Line equation, points ({_a},0), (0,a)";
        var line2 = _solver.PlainText(query, PodId.Result);

        // Line 3 result
        query = $"Line equation, points (0, a), ({_b}, 0)";
        var line3 = _solver.PlainText(query, PodId.Result);

        // Lines to array
        _lines = new[] { "0", _solver.OperandFromResult(line2), _solver.OperandFromResult(line3), "0"};
        
        // FINDING A
        
        // First integral
        query = $"Integrate({_lines[0]}) from x=-inf to x={_a}";
        _integralsResults.Add(_solver.OperandFromResult(_solver.PlainText(query, PodId.Result)));
        
        // Second integral
        query = $"Integrate({_lines[1]}) from x={_a} to x=0";
        _integralsResults.Add(_solver.OperandFromResult(_solver.PlainText(query, PodId.Result)));
        
        // Third integral
        query = $"Integrate({_lines[2]}) from x=0 to x={_b}";
        _integralsResults.Add(_solver.OperandFromResult(_solver.PlainText(query, PodId.Result)));
        
        // Fourth integral
        query = $"Integrate({_lines[3]}) from x={_b} to x=inf";
        _integralsResults.Add(_solver.OperandFromResult(_solver.PlainText(query, PodId.Result)));
        
        // Sum
        query = $"{_integralsResults[0]} + {_integralsResults[1]} + {_integralsResults[2]} + {_integralsResults[3]}";
        _integralsResults.Add(_solver.OperandFromResult(_solver.PlainText(query, PodId.Result)));

        // A
        query = $"Solve {_integralsResults[4]} = 1"; 
        _density = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
    }

    public List<Bitmap> Render()
    {
        var images = new List<Bitmap>();

        // Line equations in Latex format
        var lines = _lines.Select(LaTeXService.ToLaTeXFormat).ToArray();
        var integralsResults = _integralsResults.Select(LaTeXService.ToLaTeXFormat).ToArray();
        
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
                + lines[0] +  @", & x \leq  " + _a + @" \\ " 
                + lines[1] + @", & " + _a + @" < x \leq  0 \\ " 
                + lines[2] + @", & 0 < x \leq  " + _b + @" \\ " 
                + lines[3] + @", & x \geq  " + _b 
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
        latex = @"\star \int_{-\infty}^{" + _a + @"}" + lines[0] + @"dx + \int_{" + _a +
                @"}^{0}(" + lines[1] + @")dx + \int_{0}^{" + _b + @"}(" + lines[2] + @")dx + \int_{" + _b +
                @"}^{\infty}" + lines[3] + @"dx = ... = 1? ";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Integrals Title
        latex = @"\text{Each integral separately:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // First integral
        latex = @"\bullet \int_{-\infty}^{" + _a + @"}" + lines[0] + @"dx = " + $"{integralsResults[0]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Second integral
        latex = @"\bullet \int_{" + _a + @"}^{0}(" + lines[1] + @")dx = " + $"{integralsResults[1]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Third integral
        latex = @"\bullet \int_{0}^{" + _b + @"}(" + lines[2] + @")dx = " + $"{integralsResults[2]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Fourth integral
        latex = @"\bullet \int_{" + _b + @"}^{\infty}" + lines[3] + @"dx = " + $"{integralsResults[3]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Sum Title
        latex = @"\star \text{Sum:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Sum
        latex = $"{integralsResults[0]} + {integralsResults[1]} + {integralsResults[2]} + {integralsResults[3]}" +
                $" = {integralsResults[4]} " + @"\Rightarrow " + integralsResults[4] + " = 1";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // A
        latex = $"a = {LaTeXService.ToLaTeXFormat(_density)}";
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