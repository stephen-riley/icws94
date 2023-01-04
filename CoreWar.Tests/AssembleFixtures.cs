using CoreWar.Asm;
using CoreWar.Runtime;

namespace CoreWar.Test;

[TestClass]
public class AssembleFixtures
{
    [TestMethod]
    public void AssembleAllFixtures()
    {
        foreach (var file in Directory.GetFiles("fixtures"))
        {
            var prog = RedcodeGrammar.ParseProgram(File.ReadAllText(file));
            Assert.IsTrue(prog.Count() > 0);
            foreach (var instr in prog)
            {
                Assert.IsTrue(instr.OperandA?.AddrMode != AddrMode.None);
                Assert.IsTrue(instr.OperandB?.AddrMode != AddrMode.None);
            }
        }
    }

    [TestMethod]
    public void AssembleDwarf()
    {
        var src = File.ReadAllText("fixtures/dwarf.rc");
        var prog = RedcodeGrammar.ParseProgram(src);
        Assert.AreEqual(4, prog.Where(i => i.IsInstruction()).Count());
    }
}