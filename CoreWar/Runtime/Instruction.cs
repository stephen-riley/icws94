namespace CoreWar.Runtime;

public class Instruction
{
    public uint AddrOffset { get; set; }
    public Mnemonic Opcode { get; set; }
    public OpcodeModifier OpMod { get; set; }
    public Operand? OperandA { get; set; }
    public Operand? OperandB { get; set; }

    public bool IsResolved() => (OperandA?.IsResolved() ?? true) && (OperandB?.IsResolved() ?? true);
}