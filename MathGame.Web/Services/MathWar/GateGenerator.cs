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
                YPosition = -20.0 - i * 30.0
            });
        }
        return gates;
    }

    public int GateCount() => _random.Next(5, 8);

    private Operation RandomOperation()
    {
        return _random.Next(0, 5) switch
        {
            0 => Add(),
            1 => Subtract(),
            2 => new Operation { Formula = "x*1.5", Label = "×1.5" },
            3 => new Operation { Formula = "x*2",   Label = "×2" },
            _ => new Operation { Formula = "x/2",   Label = "÷2" },
        };
    }

    private Operation Add()
    {
        var n = _random.Next(10, 51);
        return new Operation { Formula = $"x+{n}", Label = $"+{n}" };
    }

    private Operation Subtract()
    {
        var n = _random.Next(10, 31);
        return new Operation { Formula = $"x-{n}", Label = $"-{n}" };
    }
}
