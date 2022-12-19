namespace CoreWars.Asm;

public class TokenizedInstruction
{
    public string? Label { get; set; }
    public string? Opcode { get; set; }
    public List<string> Sigils { get; private set; } = new();
    public List<string> Operands { get; private set; } = new();
    public string? Comment { get; set; }
}