using CoreWar.Asm;

namespace CoreWar.Test;

[TestClass]
public class AssemblerTests
{
    [TestMethod]
    public void EmptyProgram()
    {
        var asm = new Assembler();
        asm.Assemble("");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(0, asm.Program.Count);
    }

    [TestMethod]
    public void OnlyComment()
    {
        var asm = new Assembler();
        asm.Assemble("; test");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(0, asm.Program.Count);
    }

    [TestMethod]
    public void WeirdWhitespace()
    {
        var asm = new Assembler();
        asm.Assemble("\r");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(0, asm.Program.Count);
    }

    [TestMethod]
    public void LineWithLabel()
    {
        var asm = new Assembler();
        asm.Assemble("step  EQU     4");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(0, asm.Program.Count);
    }

    [TestMethod]
    public void LineWithOnlyLabel()
    {
        var asm = new Assembler();
        asm.Assemble("step");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(0, asm.Program.Count);
    }

    [TestMethod]
    public void MultipleLabels()
    {
        var asm = new Assembler();
        asm.Assemble(@"
    l1
    l2  mov 1, 2
    ");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(1, asm.Program.Count);
        Assert.AreEqual(2, asm.Labels.Count);
        Assert.AreEqual(0, asm.Labels["l1"]);
        Assert.AreEqual(0, asm.Labels["l2"]);
    }

    [TestMethod]
    public void LabelsWithColons()
    {
        var asm = new Assembler();
        asm.Assemble(@"
    l1:
    l2:  mov 1, l1
    ");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(1, asm.Program.Count);
        Assert.AreEqual(2, asm.Labels.Count);
        Assert.AreEqual(0, asm.Labels["l1"]);
        Assert.AreEqual(0, asm.Labels["l2"]);
    }

    [TestMethod]
    public void BasicInstruction()
    {
        var asm = new Assembler();
        asm.Assemble(@"
            paperb  spl    @paperb,  >paperb");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(1, asm.Program.Count);
    }

    [TestMethod]
    public void LabelExpressions()
    {
        var asm = new Assembler();
        asm.Assemble(@"
            L1      EQU     10
            L2      EQU     7
                    MOV     0, L1-L2");
        Assert.AreEqual(0u, asm.Org);
        Assert.AreEqual(1, asm.Program.Count);
        // Assert.AreEqual(3, asm.Program[0].OpB);
    }

    [TestMethod]
    public void Dwarf()
    {
        var prog = File.ReadAllText(@"fixtures/dwarf.rc");
        var asm = new Assembler();
        asm.Assemble(prog);

        Assert.AreEqual(1u, asm.Org);
        Assert.AreEqual(4, asm.Program.Count);
    }
}