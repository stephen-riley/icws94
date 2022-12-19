namespace CoreWar.Instructions;

public class InstructionPass1 : Instruction
{
    public bool Resolved { get; set; }
    public string? LabelA { get; set; }
    public string? LabelB { get; set; }
}