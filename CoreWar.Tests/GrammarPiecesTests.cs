using CoreWar.Asm;
using CoreWar.Runtime;
using Sprache;

namespace CoreWar.Test;

[TestClass]
public class GrammarPiecesTests
{
    [TestMethod]
    public void OperandValueTest()
    {
        var val = RedcodeGrammar.OperandValue.Parse("1");
        Assert.AreEqual("1", val);
    }

    [TestMethod]
    public void StaticLabelValuesAreCorrect()
    {
        var ctx = AssemblyContext.CreateAndAssemble(@"
            l1:  dat     #-1, @2
            l2:  mov     #1, #2");
        Assert.AreEqual(0, ctx.Labels["l1"]);
        Assert.AreEqual(1, ctx.Labels["l2"]);
        Assert.AreEqual(Mnemonic.DAT, ctx.Instructions[0].Opcode);
        Assert.AreEqual(Mnemonic.MOV, ctx.Instructions[1].Opcode);
    }

    [TestMethod]
    public void StaticLabelValuesAreCorrect2()
    {
        var ctx = AssemblyContext.CreateAndAssemble(@"
            l2:  mov     #1, #2
            l1:  dat     #-1
        ");
        Assert.AreEqual(1, ctx.Labels["l1"]);
        Assert.AreEqual(0, ctx.Labels["l2"]);
        Assert.AreEqual(Mnemonic.MOV, ctx.Instructions[0].Opcode);
        Assert.AreEqual(Mnemonic.DAT, ctx.Instructions[1].Opcode);
    }
}