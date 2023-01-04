namespace CoreWar.Runtime;
using static AddrMode;

public static class Extensions
{
    private static Dictionary<AddrMode, string> sigils = new Dictionary<AddrMode, string>
    {
        [Immediate] = "#",
        [Direct] = "",
        [IndirectA] = "*",
        [IndirectB] = "@",
        [PreDecIndirectA] = "{",
        [PreDecIndirectB] = "<",
        [PostDecIndirectA] = "}",
        [PostDecIndirectB] = ">",
    };

    public static string ToSigil(this AddrMode mode) => sigils[mode];
}