using System.Text.RegularExpressions;

namespace CoreWars.Asm;

public class Assembler
{
    private char[] whitespace = new char[] { ' ', '\t', '\r', '\n', ',' };

    private uint offset;
    private string? rawOrgOperand;

    public uint Org { get; set; }

    public Dictionary<string, int> Labels { get; } = new();

    public List<Instruction> Program { get; } = new();

    public void Assemble(string program)
    {
        var pass1 = new List<InstructionPass1>();

        foreach (var line in program.Split('\n'))
        {
            try
            {
                var instr = AssembleLine(line, ref offset);
                if (instr != null)
                {
                    pass1.Add(instr);
                }
            }
            catch (CompletedParsingEvent)
            {
                // pass
            }
        }

        foreach (var instr in pass1)
        {
            if (instr.LabelA is not null)
            {
                instr.OpA = (int)instr.AddrOffset + ResolveExpression(instr.LabelA);
            }

            if (instr.LabelB is not null)
            {
                instr.OpB = (int)instr.AddrOffset + ResolveExpression(instr.LabelB);
            }

            Program.Add(instr);
        }

        if (rawOrgOperand is not null)
        {
            Org = (uint)ResolveExpression(rawOrgOperand);
        }
    }

    public InstructionPass1? AssembleLine(string rawLine, ref uint addrOffset)
    {
        var tokLine = ParseLine(rawLine);

        if (tokLine?.Label is not null)
        {
            RegisterLabel(tokLine.Label, (int)addrOffset);
        }

        if (tokLine?.Opcode is null)
        {
            return null;
        }

        if (tokLine.Opcode.EqualsIgnoreCase("END"))
        {
            throw new CompletedParsingEvent();
        }

        if (tokLine.Opcode.EqualsIgnoreCase("EQU"))
        {
            if (tokLine.Label is null)
            {
                throw new AsmException("EQU requires label");
            }

            RegisterLabel(tokLine.Label, int.Parse(tokLine.Operands[0]));
            return null;
        }

        if (tokLine.Opcode.EqualsIgnoreCase("ORG"))
        {
            rawOrgOperand = tokLine.Operands[0];
            return null;
        }

        var instr = new InstructionPass1() { AddrOffset = addrOffset };

        (instr.Opcode, instr.OpMod) = tokLine.Opcode.ToOpcode();
        if (int.TryParse(tokLine.Operands[0], out int i))
        {
            instr.OpA = i;
        }
        else
        {
            instr.LabelA = tokLine.Operands[0];
        }
        instr.ModeA = tokLine.Sigils[0].ToAddrMode();

        if (tokLine.Operands.Count > 1)
        {
            (instr.Opcode, instr.OpMod) = tokLine.Opcode.ToOpcode();
            if (int.TryParse(tokLine.Operands[1], out i))
            {
                instr.OpB = i;
            }
            else
            {
                instr.LabelB = tokLine.Operands[1];
            }
            instr.ModeB = tokLine.Sigils[1].ToAddrMode();
        }

        addrOffset++;
        return instr;
    }

    private TokenizedInstruction? ParseLine(string rawLine)
    {
        var tokLine = new TokenizedInstruction();

        var commentIndex = rawLine.IndexOf(';');
        if (commentIndex != -1)
        {
            tokLine.Comment = rawLine.Substring(commentIndex + 1);
        }

        var pieces = Regex.Replace(rawLine, ";.*", "").Split(whitespace).Where(x => x != "").ToLinkedList();
        if (pieces.Count == 0)
        {
            return null;
        }

        var el = pieces.Shift();
        if (el is null)
        {
            return null;
        }

        if (!el.IsOpcode())
        {
            tokLine.Label = el;
            el = pieces.Shift();
        }

        if (pieces.Count > 0)
        {
            tokLine.Opcode = el;

            while (pieces.Count > 0)
            {
                el = pieces.Shift() ?? throw new AsmException($"missing operand(s) in {rawLine}");
                if ("#$*@{<}>".IndexOf(el[0]) != -1)
                {
                    tokLine.Sigils.Add(el[0].ToString());
                    el = el.Substring(1);
                }
                else
                {
                    tokLine.Sigils.Add("");
                }

                tokLine.Operands.Add(el);
            }
        }

        return tokLine;
    }

    private void RegisterLabel(string label, int val)
    {
        label = label.ToLowerInvariant();
        Labels[label] = val;
    }

    private int ResolveExpression(string s)
    {
        var pieces = Regex.Split(s, @"([+\-])");
        int value = 0;
        var queuedOp = BinaryOp.None;

        foreach (var piece in pieces)
        {
            switch (piece)
            {
                case "+": queuedOp = BinaryOp.Add; continue;
                case "-": queuedOp = BinaryOp.Sub; continue;
                case "*": queuedOp = BinaryOp.Mul; continue;
                case "/": queuedOp = BinaryOp.Div; continue;
            }

            int i;
            if (!int.TryParse(piece, out i))
            {
                i = Labels[piece.ToLowerInvariant()];
            }

            value = queuedOp switch
            {
                BinaryOp.Add => value + i,
                BinaryOp.Sub => value - i,
                BinaryOp.Mul => value * i,
                BinaryOp.Div => value / i,
                _ => i,
            };
            queuedOp = BinaryOp.None;
        }

        return value;
    }
}
