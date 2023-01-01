using CoreWar.Asm;

namespace CoreWar.Test;

[TestClass]
public class AsmGrammarTests
{
    [TestMethod]
    public void EmptyProgram()
    {
        var instr = RedcodeGrammar.ParseLine("");
        Assert.IsNotNull(instr);
    }

    [TestMethod]
    public void OnlyComment()
    {
        var instr = RedcodeGrammar.ParseLine("; test");
        Assert.IsNotNull(instr);
    }

    [TestMethod]
    public void WeirdWhitespace()
    {
        var instr = RedcodeGrammar.ParseLine("\r");
    }

    [TestMethod]
    public void BasicInstructionWithOnlyLiterals()
    {
        var instr = RedcodeGrammar.ParseLine("MOV 1 2 ; comment");
        Assert.IsNotNull(instr);
        Assert.AreEqual(Runtime.Mnemonic.MOV, instr.Opcode);
        Assert.AreEqual(Runtime.OpcodeModifier.Default, instr.OpMod);
        Assert.AreEqual(Runtime.AddrMode.Direct, instr.OperandA?.AddrMode);
        Assert.AreEqual(1, instr.OperandA?.Value);
        Assert.AreEqual(Runtime.AddrMode.Direct, instr.OperandB?.AddrMode);
        Assert.AreEqual(2, instr.OperandB?.Value);
    }

    [TestMethod]
    public void DatInstruction()
    {
        var prog = RedcodeGrammar.ParseProgram("label:     dat #1 ; a comment");
        Assert.AreEqual(1, prog.Count());

        var instr = prog.FirstOrDefault();
        Assert.IsNotNull(instr);
        Assert.AreEqual(Runtime.Mnemonic.DAT, instr.Opcode);
        Assert.AreEqual(Runtime.OpcodeModifier.Default, instr.OpMod);
        Assert.AreEqual(Runtime.AddrMode.Immediate, instr.OperandA?.AddrMode);
        Assert.AreEqual(1, instr.OperandA?.Value);
    }

    [TestMethod]
    public void LineWithLabel()
    {
        var instr = RedcodeGrammar.ParseLine("step  EQU     4");
        Assert.IsNotNull(instr);
    }

    [TestMethod]
    public void LineWithOnlyLabel()
    {
        var instr = RedcodeGrammar.ParseLine("step");
    }

    [TestMethod]
    public void MultipleLabels()
    {
        var prog = RedcodeGrammar.ParseProgram(@"
            l1  dat -1
            l2  mov 1, 2");
        Assert.IsNotNull(prog);
        Assert.IsTrue(prog.Count() > 0);
    }

    [TestMethod]
    public void LabelsWithColons()
    {
        var prog = RedcodeGrammar.ParseProgram(@"
            l1:
            l2:  mov 1, l1");
        Assert.IsNotNull(prog);
        Assert.IsTrue(prog.Count() > 0);
    }

    [TestMethod]
    public void BasicInstruction()
    {
        var prog = RedcodeGrammar.ParseProgram(@"    
            ; leading comment
            paperb  spl    @paperb,  >paperb");
        Assert.IsNotNull(prog);
        Assert.IsTrue(prog.Count() > 0);
    }

    [TestMethod]
    public void LabelExpressions()
    {
        var instr = RedcodeGrammar.ParseLine(@"
            L1      EQU     10
            L2      EQU     7
                    MOV     0, L1-L2");
    }

    [TestMethod]
    public void Dwarf()
    {
        var src = File.ReadAllText(@"fixtures/dwarf.rc");
        var prog = RedcodeGrammar.ParseProgram(src);
        Assert.IsNotNull(prog);
    }
}