public class Instruction
{
    public uint AddrOffset { get; set; }
    public Opcode Opcode { get; set; }
    public OpMod OpMod { get; set; }
    public AddrMode ModeA { get; set; }
    public AddrMode ModeB { get; set; }
    public int OpA { get; set; }
    public int OpB { get; set; }
}