namespace CoreWar.Instructions;

public class Operand
{
    public AddrMode AddrMode;
    public int Value;
    public string? LabelExpr;

    public Operand(string expression, AddrMode addrMode)
    {
        if (int.TryParse(expression, out var i))
        {
            Value = i;
        }
        else
        {
            LabelExpr = expression;
        }

        AddrMode = addrMode;
    }

    public bool IsResolved() => LabelExpr != null;
}