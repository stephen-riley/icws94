// See https://aka.ms/new-console-template for more information
using CoreWar.Asm;

var src = File.ReadAllText("/tmp/dwarf.rc");
var ctx = AssemblyContext.CreateAndAssemble(src);
Console.WriteLine(ctx.Instructions.Count());
