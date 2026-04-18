using MathGame.Web.Models.MathWar;

namespace MathGame.Web.Services.MathWar;

public class GateGenerator
{
    private readonly Random _random = new();

    public List<Gate> Generate(int count)
    {
        var gates = new List<Gate>();
        for (int i = 0; i < count; i++)
        {
            gates.Add(new Gate
            {
                OptionA = RandomOperation(),
                OptionB = RandomOperation(),
                YPosition = -20.0 - i * 30.0  // staggered start above viewport
            });
        }
        return gates;
    }

    public int GateCount() => _random.Next(5, 8);

    private Operation RandomOperation()
    {
        var type = (OperationType)_random.Next(0, 4);
        return type switch
        {
            OperationType.Add => new Operation { Type = type, Value = _random.Next(10, 51) },
            OperationType.Subtract => new Operation { Type = type, Value = _random.Next(10, 31) },
            OperationType.Multiply => new Operation { Type = type, Value = _random.Next(15, 21) / 10.0 },
            OperationType.Divide => new Operation { Type = type, Value = 2 },
            _ => throw new InvalidOperationException()
        };
    }
}
