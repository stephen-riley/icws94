namespace CoreWar.Runtime;

public enum Mnemonic
{
    NoInstruction = -1,
    DAT = 0,
    MOV,
    ADD,
    SUB,
    MUL,
    DIV,
    MOD,
    JMP,
    JMZ,
    JMN,
    DJN,
    CMP,
    SEQ,
    SNE,
    SLT,
    SPL,
    NOP,
    ORG,
    EQU,
    END
}