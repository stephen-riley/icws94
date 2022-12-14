namespace CoreWar.Runtime;

public class Instruction
{
    public int AddrOffset { get; set; }
    public Mnemonic Opcode { get; set; }
    public OpcodeModifier OpMod { get; set; }
    public Operand? OperandA { get; set; }
    public Operand? OperandB { get; set; }

    public bool IsResolved() => (OperandA?.IsResolved() ?? true) && (OperandB?.IsResolved() ?? true);

    public override string ToString() =>
        $"{Opcode}{(OpMod != OpcodeModifier.Default ? $".{OpMod}" : "")} {OperandA?.ToString() ?? ""} {OperandB?.ToString() ?? ""}";
}