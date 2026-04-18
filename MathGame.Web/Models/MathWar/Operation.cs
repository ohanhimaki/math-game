namespace MathGame.Web.Models.MathWar;

public class Operation
{
    public OperationType Type { get; set; }
    public double Value { get; set; }

    public string Label => Type switch
    {
        OperationType.Add => $"+{(int)Value}",
        OperationType.Subtract => $"-{(int)Value}",
        OperationType.Multiply => Value == 1.5 ? "×1.5" : $"×{(int)Value}",
        OperationType.Divide => $"÷{(int)Value}",
        _ => "?"
    };

    public int Apply(int input) => Type switch
    {
        OperationType.Add => input + (int)Value,
        OperationType.Subtract => input - (int)Value,
        OperationType.Multiply => (int)(input * Value),
        OperationType.Divide => (int)(input / Value),
        _ => input
    };
}
