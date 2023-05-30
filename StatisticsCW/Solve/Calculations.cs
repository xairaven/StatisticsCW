using System;
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

        _results["Line1"] =  "0";

        // Line 2 result
        var query = $"Line equation, points ({_a},0), (0,a)";
        var result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Line2"] = result;

        // Line 3 result
        query = $"Line equation, points (0, a), ({_b}, 0)";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Line3"] = result;

        _results["Line4"] =  "0";
        
        // FINDING A
        
        // First integral
        query = $"Integrate({_results["Line1"]}) from x=-inf to x={_a}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["A1Integral"] = result;

        // Second integral
        query = $"Integrate({_results["Line2"]}) from x={_a} to x=0";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["A2Integral"] = result;
        
        // Third integral
        query = $"Integrate({_results["Line3"]}) from x=0 to x={_b}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["A3Integral"] = result;
        
        // Fourth integral
        query = $"Integrate({_results["Line4"]}) from x={_b} to x=inf";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["A4Integral"] = result;
        
        // Sum
        query = $"{_results["A1Integral"]} + {_results["A2Integral"]} " +
                $"+ {_results["A3Integral"]} + {_results["A4Integral"]}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["ASum"] = result;

        // A
        query = $"Solve {_results["ASum"]} = 1"; 
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["A"] = result;
        
        
        
        // F(X)
        // First interval
        query = $"Integrate({_results["Line1"]}) from x=-inf to x=x";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx1Integral"] = result;
        _results["Fx1Sum"] = result;
        
        
        
        
        // Second interval
        // x integral
        query = $"Integrate({_results["Line2"]}) from x={a} to x=x";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx2Integral"] = result;
        
        try
        {
            query = $"{_results["Fx2Integral"]}";
            result = _solver.OperandFromResult(_solver.PlainText(query, PodId.ExpandedForm));
            _results["Fx2Integral"] = result;
        }
        catch (Exception) {
            // ignored
        }
        
        // Raw sum
        query = $"{_results["A1Integral"]} + {_results["Fx2Integral"]}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx2RawSum"] = result;
        
        // Sum
        query = $"{_results["Fx2RawSum"]}, a = {_results["A"]}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx2Sum"] = result;
        
        try
        {
            query = $"{_results["Fx2Sum"]}";
            result = _solver.OperandFromResult(_solver.PlainText(query, PodId.ExpandedForm));
            _results["Fx2Sum"] = result;
        }
        catch (Exception) {
            // ignored
        }
        
        
        
        // Third interval
        // x integral
        query = $"Integrate({_results["Line3"]}) from x=0 to x=x";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx3Integral"] = result;
        
        try
        {
            query = $"{_results["Fx3Integral"]}";
            result = _solver.OperandFromResult(_solver.PlainText(query, PodId.ExpandedForm));
            _results["Fx3Integral"] = result;
        }
        catch (Exception) {
            // ignored
        }
        
        // Raw sum
        query = $"{_results["A1Integral"]} + {_results["A2Integral"]} + {_results["Fx3Integral"]}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx3RawSum"] = result;
        
        // Sum
        query = $"{_results["Fx3RawSum"]}, a = {_results["A"]}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx3Sum"] = result;
        
        try
        {
            query = $"{_results["Fx3Sum"]}";
            result = _solver.OperandFromResult(_solver.PlainText(query, PodId.ExpandedForm));
            _results["Fx3Sum"] = result;
        }
        catch (Exception) {
            // ignored
        }
        
        
        
        
        
        // Fourth interval
        // x integral
        query = $"Integrate({_results["Line4"]}) from x={_b} to x=x";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx4Integral"] = result;
        
        try
        {
            query = $"{_results["Fx4Integral"]}";
            result = _solver.OperandFromResult(_solver.PlainText(query, PodId.ExpandedForm));
            _results["Fx4Integral"] = result;
        }
        catch (Exception) {
            // ignored
        }
        
        // Raw sum
        query = $"{_results["A1Integral"]} + {_results["A2Integral"]} " +
                $"+ {_results["A3Integral"]} + {_results["Fx4Integral"]}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx4RawSum"] = result;
        
        // Sum
        query = $"{_results["Fx4RawSum"]}, a = {_results["A"]}";
        result = _solver.OperandFromResult(_solver.PlainText(query, PodId.Result));
        _results["Fx4Sum"] = result;
        
        try
        {
            query = $"{_results["Fx4Sum"]}";
            result = _solver.OperandFromResult(_solver.PlainText(query, PodId.ExpandedForm));
            _results["Fx4Sum"] = result;
        }
        catch (Exception) {
            // ignored
        }
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
        
        
        
        
        
        // F(X) TITLE
        text = "Find F(x):";
        images.Add(BitmapService.ByText(text, 24, FontStyle.Underline));
        
        // Formula F(x) title
        latex = @"\text{Formula:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula F(x)
        latex = @"F(x) = \int_{-\infty}^{x}f(x)dx";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Number Line Title
        latex = @"\text{Number line on 4 intervals:}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // f(x) system (lines)
        latex = @"f(x) = \left\{\matrix{" 
                + results["Line1"] +  @", & x \leq  " + _a + @" \\ " 
                + results["Line2"] + @", & " + _a + @" < x \leq  0 \\ " 
                + results["Line3"] + @", & 0 < x \leq  " + _b + @" \\ " 
                + results["Line4"] + @", & x >  " + _b 
                + @"}\right.";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Number Line
        query = $"Line {_a}, 0, {_b}";
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query, PodId.NumberLine)));

        
        
        
        // First Interval
        latex = @"1) -\infty; " + _a;
        images.Add(LaTeXService.RenderToPng(latex));
        latex = @"\bullet \int_{-\infty}^{x}" + results["Line1"] + @"dx = " + $"{results["Fx1Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        
        
        
        
        // Second Interval
        latex = $"2) {_a}; 0";
        images.Add(LaTeXService.RenderToPng(latex));
        latex = @"\star \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"dx + \int_{" + _a +
                @"}^{x}(" + results["Line2"] + @")dx = ... \Rightarrow";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Second 1 integral
        latex = @"\bullet \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"dx = " + $"{results["A1Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Second 2 integral
        latex = @"\bullet \int_{" + _a + @"}^{x}(" + results["Line2"] + @")dx = " + $"{results["Fx2Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Second sum
        latex = @"\star " + $" {results["A1Integral"]} + {results["Fx2Integral"]}"  + $" = {results["Fx2RawSum"]}" +
                @" \Rightarrow " + $"a = {results["A"]} " + @" \Rightarrow " + $"{results["Fx2Sum"]}" ;
        images.Add(LaTeXService.RenderToPng(latex));
        
        
        
        
        
        // Third Interval
        latex = $"3) 0; {_b}";
        images.Add(LaTeXService.RenderToPng(latex));
        latex = @"\star \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"dx + \int_{" + _a +
                @"}^{0}(" + results["Line2"] + @")dx + \int_{0}^{x}(" + results["Line3"] + @")dx = ... \Rightarrow";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Third 1 integral
        latex = @"\bullet \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"dx = " + $"{results["A1Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Third 2 integral
        latex = @"\bullet \int_{" + _a + @"}^{0}(" + results["Line2"] + @")dx = " + $"{results["A2Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Third 3 integral
        latex = @"\bullet \int_{0}^{x}(" + results["Line3"] + @")dx = " + $"{results["Fx3Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Third sum
        latex = @"\star " + $" {results["A1Integral"]} + {results["A2Integral"]} + {results["Fx3Integral"]}" +
                $" = {results["Fx3RawSum"]}" +
                @" \Rightarrow " + $"a = {results["A"]} " + @" \Rightarrow " + $"{results["Fx3Sum"]}" ;
        images.Add(LaTeXService.RenderToPng(latex));
        
        
        
        
        // Fourth Interval
        latex = $"4) {_b}; " + @"\infty";
        images.Add(LaTeXService.RenderToPng(latex));
        latex = @"\star \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"dx + \int_{" + _a +
                @"}^{0}(" + results["Line2"] + @")dx + \int_{0}^{" + _b + "}(" + results["Line3"] + @")dx + \int_{" 
                + _b + @"}^{x}" + results["Line4"] + @"dx = ... \Rightarrow";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Fourth 1 integral
        latex = @"\bullet \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"dx = " + $"{results["A1Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Fourth 2 integral
        latex = @"\bullet \int_{" + _a + @"}^{0}(" + results["Line2"] + @")dx = " + $"{results["A2Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Fourth 3 integral
        latex = @"\bullet \int_{0}^{" + _b + @"}(" + results["Line3"] + @")dx = " + $"{results["A3Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Fourth 4 integral
        latex = @"\bullet \int_{" + _b + @"}^{x}(" + results["Line4"] + @")dx = " + $"{results["Fx4Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Fourth sum
        latex = @"\star " + $" {results["A1Integral"]} + {results["A2Integral"]} + {results["A3Integral"]} " +
                $"+ {results["Fx4Integral"]} = {results["Fx4RawSum"]}" +
                @" \Rightarrow " + $"a = {results["A"]} " + @" \Rightarrow " + $"{results["Fx4Sum"]}" ;
        images.Add(LaTeXService.RenderToPng(latex));
        
        
        // F(x) system (integrals)
        latex = @"F(x) = \left\{\matrix{" 
                + results["Fx1Sum"] +  @", & x \leq  " + _a + @" \\ " 
                + results["Fx2Sum"] + @", & " + _a + @" < x \leq  0 \\ " 
                + results["Fx3Sum"] + @", & 0 < x \leq  " + _b + @" \\ " 
                + results["Fx4Sum"] + @", & x >  " + _b 
                + @"}\right.";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Intersection with the y-axis Title
        latex = @"\text{Intersection with the y-axis: }";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Intersection with the y-axis, second integral
        query = $"{results["Fx2Sum"]}, where x = 0";
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query)));
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query, PodId.Result)));
        
        // Intersection with the y-axis third integral
        query = $"{results["Fx3Sum"]}, where x = 0";
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query)));
        images.Add(BitmapService.ByGenboxImage(_solver.Image(query, PodId.Result)));
        
        // Graphic :(
        latex = @"\text{Plot. Unfortunately, you have to draw it yourself :(}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        
        
        // M(X) TITLE
        text = "Find M(x):";
        images.Add(BitmapService.ByText(text, 24, FontStyle.Underline));
        
        // Formula M(x) title
        latex = @"\text{Formula:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula M(x)
        latex = @"M(x) = \int_{-\infty}^{\infty}xf(x)dx \Rightarrow";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Formula M(x) extended
        latex = @"\star \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"xdx + \int_{" + _a +
                @"}^{0}x(" + results["Line2"] + @")dx + \int_{0}^{" + _b + @"}x(" + results["Line3"] + @")dx + \int_{" + _b +
                @"}^{\infty}" + results["Line4"] + @"xdx = ... ";
        images.Add(LaTeXService.RenderToPng(latex));
        
        // Integrals Title
        latex = @"\text{Each integral separately:}";
        images.Add(LaTeXService.RenderToPng(latex));
        
        return images;
    }
}