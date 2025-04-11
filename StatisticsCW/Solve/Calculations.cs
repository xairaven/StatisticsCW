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

        // First lines
        _results["Line1"] = "0";
        SaveResult("Line2", $"Line equation, points ({_a},0), (0,a)", PodId.Result);
        SaveResult("Line3", $"Line equation, points (0, a), ({_b}, 0)", PodId.Result);
        _results["Line4"] = "0";

        // FINDING A

        // First integral
        SaveResult("A1Integral", $"Integrate({_results["Line1"]}) from x=-inf to x={_a}", PodId.Result);
        // Second integral
        SaveResult("A2Integral", $"Integrate({_results["Line2"]}) from x={_a} to x=0", PodId.Result);
        // Third integral
        SaveResult("A3Integral", $"Integrate({_results["Line3"]}) from x=0 to x={_b}", PodId.Result);
        // Fourth integral
        SaveResult("A4Integral", $"Integrate({_results["Line4"]}) from x={_b} to x=inf", PodId.Result);

        // Sum
        SaveResult("ASum", $"{_results["A1Integral"]} + {_results["A2Integral"]} " +
                           $"+ {_results["A3Integral"]} + {_results["A4Integral"]}", PodId.Result);

        // A
        SaveResult("A", $"Solve {_results["ASum"]} = 1", PodId.Result);

        // F(X)
        // First interval
        SaveResult("Fx1Integral", $"Integrate({_results["Line1"]}) from x=-inf to x=x", PodId.Result);
        _results["Fx1Sum"] = _results["Fx1Integral"];

        // Second interval
        // x integral
        SaveResult("Fx2Integral", $"Integrate({_results["Line2"]}) from x={a} to x=x", PodId.Result);
        TrySaveResult("Fx2Integral", $"{_results["Fx2Integral"]}", PodId.ExpandedForm);

        // Raw sum
        SaveResult("Fx2RawSum", $"{_results["A1Integral"]} + {_results["Fx2Integral"]}", PodId.Result);

        // Sum
        SaveResult("Fx2Sum", $"{_results["Fx2RawSum"]}, a = {_results["A"]}", PodId.Result);
        TrySaveResult("Fx2Sum", $"{_results["Fx2Sum"]}", PodId.ExpandedForm);

        // Third interval
        // x integral
        SaveResult("Fx3Integral", $"Integrate({_results["Line3"]}) from x=0 to x=x", PodId.Result);
        TrySaveResult("Fx3Integral", $"{_results["Fx3Integral"]}", PodId.ExpandedForm);

        // Raw sum
        SaveResult("Fx3RawSum", $"{_results["A1Integral"]} + {_results["A2Integral"]} + {_results["Fx3Integral"]}",
            PodId.Result);

        // Sum
        SaveResult("Fx3Sum", $"{_results["Fx3RawSum"]}, a = {_results["A"]}", PodId.Result);
        TrySaveResult("Fx3Sum", $"{_results["Fx3Sum"]}", PodId.ExpandedForm);

        // Fourth interval
        // x integral
        SaveResult("Fx4Integral", $"Integrate({_results["Line4"]}) from x={_b} to x=x", PodId.Result);
        TrySaveResult("Fx4Integral", $"{_results["Fx4Integral"]}", PodId.ExpandedForm);

        // Raw sum
        SaveResult("Fx4RawSum", $"{_results["A1Integral"]} + {_results["A2Integral"]} " +
                                $"+ {_results["A3Integral"]} + {_results["Fx4Integral"]}", PodId.Result);

        // Sum
        SaveResult("Fx4Sum", $"{_results["Fx4RawSum"]}, a = {_results["A"]}", PodId.Result);
        TrySaveResult("Fx4Sum", $"{_results["Fx4Sum"]}", PodId.ExpandedForm);

        // FINDING M(x)

        // First integral
        SaveResult("Mx1Integral", $"Integrate({_results["Line1"]}x) from x=-inf to x={_a}", PodId.Input);
        // Second integral
        SaveResult("Mx2Integral", $"Integrate(({_results["Line2"]}) x) from x={_a} to x=0", PodId.Input);
        // Third integral
        SaveResult("Mx3Integral", $"Integrate(({_results["Line3"]}) x) from x=0 to x={_b}", PodId.Input);
        // Fourth integral
        SaveResult("Mx4Integral", $"Integrate({_results["Line4"]}x) from x={_b} to x=inf", PodId.Input);

        // Raw Sum
        SaveResult("MxRawSum", $"{_results["Mx1Integral"]} + {_results["Mx2Integral"]} " +
                               $"+ {_results["Mx3Integral"]} + {_results["Mx4Integral"]}", PodId.Result);

        // Sum
        SaveResult("MxSum", $"{_results["MxRawSum"]}, a = {_results["A"]}", PodId.Result);

        // M(x) Number
        SaveResult("MxFloat", $"N[{_results["MxSum"]}, 10]", PodId.Result);

        // FINDING D(x)

        // First integral
        SaveResult("Mx2Integral1", $"Integrate({_results["Line1"]}x^2) from x=-inf to x={_a}", PodId.Input);
        // Second integral
        SaveResult("Mx2Integral2", $"Integrate(({_results["Line2"]}) x^2) from x={_a} to x=0", PodId.Input);
        // Third integral
        SaveResult("Mx2Integral3", $"Integrate(({_results["Line3"]}) x^2) from x=0 to x={_b}", PodId.Input);
        // Fourth integral
        SaveResult("Mx2Integral4", $"Integrate({_results["Line4"]}x^2) from x={_b} to x=inf", PodId.Input);

        // M(x^2) Raw Sum
        SaveResult("Mx2RawSum", $"{_results["Mx2Integral1"]} + {_results["Mx2Integral2"]} " +
                                $"+ {_results["Mx2Integral3"]} + {_results["Mx2Integral4"]}", PodId.Result);

        // Sum
        SaveResult("Mx2Sum", $"{_results["Mx2RawSum"]}, a = {_results["A"]}", PodId.Result);

        // M(x)^2 Sum
        SaveResult("m2Sum", $"({_results["MxSum"]})^2", PodId.Result);

        // D(x) Sum
        SaveResult("Dx", $"{_results["Mx2Sum"]} - {_results["m2Sum"]}", PodId.Result);

        // D(x) Number
        SaveResult("DxFloat", $"N[{_results["Dx"]}, 10]", PodId.Result);

        // G(x)
        SaveResult("G", $"sqrt({_results["Dx"]}) 10 digits", PodId.Result);
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
                + results["Line1"] + @", & x \leq  " + _a + @" \\ "
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
                @"}^{0}(" + results["Line2"] + @")dx + \int_{0}^{" + _b + @"}(" + results["Line3"] + @")dx + \int_{" +
                _b +
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
        latex =
            $"{results["A1Integral"]} + {results["A2Integral"]} + {results["A3Integral"]} + {results["A4Integral"]}" +
            $" = {results["ASum"]} " + @"\Rightarrow " + results["ASum"] + " = 1";
        images.Add(LaTeXService.RenderToPng(latex));

        // A
        latex = $"a = {results["A"]}";
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
                + results["Line1"] + @", & x \leq  " + _a + @" \\ "
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
        latex = @"\star " + $" {results["A1Integral"]} + {results["Fx2Integral"]}" + $" = {results["Fx2RawSum"]}" +
                @" \Rightarrow " + $"a = {results["A"]} " + @" \Rightarrow " + $"{results["Fx2Sum"]}";
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
                @" \Rightarrow " + $"a = {results["A"]} " + @" \Rightarrow " + $"{results["Fx3Sum"]}";
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
                @" \Rightarrow " + $"a = {results["A"]} " + @" \Rightarrow " + $"{results["Fx4Sum"]}";
        images.Add(LaTeXService.RenderToPng(latex));


        // F(x) system (integrals)
        latex = @"F(x) = \left\{\matrix{"
                + results["Fx1Sum"] + @", & x \leq  " + _a + @" \\ "
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
        latex = @"M(x) = \int_{-\infty}^{\infty}x f(x)dx = ... \Rightarrow";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula M(x) extended
        latex = @"\star \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"xdx + \int_{" + _a +
                @"}^{0}(" + results["Line2"] + @")xdx + \int_{0}^{" + _b + @"}(" + results["Line3"] + @")xdx + \int_{" +
                _b +
                @"}^{\infty}" + results["Line4"] + @"xdx = ... ";
        images.Add(LaTeXService.RenderToPng(latex));

        // Integrals Title
        latex = @"\text{Each integral separately:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // First integral
        latex = @"\bullet \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"xdx = " + $"{results["Mx1Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Second integral
        latex = @"\bullet \int_{" + _a + @"}^{0}(" + results["Line2"] + @")xdx = " + $"{results["Mx2Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Third integral
        latex = @"\bullet \int_{0}^{" + _b + @"}(" + results["Line3"] + @")xdx = " + $"{results["Mx3Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Fourth integral
        latex = @"\bullet \int_{" + _b + @"}^{\infty}" + results["Line4"] + @"xdx = " + $"{results["Mx4Integral"]}";
        images.Add(LaTeXService.RenderToPng(latex));


        // Sum Title
        latex = @"\star \text{Sum:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Sum
        latex =
            $"{results["Mx1Integral"]} + {results["Mx2Integral"]} + {results["Mx3Integral"]} + {results["Mx4Integral"]}" +
            $" = {results["MxRawSum"]} "
            + @"\Rightarrow " + $"a = {results["A"]} " + @" \Rightarrow " + $"{results["MxSum"]}";
        images.Add(LaTeXService.RenderToPng(latex));


        // M(x)
        latex = $"M(x) = m = {results["MxSum"]} = {results["MxFloat"]}";
        images.Add(LaTeXService.RenderToPng(latex));


        // D(X) TITLE
        text = "Find D(x):";
        images.Add(BitmapService.ByText(text, 24, FontStyle.Underline));

        // Formula D(x) title
        latex = @"\text{Formula:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula D(x)
        latex = @"D(x) = M(x^2)-[M(x)]^2 = d - m^2 = ... ";
        images.Add(LaTeXService.RenderToPng(latex));

        // M(x^2) title
        latex = @"M(x^2) = d = ... \Rightarrow";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula M(x^2) extended
        latex = @"\star \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"x^2dx + \int_{" + _a +
                @"}^{0}x^2(" + results["Line2"] + @")dx + \int_{0}^{" + _b + @"}x^2(" + results["Line3"] +
                @")dx + \int_{" + _b +
                @"}^{\infty}" + results["Line4"] + @"x^2dx = ... ";
        images.Add(LaTeXService.RenderToPng(latex));

        // Integrals Title
        latex = @"\text{Each integral separately:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // First integral
        latex = @"\bullet \int_{-\infty}^{" + _a + @"}" + results["Line1"] + @"x^2dx = " + $"{results["Mx2Integral1"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Second integral
        latex = @"\bullet \int_{" + _a + @"}^{0}(" + results["Line2"] + @")x^2dx = " + $"{results["Mx2Integral2"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Third integral
        latex = @"\bullet \int_{0}^{" + _b + @"}(" + results["Line3"] + @")x^2dx = " + $"{results["Mx2Integral3"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Fourth integral
        latex = @"\bullet \int_{" + _b + @"}^{\infty}" + results["Line4"] + @"x^2dx = " + $"{results["Mx2Integral4"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Sum Title
        latex = @"\star \text{Sum:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Sum
        latex =
            $"{results["Mx2Integral1"]} + {results["Mx2Integral2"]} + {results["Mx2Integral3"]} + {results["Mx2Integral4"]}" +
            $" = {results["Mx2RawSum"]} "
            + @"\Rightarrow " + $"a = {results["A"]} " + @" \Rightarrow " + $"{results["Mx2Sum"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // M(x^2)
        latex = $"M(x^2) = d = {results["Mx2Sum"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // M(x)^2
        latex = $"M(x)^2 = m^2 = ({results["MxSum"]})^2 = {results["m2Sum"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula D(x)
        latex = @"D(x) = M(x^2)-[M(x)]^2 = d - m^2 = " + $"{results["Mx2Sum"]} -{results["m2Sum"]} = {results["Dx"]}";
        images.Add(LaTeXService.RenderToPng(latex));

        // D(x)
        latex = $"D(x) = {results["Dx"]} = {results["DxFloat"]}";
        images.Add(LaTeXService.RenderToPng(latex));


        // G(X) TITLE
        text = "Find G(x):";
        images.Add(BitmapService.ByText(text, 24, FontStyle.Underline));

        // Formula G(x) title
        latex = @"\text{Formula:}";
        images.Add(LaTeXService.RenderToPng(latex));

        // Formula G(x)
        latex = @"\sigma(x) = \sqrt{D(x)} = \sqrt{" + $"{results["Dx"]}" + "} = " + results["G"];
        images.Add(LaTeXService.RenderToPng(latex));

        // G(x)
        latex = @"\sigma(x) = " + results["G"];
        images.Add(LaTeXService.RenderToPng(latex));

        return images;
    }

    private string QuerySolver(string query, PodId podId)
    {
        return _solver.OperandFromResult(_solver.PlainText(query, podId));
    }

    private void SaveResult(string key, string query, PodId podId)
    {
        _results[key] = QuerySolver(query, podId);
    }

    private void TrySaveResult(string key, string query, PodId podId)
    {
        try
        {
            SaveResult(key, query, podId);
        }
        catch (Exception)
        {
        }
    }
}