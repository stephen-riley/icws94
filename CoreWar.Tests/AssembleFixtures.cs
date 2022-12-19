using CoreWar.Asm;
using CoreWar.Instructions;

namespace CoreWar.Test;

[TestClass]
public class AssembleFixtures
{
    [TestMethod]
    public void AssembleAllFixtures()
    {
        foreach (var file in Directory.GetFiles("fixtures"))
        {
            var asm = new Assembler();
            asm.Assemble(File.ReadAllText(file));
            Assert.IsTrue(asm.Program.Count > 0);
            foreach (var instr in asm.Program)
            {
                Assert.IsTrue(instr.ModeA != AddrMode.None);
                Assert.IsTrue(instr.ModeB != AddrMode.None);
            }
        }
    }
}