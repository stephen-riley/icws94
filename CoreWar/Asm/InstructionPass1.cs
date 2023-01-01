using CoreWar.Runtime;

namespace CoreWar.Asm;

public class InstructionPass1 : Instruction
{
    public bool Resolved { get; set; }
    public string? Label { get; set; }
    public string? Comment { get; set; }

    public AssemblyContext? ForContext { get; set; }

    public bool IsDirective()
    {
        return Opcode switch
        {
            Mnemonic.EQU => true,
            Mnemonic.END => true,
            Mnemonic.ORG => true,
            Mnemonic.FOR => true,
            Mnemonic.ROF => true,
            _ => false
        };
    }

    public bool IsInstruction() => !IsDirective() && Opcode != Mnemonic.NoInstruction;
}