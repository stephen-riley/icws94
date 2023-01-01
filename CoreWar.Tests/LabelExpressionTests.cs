using CoreWar.Asm;

namespace CoreWar.Test;

[TestClass]
public class LabelExpressionTests
{
    [TestMethod]
    public void ParseLabelExpressions()
    {
        var instr = RedcodeGrammar.ParseLine("label&idx mov 1, 2");
        Assert.IsNotNull(instr);
        Assert.AreEqual("label&idx", instr.Label);
    }

    [TestMethod]
    public void StaticLabelValuesAreCorrect()
    {
        var ctx = AssemblyContext.CreateAndAssemble(@"
            l1:  dat     #-1
            l2:  mov     #1, #2");
        Assert.AreEqual(0, ctx.Labels["l1"]);
        Assert.AreEqual(1, ctx.Labels["l2"]);
    }
}