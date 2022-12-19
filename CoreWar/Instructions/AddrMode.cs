namespace CoreWar.Instructions;

public enum AddrMode
{
    None = -1,
    Immediate,
    Direct,
    IndirectA,
    IndirectB,
    PreDecIndirectA,
    PreDecIndirectB,
    PostDecIndirectA,
    PostDecIndirectB
}
