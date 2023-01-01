namespace CoreWar.Asm;

using System.Linq;
using CoreWar.Runtime;
using Sprache;

public static class RedcodeGrammar
{
    // Use this instead of Parse.Token<T>() because we do NOT want
    //  to include CRLF chars
    private static Parser<T> Tokenize<T>(this Parser<T> parser)
    {
        if (parser == null) throw new ArgumentNullException(nameof(parser));

        return from leading in Parse.Chars(' ', '\t').Many()
               from item in parser
               from trailing in Parse.Chars(' ', '\t').Many()
               select item;
    }

    private static string[] mnemonics = Enum.GetNames<Mnemonic>();
    private static string[] modifiers = Enum.GetNames<OpcodeModifier>().Where(m => m != "Default").ToArray();

    public static InstructionPass1 ParseLine(string line) => Line.Parse(line);
    public static IEnumerable<InstructionPass1> ParseProgram(string lines) => Program.Parse(lines);

    public static Parser<Mnemonic> Mnemonic =
        from m in Parse.Regex("(?i)(" + string.Join('|', mnemonics) + ")").Tokenize().Named("Mnemonic")
        select m.ToEnum<Mnemonic>();

    public static Parser<OpcodeModifier> OpcodeModifier =
        from _ in Parse.Char('.')
        from m in Parse.Regex("(?i)(" + string.Join('|', modifiers) + ")").Tokenize().Named("OpcodeModifier")
        select m.ToEnum<OpcodeModifier>(Runtime.OpcodeModifier.Default);

    public static Parser<AddrMode> AddrMode =
        from am in Parse.Chars('#', '$', '*', '@', '{', '<', '}', '>').Named("AddrMode").Optional()
        select am.IsDefined ? am.Get().ToAddrMode() : Runtime.AddrMode.Direct;

    public static Parser<string> Comment =
        from _ in Parse.Char(';').Tokenize()
        from c in Parse.AnyChar.Except(Parse.LineEnd).Many().Text().Named("Comment")
        select c;

    public static Parser<Opcode> Opcode =
        from mne in Mnemonic
        from mod in OpcodeModifier.Optional()
        select new Opcode { Mnemonic = mne, Modifier = mod.IsDefined ? mod.Get() : Runtime.OpcodeModifier.Default };

    public static Parser<string> SignedNumber =
        from neg in Parse.Char('-').Optional()
        from num in Parse.Number
        select neg.IsDefined ? $"-{num}" : num;

    public static Parser<string> OperandValue =
        (SignedNumber.Tokenize()).Or(
            from c1 in Parse.Letter
            from c2 in LetterOrDigitOrAmpersand.Many().Text().Tokenize()
            select c1 + c2
        );

    public static Parser<Operand> Operand =
        from am in AddrMode
        from ex in OperandValue
        select new Operand(ex, am);

    public static Parser<char> LetterOrDigitOrAmpersand =
        Parse.LetterOrDigit.Or(Parse.Char('&'));

    public static Parser<string> Label =
        from _ in Parse.Not(Opcode)
        from c1 in Parse.Letter
        from c2 in LetterOrDigitOrAmpersand.Many().Text().Tokenize()
        from __ in Parse.Char(':').Optional()
        select c1 + c2;

    public static Parser<InstructionPass1> Instruction =
        from op in Opcode
        from opA in Operand
        from _ in Parse.Char(',').Tokenize().Optional()
        from opB in Operand.Optional()
        select new InstructionPass1
        {
            Opcode = op.Mnemonic,
            OpMod = op.Modifier,
            OperandA = opA,
            OperandB = opB.ResolveOptional()
        };

    public static Parser<InstructionPass1> Line =
        from _ in Parse.WhiteSpace.Many().Optional()
        from l in Label.Optional()
        from instr in Instruction.Optional()
        from c in Comment.Optional()
        select new InstructionPass1()
        {
            Opcode = instr.ResolveOptional()?.Opcode ?? Runtime.Mnemonic.NoInstruction,
            OpMod = instr.ResolveOptional()?.OpMod ?? Runtime.OpcodeModifier.Default,
            Label = l.ResolveOptional(),
            OperandA = instr.ResolveOptional()?.OperandA,
            OperandB = instr.ResolveOptional()?.OperandB,
            Comment = c.ResolveOptional()?.Trim()
        };

    public static Parser<IEnumerable<InstructionPass1>> Program =
        from first in Line.Once()
        from rest in (
            Parse.LineEnd.Then(_ => Line)
        ).Many()
        select first.Concat(rest);
}
