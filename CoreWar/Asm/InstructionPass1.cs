using CoreWar.Runtime;

namespace CoreWar.Asm;

public class InstructionPass1 : Instruction
{
    public bool Resolved { get; set; }
    public string? Label { get; set; }
    public string? LabelA { get; set; }
    public string? LabelB { get; set; }
    public string? Comment { get; set; }
}