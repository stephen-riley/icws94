namespace CoreWar.Asm;

public class CompletedParsingEvent : AsmException
{
    public CompletedParsingEvent() : base("Parsing complete")
    {
    }
}