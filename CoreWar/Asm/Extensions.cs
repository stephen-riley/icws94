namespace CoreWar.Asm;

using System.Collections.Generic;
using System.Linq;
using CoreWar.Runtime;
using Sprache;

public static class Extensions
{
    private static char[] modeSigils = { '#', '$', '*', '@', '{', '<', '}', '>' };
    private static Dictionary<char, AddrMode> addrModeXlat = Enumerable.Range(0, modeSigils.Length).ToDictionary(n => modeSigils[n], n => (AddrMode)n);
    private static HashSet<string> opcodes = Enum.GetNames<Mnemonic>().ToHashSet();

    public static AddrMode ToAddrMode(this char sigil) => addrModeXlat.ContainsKey(sigil) ? addrModeXlat[sigil] : AddrMode.Direct;

    public static AddrMode ToAddrMode(this string sigil) => sigil != "" ? sigil[0].ToAddrMode() : AddrMode.Direct;

    public static bool IsOpcode(this string mnemonic) => opcodes.Contains(mnemonic.ToUpperInvariant().Split('.')[0]);

    public static (Mnemonic, OpcodeModifier) ToOpcode(this string mnemonic)
    {
        var pieces = mnemonic.Split('.');

        if (Enum.TryParse<Mnemonic>(pieces[0].ToUpperInvariant(), out Mnemonic opcode))
        {
            if (pieces.Length > 1)
            {
                if (Enum.TryParse<OpcodeModifier>(pieces[1].ToUpperInvariant(), out OpcodeModifier opmod))
                {
                    return (opcode, opmod);
                }
            }
            else
            {
                return (opcode, OpcodeModifier.Default);
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

    public static T ToEnum<T>(this string value) where T : struct
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException();
        }

        T result;
        return Enum.TryParse<T>(value, true, out result) ? result : throw new InvalidOperationException($"{value} is not a member of enum {typeof(T)}");
    }

    public static T ToEnum<T>(this string value, T defaultValue) where T : struct
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        T result;
        return Enum.TryParse<T>(value, true, out result) ? result : defaultValue;
    }

    public static T? ResolveOptional<T>(this IOption<T> option)
    {
        return option.IsDefined ? option.Get() : default(T);
    }
}