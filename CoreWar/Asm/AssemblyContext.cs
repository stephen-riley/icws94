namespace CoreWar.Asm;

using CoreWar.Runtime;

public class AssemblyContext
{
    private int forIndex;
    private string? forLabel;

    public Dictionary<string, int> Labels { get; } = new();

    public List<InstructionPass1> Instructions { get; private set; } = new();

    public int Org { get; private set; }

    private AssemblyContext? parent;

    public static AssemblyContext CreateAndAssemble(string src)
    {
        var pass1 = new List<InstructionPass1>(RedcodeGrammar.ParseProgram(src));
        var ctx = new AssemblyContext();

        int offset = 0;

        // assign offsets to instructions
        foreach (var instr in pass1)
        {
            instr.AddrOffset = instr.IsInstruction() ? offset++ : 0;
            if (instr.Opcode == Mnemonic.ORG)
            {
                ctx.Org = instr.AddrOffset;
            }
        }

        // collect and assign labels
        foreach (var instr in pass1)
        {
            if (instr.Label is not null && instr.Opcode != Mnemonic.EQU)
            {
                ctx.Labels[instr.Label.ToLowerInvariant()] = instr.AddrOffset;
            }
        }

        // assign EQU labels
        foreach (var instr in pass1.Where(i => i.Opcode == Mnemonic.EQU))
        {
            if (instr.Label is null)
            {
                throw new AsmException($"EQU requires label");
            }
            else
            {
                ctx.Labels[instr.Label.ToLowerInvariant()] = ctx.ResolveExpression(instr, OperandName.A);
            }
        }

        ctx.Instructions = pass1.Where(i => i.IsInstruction()).ToList();

        return ctx;
    }

    private int ResolveExpression(InstructionPass1 instr, OperandName opName)
    {
        var op = opName == OperandName.A ? instr.OperandA : instr.OperandB;

        if (op is null)
        {
            throw new AsmException($"Operand {opName} is null in {nameof(ResolveExpression)}");
        }

        if (op.LabelExpr is null)
        {
            throw new AsmException($"Operand {opName} has no contents in {nameof(ResolveExpression)}");
        }

        if (int.TryParse(op.LabelExpr, out int i))
        {
            op.Value = i;
            return i;
        }
        else
        {
            op.Value = Labels[op.LabelExpr.ToLowerInvariant()] + instr.AddrOffset;
            return op.Value;
        }
    }
}