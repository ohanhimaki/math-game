using NCalc;

namespace MathGame.Web.Models.MathWar;

public class Operation
{
    /// <summary>NCalc formula using 'x' as the variable, e.g. "x*2+100"</summary>
    public string Formula { get; set; } = "x";

    /// <summary>Human-readable label. x-first formulas omit the leading x (e.g. "+30", "×2"). 
    /// Formulas where x is not first show x explicitly (e.g. "100×x").</summary>
    public string Label { get; set; } = "?";

    public int Apply(int input)
    {
        var expr = new Expression(Formula);
        expr.Parameters["x"] = (double)input;
        return Convert.ToInt32(expr.Evaluate());
    }
}
