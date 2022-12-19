namespace CoreWars.Asm;

using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    private static char[] modeSigils = { '#', '$', '*', '@', '{', '<', '}', '>' };
    private static Dictionary<char, AddrMode> addrModeXlat = Enumerable.Range(0, modeSigils.Length).ToDictionary(n => modeSigils[n], n => (AddrMode)n);
    private static HashSet<string> opcodes = Enum.GetNames<Opcode>().ToHashSet();

    public static AddrMode ToAddrMode(this char sigil) => addrModeXlat.ContainsKey(sigil) ? addrModeXlat[sigil] : AddrMode.Direct;

    public static AddrMode ToAddrMode(this string sigil) => sigil != "" ? addrModeXlat[sigil[0]] : AddrMode.Direct;

    public static bool IsOpcode(this string mnemonic) => opcodes.Contains(mnemonic.ToUpperInvariant().Split('.')[0]);

    public static (Opcode, OpMod) ToOpcode(this string mnemonic)
    {
        var pieces = mnemonic.Split('.');

        if (Enum.TryParse<Opcode>(pieces[0].ToUpperInvariant(), out Opcode opcode))
        {
            if (pieces.Length > 1)
            {
                if (Enum.TryParse<OpMod>(pieces[1].ToUpperInvariant(), out OpMod opmod))
                {
                    return (opcode, opmod);
                }
            }
            else
            {
                return (opcode, OpMod.Default);
            }
        }

        throw new AsmException($"Unknown mnemonic and/or modifier ${mnemonic}");
    }

    public static bool EqualsIgnoreCase(this string s, string target) => s.Equals(target, StringComparison.OrdinalIgnoreCase);

    public static string WithoutAddrMode(this string rawOperand) => rawOperand.ToAddrMode() == AddrMode.None ? rawOperand : rawOperand.Substring(1);

    public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> enumerable) => new LinkedList<T>(enumerable);

    public static T? Shift<T>(this LinkedList<T> list)
    {
        var node = list.First;
        if (node is null)
        {
            return default(T);
        }
        var el = node.Value;
        list.RemoveFirst();
        return el;
    }
}