namespace CoreWar.Asm;

using CoreWar.Runtime;

public class AssemblyContext
{
    // private int forIndex;
    // private string? forLabel;

    public Dictionary<string, int> Labels { get; } = new();

    public List<InstructionPass1> Instructions { get; private set; } = new();

    public int Org { get; private set; }

    // private AssemblyContext? parent;

    public static AssemblyContext CreateAndAssemble(string src)
    {
        var pass1 = new List<InstructionPass1>(RedcodeGrammar.ParseProgram(src));
        var ctx = new AssemblyContext();

        int offset = 0;

        // TODO: expand FOR..ROF blocks

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

        // resolve all operands
        foreach (var instr in pass1.Where(i => i.IsInstruction() && !i.IsResolved()))
        {
            if (instr.OperandA is Operand opA)
            {
                if (opA.LabelExpr is string labelExpr)
                {
                    instr.OperandA.Value = ctx.Labels[labelExpr];
                    opA.LabelExpr = null;
                }
            }

            if (instr.OperandB is Operand opB)
            {
                if (opB.LabelExpr is string labelExpr)
                {
                    instr.OperandB.Value = ctx.Labels[labelExpr];
                    opB.LabelExpr = null;
                }
            }
        }

        foreach (var instr in pass1.Where(i => i.IsInstruction()))
        {
            Console.WriteLine(instr);
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
            return op.Value;
        }
        else
        {
            op.Value = Labels[op.LabelExpr.ToLowerInvariant()];
            op.LabelExpr = null;
            return op.Value;
        }
    }
}