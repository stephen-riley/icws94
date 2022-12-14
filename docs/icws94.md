# Draft of Proposed Core War Standard


```
Version 3.3
Draft Last Modified: November 8, 1995
Last modified by: Damien Doligez
Base draft by: Mark Durham

Markdown version derived from https://corewar.co.uk/standards/icws94.txt
```
 

```
# Contents

   1. Introduction
      1. Purpose
      2. Overview
      3. Acknowledgements
   2. Redcode Assembly File Format
      1. Purpose
      2. Description
      3. Grammar
      4. Assembly To Object Code Conversion
      5. Pseudo-instructions
      6. Comment Conventions
      7. Example Assembly File
   3. Load File Format
      1. Purpose
      2. Description
      3. Grammar
      4. Comment Conventions
      5. Example Load File
   4. Run-time Variables
      1. Purpose
      2. Predefined Labels
      3. Variables
      4. Standard Variable Sets
   5. MARS
      1. Purpose
      2. Description
      3. Address Modes
         1. Immediate
         2. Direct
         3. A-number Indirect
         4. B-number Indirect
         5. A-number Predecrement Indirect
         6. B-number Predecrement Indirect
         7. A-number Postincrement Indirect
         8. B-number Postincrement Indirect
      4. Modifiers
         1. A
         2. B
         3. AB
         4. BA
         5. F
         6. X
         7. I
      5. Instruction Set
         1. DAT
         2. MOV
         3. ADD
         4. SUB
         5. MUL
         6. DIV
         7. MOD
         8. JMP
         9. JMZ
         10. JMN
         11. DJN
         12. SEQ and CMP
         13. SNE
         14. SLT
         15. SPL
         16. NOP
      6. Example MARS interpreter
   6. Validation Suite
      1. Purpose and Requirements
      2. Tests
         1. Assembly to Load File Test
         2. MARS Tests
            1. DAT Tests
            2. MOV Tests
            3. ADD Tests
            4. SUB Tests
            5. MUL Tests
            6. DIV Tests
            7. MOD Tests
            8. JMP Tests
            9. JMZ Tests
            10. JMN Tests
            11. DJN Tests
            12. SEQ/CMP Tests
            13. SNE Tests
            14. SLT Tests
            15. SPL Tests
            16. NOP Tests
   7. Glossary and Index

   A. Differences Between Standards
      1. Purpose
      2. Changes
         1. Assembly Files
            1. ICWS'88 conversion
            2. ICWS'86 conversion
         2. Load Files
         3. MARS
```
 

# 1. Introduction
 

## 1.1 Purpose
 This standard seeks to fully define and describe the game of Core War. 

## 1.2 Overview
 Core War is a game in which programs compete for control of a computer called MARS (for Memory Array Redcode Simulator).  Redcode is the name of the assembly language in which Core War programs, called warriors, are written.  

 In order to play Core Wars, access to a Core War system is required. A Core War system at a minimum must have a MARS executive function (interpreter) and a way to load warriors into core (the MARS memory). Most systems include a Redcode assembler, either separately or as part of the loader.  Also, many systems include a graphical interface and code debugging features.  Some systems have the ability to run automated tournaments. 

## 1.3 Acknowledgements
 This document is the fourth standard for Core War, the first three being "Core War Guidelines" (March 1984) by D. G. Jones and A. K. Dewdney, the International Core War Society standard of - "Core Wars" (May 1986), principally authored by Graeme McRae and the "Core Wars Standard of 1988" (Summer 1988), principally authored by Thomas Gettys.  The latter two standards are often referred to as ICWS'86 and ICWS'88, respectively. 

* Scott W. Adkins
* Mark A. Durham
* Anders Ivner
* Morten Due Joergensen
* Paul Kline
* Scott Nelson
* Jon Newman
* John Perry
* Andy Pierce
* Planar
* Wayne Sheppard
* William Shubert
* Nandor Sieben
* Stefan Strack
* Mintardjo Wangsaw
* Kevin Whyte
 People who contributed to this document (in alphabetical order): 

# 2. Redcode Assembly File Format
 

## 2.1 Purpose
 A Redcode assembly file consists of the information necessary for a Redcode assembler to produce a load file.  A standard assembly file format allows programmers to exchange warriors in a more meaningful format than load files.  An assembly file, through the use of labels, arithmetic expressions, and macros can also greatly reduce the work necessary to produce a particular warrior while enhancing code readability. 

## 2.2 Description
 Each Redcode warrior consists of one or more lines of Redcode.  Each line of Redcode consists of a string of alphanumerals and special characters.  Special characters in Redcode are the addressing mode indicators for immediate `#`, direct `$`, A-number indirect `*`, B-number indirect `@`, A-number predecrement indirect `{`, B-number predecrement indirect `<`, A-number postincrement indirect `}`, and B-number postincrement indirect `>` modes, the field separator (comma) `,`, the comment indicator (semicolon) `;`, the arithmetic operators for addition `+`, subtraction `-`, multiplication `*`, division `/`, and  modulus `%`, and opening `(` and closing `)` parentheses for precedence grouping. 

 A line may be blank or consist of an instruction, a pseudo-instruction, a comment, or an instruction or pseudo-instruction followed by a comment.  Each line is terminated with a newline.  All comments begin with a semicolon.  Each instruction consists of these elements in the following order: a label, an opcode, an A-operand, a comma, and a B-operand.  Each element may be preceded and/or followed by whitespace (newline is not considered whitespace).  The label is optional.  If either operand is blank, the comma may be omitted.  The operands may not be both blank. 

 Pseudo-instructions appear just like instructions but are directives to the assembler and do not result in object code as an instruction would.  Each pseudo-instruction has a pseudo-opcode which appears where an opcode would appear in an instruction.  The format of the remainder of the pseudo-instruction depends on which pseudo-opcode is used.  For the remainder of this section (2.2) and the next (2.3), references to "opcode" include "pseudo-opcode" assembler directives. 

 A label is any alphanumeric string other than those reserved for opcodes.  Labels are case sensitive, i.e. "start" is different from "Start".  An opcode is any of the following: `DAT`, `MOV`, `ADD`, `SUB`, `MUL`, `DIV`, `MOD`, `JMP`, `JMZ`, `JMN`, `DJN`, `CMP`, `SEQ`, `SNE`, `SLT`, `SPL`, `and` `NOP`. Opcodes may be in upper or lower case or any combination.  An opcode may be followed by a modifier.  A modifier always begins with a dot. A modifier is any of the following: `.A`, `.B`, `.AB`, `.BA`, `.F`, `.X`, or `.I`. Modifiers may be in upper or lower case or any combination. 

 Each operand is blank, contains an address, or contains an addressing mode indicator and an address.  An address consists of any number of labels and numbers separated by arithmetic operators and possibly grouped with parentheses.  All elements of an address may be separated by whitespace. 

## 2.3 Grammar
 Tokens are separated by whitespace (space and tab) exclusive of newline characters, which are used for line termination.  End-of-file should occur only where newline could logically occur, otherwise the assembly file is invalid. 

 In the following, 'e' is the "empty" element, meaning the token may be omitted, a caret '^' means NOT, an asterisk '*' immediately adjacent means zero or more occurrences of the previous token, and a plus '+' immediately adjacent means one or more occurrences of the previous token.  The vertical bar '|' means OR. 

```
assembly_file:
		line+ EOF
line:
		comment | instruction
comment:
		; v* newline | newline
instruction:
		label_list operation mode expr comment |
		label_list operation mode expr , mode expr comment
label_list:
		label newline* label_list | e
label:
		alpha alphanumeral*
operation:
		opcode | opcode.modifier
opcode:
		DAT | MOV | ADD | SUB | MUL | DIV | MOD |
		JMP | JMZ | JMN | DJN | CMP | SEQ | SNE |
		SLT | SPL | NOP | ORG | EQU | END
modifier:
		A | B | AB | BA | F | X | I
mode:
		# | $ | * | @ | { | < | } | > | e
expr:
		term |
		term + expr | term - expr |
		term * expr | term / expr |
		term % expr
term:
		label | number | ( expression )
number:
		whole_number | signed_integer
signed_integer:
		+whole_number | -whole_number
whole_number:
		numeral+
alpha:
		A-Z | a-z | _
numeral:
		0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9
alphanumeral:
		alpha | numeral
v:
		^newline
newline:
		LF | CR | LF CR | CR LF
e:
```
 

## 2.4 Assembly To Load File Conversion
 A Redcode program can be assembled into a list of MARS instructions. (When assembled to a file instead of directly to core, the list is called a load file.  See Section 3).  Each Redcode instruction assembles into a MARS instruction of five fields: an opcode.modifier field, the A-mode field, the A-address field, the B-mode field, and the B-address field.  A missing (null or blank) mode assembles as '$' does. 

 If no modifier is present in the assembly instruction, the appropriate modifier is appended to the opcode.  The appropriate modifier depends upon the opcode, the modes, and which standard (ICWS'86 or ICWS'88) to consider (ICWS'88 by default).  See Appendix A for the appropriate translations. 

 The address field values are derived from the numbers, labels, and arithmetic operators contained in the addresses.  Labels are converted to an address relative to the current instruction.  Then the arithmetic operations are carried out according to the appropriate operator and parenthetical precedence to determine the final value.  If there is only one operand associated with a `DAT` instruction, this operand is assembled into the B-mode and B-address fields, while #0 is assembled into the A-mode and A-address fields. For all other instructions with only one operand, this operand is assembled into the A-mode and A-address fields and #0 is assembled into the B-mode and B-address fields. 

## 2.5 Pseudo-instructions
 Pseudo-opcodes are `ORG`, `EQU`, and `END`. 

 `ORG` ("ORiGin") is a way for the assembly file to indicate the logical origin of the warrior.  The A-operand contains an offset to the logical first instruction.  Thus "ORG 0" means execution should start with the first instruction (the default) whereas "ORG 6" means execution should start with the seventh instruction. Although multiple ORG instructions are of no additional benefit to the programmer, they are allowed. When there is more than one ORG instruction in a file, the last ORG instruction encountered will be the one that takes effect. 

 `EQU` ("EQUate") is a simple text substitution utility.  Instructions of the form "label EQU text" will replace all occurrences of "label" with the (probably longer and more complicated) "text" before any actual assembly takes place on the file.  Some labels are predefined with the value of run-time variables as if they were defined with EQU at the start of the program (see section 4.2 for the list of predefined labels). 

 `END` indicates the logical end of the assembly file.  If END has an A-operand, then the A-operand indicates the logical origin of the warrior in the same manner as ORG does.  The rest of the file (after the end of the line containing END) is ignored. 

## 2.6 Comment Conventions
 "`;redcode<switch>`" as a first line identifies the file as a Redcode assembly file.  The `<switch>` is optional and implementation dependent. 

 "`;strategy <text>`" indicates a comment for `public` consumption. 

 "`;name <program name>`", "`;author <name of author(s)>`", "`;version <version number>`", and "`;date <date of last revision>`" offer uniform ways of presenting this information. 

 "`;kill <program name>`" is for removing warriors from ongoing tournaments.  If no <program name> is supplied, all of the author's previous submissions will be removed. 

 "`;assert <expression>`" will evaluate the expression and trigger an error if it is 0.  In conjunction with predefined labels (see section 4.2), this provides a way of specifying the conditions under which a warrior is supposed to run. 

## 2.7 Example Assembly File
 

```
;redcode

;name          Dwarf
;author        A. K. Dewdney
;version       94.1
;date          April 29, 1993

;strategy      Bombs every fourth instruction.
;assert        CORESIZE % 4 == 0

        ORG     start              ; Indicates the instruction with
                                   ; the label "start" should be the
                                   ; first to execute.

step    EQU      4                 ; Replaces all occurrences of "step"
                                   ; with the character "4".

target  DAT.F   #0,     #0         ; Pointer to target instruction.
start   ADD.AB  #step,   target    ; Increments pointer by step.
        MOV.AB  #0,     @target    ; Bombs target instruction.
        JMP.A    start             ; Same as JMP.A -2.  Loops back to
                                   ; the instruction labelled "start".
        END
```
 

# 3. Load File Format
 

## 3.1 Purpose
 A load file represents the minimum amount of information necessary for a warrior to execute properly and is presented in a very simple format which is a subset of the assembly file format presented in Section 2. A standard load file format allows programmers to choose assemblers and MARS programs separately and to verify assembler performance and MARS performance separately.  Not all Core War systems will necessarily write load files (for example, those which assemble directly to core), but all systems should support reading load files. 

## 3.2 Description
 Each load file will consist of one or more lines of MARS instructions or comments.  Each line is terminated with a newline. All comments start with with a semicolon.  Each MARS instruction consists of five fields: an opcode.modifier pair, an A-mode, an A-field, a B-mode, and a B-field.  The A-mode is separated from the opcode.modifier pair by whitespace and the B-mode is separated from the A-field by a comma and additional whitespace.  Each MARS instruction may be followed by extraneous information, which is ignored.  Note that the instruction format for load files is more rigid than for assembly files to simplify parsing. No blank modes or operands are allowed. 

## 3.3 Grammar
 Tokens are separated by whitespace (non-marking characters such as SPACE and TAB) exclusive of newline characters, which are used for line termination.  End-of-file should occur only where newline could logically occur, otherwise the load file is invalid. 

```
load_file:
         line+ EOF
line:
         comment | instruction
comment:
         ; v* newline | newline
instruction:
         opcode.modifier mode number , mode number comment
opcode:
         DAT | MOV | ADD | SUB | MUL | DIV | MOD |
         JMP | JMZ | JMN | DJN | CMP | SEQ | SNE |
         SLT | SPL | NOP | ORG
modifier:
         A | B | AB | BA | F | X | I
mode:
         # | $ | @ | * | < | { | > | }
number:
         whole_number | signed_integer
signed_integer:
         +whole_number | -whole_number
whole_number:
         numeral+
numeral:
         0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9
v:
         ^newline
newline:
   LF | CR | LF CR | CR LF
```
 

## 3.4 Comment Conventions
 Comment conventions for load files are the same as for assembly files. Of particular note are the "`; name <program name>`" and "`; author <author(s)>`" comments.  These comments provide a more suitable identification of programs to the MARS than "Warrior #1", "Warrior #2", etc.  It also allows for less cryptic identification than by filename. 

## 3.5 Example Load File
```
;redcode

;name          Dwarf
;author        A. K. Dewdney
;version       94.1
;date          April 29, 1993

;strategy      Bombs every fourth instruction.
;assert        CORESIZE % 4 == 0

ORG     1          ; Indicates execution begins with the second
                   ; instruction (ORG is not actually loaded, and is
                   ; therefore not counted as an instruction).

DAT.F   #0, #0     ; Pointer to target instruction.
ADD.AB  #4, $-1    ; Increments pointer by step.
MOV.AB  #0, @-2    ; Bombs target instruction.
JMP.A   $-2, #0    ; Loops back two instructions.
```
 

# 4. Run-time Variables
 

## 4.1 Purpose
 This section describes those variables which are determined just prior to running a battle by the person running the battle.  It also enumerates some of the standardized sets of variables used for tournaments. 

## 4.2 Predefined Labels
 Most of the run-time variables are available to the programmer as predefined labels.  The purpose of these labels is twofold: first, to parameterize the warriors with the characteristics of their run-time environment; second, to check (with the ";assert" comment) that the warrior is run in an environment that makes sense for it. 

 The next section gives a list of the run-time variables.  When a predefined label takes the value of the variable, the label is given between parentheses after the name of the variable. 

## 4.2 Variables
 Run-time variables consist of the following: 

### Core Size: `CORESIZE`
 

 Core size is the number of instructions which make up core during the battle. 

### Cycles Before Tie: `MAXCYCLES`
 

 In each cycle, one instruction from each warrior is executed. This variable determines how many cycles without a winner should be executed before declaring a tie. 

### Initial Instruction
 

 The initial instruction is that instruction which is preloaded into core prior to loading warriors.  In addition to loading an instruction such as `DAT` #0, #0` into all of core, the initial instruction could be set to `NONE`, meaning core should not be cleared between battles, or `RANDOM`, meaning core instructions are filled with randomly generated instructions. 

### Instruction Limit: `MAXLENGTH`
 

 The maximum number of instructions allowed per load file. 

### Maximum Number of Tasks: `MAXPROCESSES`
 

 Each warrior can spawn multiple additional tasks.  This variable sets the maximum number of tasks allowed per warrior. In other words, this is the size of each warrior's task queue. 

### Minimum Separation: `MINDISTANCE`
 

 The minimum number of instructions from the first instruction of one warrior to the first instruction of the next warrior. 

### Read Distance
 

 This is the range available for warriors to read information from core.  Attempts to read outside the limits of this range result in reading within the local readable range.  The range is centered on the current instruction.  Thus, a range of 500 limits reading to offsets of (-249 -> +250) from the currently executing instruction.  The read limit can therefore be considered a mini-core within core.  An attempt to read location PC+251 reads location PC-249 instead.  An attempt to read location PC+500 reads location PC instead. 

 Read distance must be a factor of core size, otherwise the above defined behaviour is not guaranteed. 

### Separation
 

 The number of instructions from the first instruction of one warrior to the first instruction of the next warrior. Separation can be set to "RANDOM", meaning separations will be chosen randomly from those larger than the minimum separation. 

### Warriors
 

 The initial number of warriors to battle simultaneously in core. 

### Write Distance
 

 This is the range available for warriors to write information to core.  Attempts to write outside the limits of this range result in writing within the local writable range.  The range is centered on the current instruction.  Thus, a range of 500 limits writing to offsets of (-249 -> +250) from the currently executing instruction.  The write limit can therefore be considered a mini-core within core.  An attempt to write location PC+251 writes to location PC-249 instead. An attempt to write to location PC+500 writes to location PC instead. 

 Write distance must be a factor of core size, otherwise the above defined behaviour is not guaranteed. 

## 4.3 Standard Variable Sets
### ICWS86
| Variable                 | Setting      |
| ---                      | ---          |
| Core Size:               | 8192         |
| Cycles Before Tie:       | 100000       |
| Initial Instruction:     | DAT.F #0, #0 |
| Instruction Limit:       | 300          |
| Maximum Number of Tasks: | 64           |
| Minimum Separation:      | 300          |
| Read Distance:           | 8192         |
| Separation:              | RANDOM       |
| Warriors:                | 2            |
| Write Distance:          | 8192         |
 

### KOTH
| Variable                 | Setting      |
| ---                      | ---          |
| Core Size:               | 8000         |
| Cycles Before Tie:       | 80000        |
| Initial Instruction:     | DAT.F $0, $0 |
| Instruction Limit:       | 100          |
| Maximum Number of Tasks: | 8000         |
| Minimum Separation:      | 100          |
| Read Distance:           | 8000         |
| Separation:              | RANDOM       |
| Warriors:                | 2            |
| Write Distance:          | 8000         |
 

# 5. MARS
 

## 5.1 Purpose
 The Memory Array Redcode Simulator (MARS) is the computer in which Core War warriors do combat. 

## 5.2 Description
 A minimally-complete MARS consists of a core, a loader, task queues, the MARS executive function, and a way to present the final results of a battle.  Additionally, many MARS provide a "real-time" battle display and various debugging tools. 

 The core consists of a cyclic list (0, 1, 2, ..., M-2, M-1, 0, 1, ...) of M MARS instructions.  The integer M is referred to as "core size". All operations are performed modulo M.  Core initialization (the initial instructions placed in core before loading warriors) is a run-time variable (see Section 4). 

 The loader loads warriors into core, converting each negative field value N to the positive field value P such that 0 <= P < M and P = kM \+ N where k is a positive integer (P = N modulo M).  Each field value G greater than or equal to M is converted to the field values L such that 0 <= L < M and L = kM + G where k is a negative integer (L = G modulo M).  The loader also initializes each warrior's task queue with the appropriate task pointer. 

 There is a task queue for each warrior loaded into core.  Each task queue can hold a limited number of task pointers.  The "task limit" (number of tasks an individual warrior's task queue can hold) is a run-time variable.  The task queues are FIFOs (First In, First Out). 

 Each warrior consists of one task initially.  Subsequent tasks are added to the warrior's task queue using the `SPL` instruction. Attempted execution of a `DAT` instruction by a task effectively removes that task from the warrior's task queue. 

 Warriors execute for a specified number of cycles ("time limit", see Section 4) or until only one warrior is still executing, whichever occurs first.  Each cycle, one instruction from each warrior is executed.  The instruction to execute is the instruction pointed to by the next task pointer in the warrior's task queue.  A warrior is no longer executing when its task queue is empty. 

 The following expressions are used in describing the MARS executive function's operation. 

* An **instruction** consists of an opcode, a modifier, an A-operand, and a B-operand.
* An **A-operand** consists of an A-mode and an A-number.
* An **A-mode** is the addressing mode of an A-operand.
* An **A-number** is an integer between 0 and M-1, inclusive.
* A **B-operand** consists of a B-mode and a B-number.
* A **B-mode** is the addressing mode of a B-operand.
* A **B-number** is an integer between 0 and M-1, inclusive.
 General Definitions: 

* The **program counter (PC)** is the pointer to the location in core of the instruction fetched from core to execute.
* The **current instruction** is the instruction in the instruction register, as copied (prior to execution) from the PC location of core.
* The **A-pointer** points to the instruction the A-operand of the current instruction references in core.
* The **A-instruction** is a copy of the instruction the A-pointer points to in core (as it was during operand evaluation).
* The **A-value** is the A-number and/or the B-number of the A-instruction or the A-instruction itself, whichever are/is selected by the opcode modifier.
* The **B-pointer** points to the instruction the B-operand of the current instruction references in core.
* The **B-instruction** is a copy of the instruction the B-pointer points to in core (as it was during operand evaluation).
* The **B-value** is the A-number and/or the B-number of the B-instruction or the B-instruction itself, whichever are/is selected by the opcode modifier.
* The **B-target** is the A-number and/or the B-number of the instruction pointed to by the B-pointer or the instruction itself, whichever are/is selected by the opcode modifier.
 Specific Definitions: 

1. The currently executing warrior's current task pointer is extracted from the warrior's task queue and assigned to the program counter.
2. The corresponding instruction is fetched from core and stored in the instruction register as the current instruction.
3. The A-operand of the current instruction is evaluated.
4. The results of A-operand evaluation, the A-pointer and the A-instruction, are stored in the appropriate registers.
5. The B-operand of the current instruction is evaluated.
6. The results of B-operand evaluation, the B-pointer and the B-instruction, are stored in the appropriate registers.
7. Operations appropriate to the opcode.modifier pair in the instruction register are executed.  With the exception of DAT instructions, all operations queue an updated task pointer. (How the task pointer is updated and when it is queued depend on instruction execution).
 All MARS instructions are executed following the same procedure: 

 All pointers are PC-relative, indicating the offset from the source of the current instruction to the desired location.  All arithmetic is to be done modulo M, with negative values converted in the same manner as during loading as discussed above (P = M + N).  Additionally, all reads of core are done modulo the read limit (R) and all writes of core are done modulo the write limit (W).  Read offsets O greater than R/2 from the current instruction are converted to backwards offsets of O = O - R.  A comparable conversion occurs for write offsets greater than W/2. 

## 5.3 Address Modes
 

### 5.3.1 Immediate
 An immediate mode operand merely serves as storage for data.  An immediate A/B-mode in the current instruction sets the A/B-pointer to zero. 

### 5.3.2 Direct
 A direct mode operand indicates the offset from the program counter. A direct A/B-mode in the current instruction means the A/B-pointer is a copy of the offset, the A/B-number of the current instruction. 

### 5.3.3 A-number Indirect
 An A-number indirect mode operand indicates the primary offset (relative to the program counter) to the secondary offset (relative to the location of the instruction in which the secondary offset is contained).  An A-number indirect A/B-mode indicates that the A/B-pointer is the sum of the A/B-number of the current instruction (the primary offset) and the A-number of the instruction pointed to by the A/B-number of the current instruction (the secondary offset). 

### 5.3.4 B-number Indirect
 A B-number indirect mode operand indicates the primary offset (relative to the program counter) to the secondary offset (relative to the location of the instruction in which the secondary offset is contained).  A B-number indirect A/B-mode indicates that the A/B-pointer is the sum of the A/B-number of the current instruction (the primary offset) and the B-number of the instruction pointed to by the A/B-number of the current instruction (the secondary offset). 

### 5.3.5 A-number Predecrement Indirect
 An A-number predecrement indirect mode operand indicates the primary offset (relative to the program counter) to the secondary offset (relative to the location of the instruction in which the secondary offset is contained) which is decremented prior to use.  An A-number predecrement indirect A/B-mode indicates that the A/B-pointer is the sum of the A/B-number of the current instruction (the primary offset) and the decremented A-number of the instruction pointed to by the A/B-number of the current instruction (the secondary offset). 

### 5.3.6 B-number Predecrement Indirect
 A B-number predecrement indirect mode operand indicates the primary offset (relative to the program counter) to the secondary offset (relative to the location of the instruction in which the secondary offset is contained) which is decremented prior to use.  A B-number predecrement indirect A/B-mode indicates that the A/B-pointer is the sum of the A/B-number of the current instruction (the primary offset) and the decremented B-number of the instruction pointed to by the A/B-number of the current instruction (the secondary offset). 

### 5.3.7 A-number Postincrement Indirect
 An A-number postincrement indirect mode operand indicates the primary offset (relative to the program counter) to the secondary offset (relative to the location of the instruction in which the secondary offset is contained) which is incremented after the results of the operand evaluation are stored.  An A-number postincrement indirect A/B-mode indicates that the A/B-pointer is the sum of the A/B-number of the current instruction (the primary offset) and the A-number of the instruction pointed to by the A/B-number of the current instruction (the secondary offset).  The A-number of the instruction pointed to by the A/B-number of the current instruction is incremented after the A/B-instruction is stored, but before the B-operand is evaluated (for A-number postincrement indirect A-mode), or the operation is executed (for A-number postincrement indirect B-mode). 

### 5.3.8 B-number Postincrement Indirect
 A B-number postincrement indirect mode operand indicates the primary offset (relative to the program counter) to the secondary offset (relative to the location of the instruction in which the secondary offset is contained) which is incremented after the results of the operand evaluation are stored.  A B-number postincrement indirect A/B-mode indicates that the A/B-pointer is the sum of the A/B-number of the current instruction (the primary offset) and the B-number of the instruction pointed to by the A/B-number of the current instruction (the secondary offset).  The B-number of the instruction pointed to by the A/B-number of the current instruction is incremented after the A/B-instruction is stored, but before the B-operand is evaluated (for B-number postincrement indirect A-mode), or the operation is executed (for B-number postincrement indirect B-mode). 

## 5.4 Modifiers
 

### 5.4.1 A
 Instruction execution proceeds with the A-value set to the A-number of the A-instruction and the B-value set to the A-number of the B-instruction.  A write to core replaces the A-number of the instruction pointed to by the B-pointer. 

 For example, a `CMP.A` instruction would compare the A-number of the A-instruction with the A-number of the B-instruction.  A `MOV.A` instruction would replace the A-number of the instruction pointed to by the B-pointer with the A-number of the A-instruction. 

### 5.4.2 B
 Instruction execution proceeds with the A-value set to the B-number of the A-instruction and the B-value set to the B-number of the B-instruction.  A write to core replaces the B-number of the instruction pointed to by the B-pointer. 

 For example, a `CMP.B` instruction would compare the B-number of the A-instruction with the B-number of the B-instruction.  A `MOV.B` instruction would replace the B-number of the instruction pointed to by the B-pointer with the B-number of the A-instruction. 

### 5.4.3 AB
 Instruction execution proceeds with the A-value set to the A-number of the A-instruction and the B-value set to the B-number of the B-instruction.  A write to core replaces the B-number of the instruction pointed to by the B-pointer. 

 For example, a `CMP.AB` instruction would compare the A-number of the A-instruction with the B-number of the B-instruction.  A `MOV.AB` instruction would replace the B-number of the instruction pointed to by the B-pointer with the A-number of the A-instruction. 

### 5.4.4 BA
 Instruction execution proceeds with the A-value set to the B-number of the A-instruction and the B-value set to the A-number of the B-instruction.  A write to core replaces the A-number of the instruction pointed to by the B-pointer. 

 For example, a `CMP.BA` instruction would compare the B-number of the A-instruction with the A-number of the B-instruction.  A `MOV.BA` instruction would replace the A-number of the instruction pointed to by the B-pointer with the B-number of the A-instruction. 

### 5.4.5 F
 Instruction execution proceeds with the A-value set to both the A-number and B-number of the A-instruction (in that order) and the B-value set to both the A-number and B-number of the B-instruction (also in that order).  A write to core replaces both the A-number and the B-number of the instruction pointed to by the B-pointer (in that order). 

 For example, a `CMP.F` instruction would compare the A-number of the A-instruction to the A-number of the B-instruction and the B-number of the A-instruction to B-number of the B-instruction.  A `MOV.F` instruction would replace the A-number of the instruction pointed to by the B-pointer with the A-number of the A-instruction and would also replace the B-number of the instruction pointed to by the B-pointer with the B-number of the A-instruction. 

### 5.4.6 X
 Instruction execution proceeds with the A-value set to both the A-number and B-number of the A-instruction (in that order) and the B-value set to both the B-number and A-number of the B-instruction (in that order).  A write to to core replaces both the B-number and the A-number of the instruction pointed to by the B-pointer (in that order). 

 For example, a `CMP.X` instruction would compare the A-number of the A-instruction to the B-number of the B-instruction and the B-number of the A-instruction to A-number of the B-instruction.  A `MOV.X` instruction would replace the B-number of the instruction pointed to by the B-pointer with the A-number of the A-instruction and would also replace the A-number of the instruction pointed to by the B-pointer with the B-number of the A-instruction. 

### 5.4.7 I
 Instruction execution proceeds with the A-value set to the A-instruction and the B-value set to the B-instruction.  A write to core replaces the entire instruction pointed to by the B-pointer. 

 For example, a `CMP.I` instruction would compare the A-instruction to the B-instruction.  A `MOV.I` instruction would replace the instruction pointed to by the B-pointer with the A-instruction. 

## 5.5 Instruction Set
 

### 5.5.1 DAT
 No additional processing takes place.  This effectively removes the current task from the current warrior's task queue. 

### 5.5.2 MOV
 Move replaces the B-target with the A-value and queues the next instruction (PC + 1). 

### 5.5.3 ADD
 `ADD` replaces the B-target with the sum of the A-value and the B-value (A-value + B-value) and queues the next instruction (PC + 1).  `ADD.I` functions as `ADD.F` would. 

### 5.5.4 SUB
 `SUB` replaces the B-target with the difference of the B-value and the A-value (B-value - A-value) and queues the next instruction (PC + 1). `SUB.I` functions as `SUB.F` would. 

### 5.5.5 MUL
 `MUL` replaces the B-target with the product of the A-value and the B-value (A-value * B-value) and queues the next instruction (PC + 1). `MUL.I` functions as `MUL.F` would. 

### 5.5.6 DIV
 `DIV` replaces the B-target with the integral result of dividing the B-value by the A-value (B-value / A-value) and queues the next instruction (PC + 1).  `DIV.I` functions as `DIV.F` would. If the A-value is zero, the B-value is unchanged and the current task is removed from the warrior's task queue.  `DIV.I`, `DIV.F`, and `DIV.X` operate on pairs of operands.  If either component of the A-value is zero, the corresponding component of the B-value is unchanged (the other component is divided normally), and the current task is removed from the warrior queue. 

### 5.5.7 MOD
 `MOD` replaces the B-target with the integral remainder of dividing the B-value by the A-value (B-value % A-value) and queues the next instruction (PC + 1).  `MOD.I` functions as `MOD.F` would. If the A-value is zero, the B-value is unchanged and the current task is removed from the warrior's task queue.  `MOD.I`, `MOD.F`, and `MOD.X` operate on pairs of operands.  If either component of the A-value is zero, the corresponding component of the B-value is unchanged (the other component is divided normally), and the current task is removed from the warrior queue. 

### 5.5.8 JMP
 `JMP` queues the sum of the program counter and the A-pointer. 

### 5.5.9 JMZ
 `JMZ` tests the B-value to determine if it is zero.  If the B-value is zero, the sum of the program counter and the A-pointer is queued. Otherwise, the next instruction is queued (PC + 1).  `JMZ.I` functions as `JMZ.F` would, i.e. it jumps if both the A-number and the B-number of the B-instruction are zero. 

### 5.5.10 JMN
 `JMN` tests the B-value to determine if it is zero.  If the B-value is not zero, the sum of the program counter and the A-pointer is queued. Otherwise, the next instruction is queued (PC + 1).  `JMN.I` functions as `JMN.F` would, i.e. it jumps if the A-number or the B-number of the B-instruction (or both) is non-zero. This is the negation of the condition for `JMZ.F`. 

### 5.5.11 DJN
 `DJN` decrements the B-value and the B-target, then tests the B-value to determine if it is zero.  If the decremented B-value is not zero, the sum of the program counter and the A-pointer is queued. Otherwise, the next instruction is queued (PC + 1).  `DJN.I` functions as `DJN.F` would, i.e. it decrements both both A/B-numbers of the B-value and the B-target, and jumps if one (or both) of the A/B-numbers of the B-value is non-zero. 

### 5.5.12 SEQ and CMP
 `SEQ` and `CMP` are synonymous opcodes.  `SEQ` is provided as an easy-to-remember mnemonic, and `CMP` is provided for backward compatibility.  They are completely equivalent.  `SEQ` (or `CMP`) compares the A-value to the B-value.  If the result of the comparison is equal, the instruction after the next instruction (PC + 2) is queued (skipping the next instruction).  Otherwise, the next instruction is queued (PC + 1). 

### 5.5.13 SNE
 `SNE` compares the A-value to the B-value.  If the result of the comparison is not equal, the instruction after the next instruction (PC + 2) is queued (skipping the next instruction).  Otherwise, the next instruction is queued (PC + 1). 

### 5.5.14 SLT
 `SLT` compares the A-value to the B-value.  If the A-value is less than the B-value, the instruction after the next instruction (PC + 2) is queued (skipping the next instruction).  Otherwise, the next instruction is queued (PC + 1).  `SLT.I` functions as `SLT.F` would, i.e. the next instruction is skipped only if each of the A/B-numbers of the A-value is less than its B-value counterpart. 

### 5.5.15 SPL
 `SPL` queues the next instruction (PC + 1) and then queues the sum of the program counter and the A-pointer. If the queue is full, only the next instruction is queued. 

### 5.5.16 NOP
 `NOP` queues the next instruction (PC + 1). 

## 5.6 Example MARS Interpreter
```
/************************************/
/*                                  */
/*            EMI94.c               */
/*                                  */
/* Execute MARS Instruction ala     */
/* ICWS'94 Draft Standard.          */
/*                                  */
/* Last Update: November 8,    */
/*                                  */
/************************************/

/* This ANSI C function is the benchmark MARS instruction   */
/* interpreter for the ICWS'94 Draft Standard.              */

/* The design philosophy of this function is to mirror the  */
/* standard as closely as possible, illuminate the meaning  */
/* of the standard, and provide the definitive answers to   */
/* questions of the "well, does the standard mean this or   */
/* that?" variety.  Although other, different implemen-     */
/* tations are definitely possible and encouraged; those    */
/* implementations should produce the same results as this  */
/* one does.                                                */

/* The function returns the state of the system.  What the  */
/* main program does with this information is not defined   */
/* by the standard.                                         */

enum SystemState {
   UNDEFINED,
   SUCCESS
};

/* Any number of warriors may be executing in core at one   */
/* time, depending on the run-time variable set and how     */
/* many warriors have failed during execution.  For this    */
/* implementation, warriors are identified by the order in  */
/* which they were loaded into core.                        */

typedef unsigned int Warrior;

/* An Address in Core War can be any number from 0 to the   */
/* size of core minus one, relative to the current          */
/* instruction.  In this implementation, core is an array   */
/* of instructions; therefore any variable types which      */
/* contain an Address can be considered to be of type       */
/* unsigned int.  One caveat: for the purposes of this      */
/* standard, unsigned int must be considered to have no     */
/* upper limit.  Depending on the size of core, it may be   */
/* necessary to take precautions against overflow.          */

typedef unsigned int Address;

/* The FIFO task queues and supporting functions are not    */
/* presented.   The function Queue() attempts to add a task */
/* pointer to the back of the currently executing warrior's */
/* task queue.  No special actions are to be taken if       */
/* Queue() is unsuccessful, which it will be if the warrior */
/* has already reached the task limit (maximum allowable    */
/* number of tasks).                                        */

extern void Queue(
   Warrior  W,
   Address  TaskPointer
);

/* There is one support function used to limit the range of */
/* reading from Core and writing to Core relative to the    */
/* current instruction.  Behaviour is as expected (a small  */
/* core within Core) only if the limits are factors of the  */
/* size of Core.                                            */

static Address Fold(
   Address  pointer,    /* The pointer to fold into the desired range.  */
   Address  limit,      /* The range limit.                             */
   Address  M           /* The size of Core.                            */
) {
   Address  result;

   result = pointer % limit;
   if ( result > (limit/2) ) {
      result += M - limit;
   };
   return(result);
}

/* Instructions are the principle data type.  Core is an    */
/* array of instructions, and there are three instruction   */
/* registers used by the MARS executive.                    */

enum Opcode {
   DAT,
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
   CMP, /* aka SEQ */
   SNE,
   SLT,
   SPL,
   NOP,
};

enum Modifier {
   A,
   B,
   AB,
   BA,
   F,
   X,
   I
};

enum Mode {
   IMMEDIATE,
   DIRECT,
   A_INDIRECT,
   B_INDIRECT,
   A_DECREMENT,
   B_DECREMENT,
   A_INCREMENT,
   B_INCREMENT,
};

typedef struct Instruction {
   enum Opcode    Opcode;
   enum Modifier  Modifier;
   enum Mode      AMode;
   Address        ANumber;
   enum Mode      BMode;
   Address        BNumber;
} Instruction;

/* The function is passed which warrior is currently        */
/* executing, the address of the warrior's current task's   */
/* current instruction, a pointer to the Core, the size of  */
/* the Core, and the read and write limits.  It returns the */
/* system's state after attempting instruction execution.   */

enum SystemState EMI94(

/* W indicates which warrior's code is executing.           */

   Warrior  W,

/* PC is the address of this warrior's current task's       */
/* current instruction.                                     */

   Address  PC,

/* Core is just an array of Instructions.  Core has been    */
/* initialized and the warriors have been loaded before     */
/* calling this function.                                   */

   Instruction Core[],

/* M is the size of Core.                                   */

   Address     M,

/* ReadLimit is the limitation on read distances.           */

   Address     ReadLimit,

/* WriteLimit is the limitation on write distances.         */

   Address     WriteLimit

) {

/* This MARS stores the currently executing instruction in  */
/* the instruction register IR.                             */

   Instruction IR;

/* This MARS stores the instruction referenced by the       */
/* A-operand in the instruction register IRA.               */

   Instruction IRA;

/* This MARS stores the instruction referenced by the       */
/* B-operand in the instruction Register IRB.               */

   Instruction IRB;

/* All four of the following pointers are PC-relative       */
/* (relative to the Program Counter).  Actual access of     */
/* core must add-in the Program Counter (mod core size).    */

/* The offset to the instruction referred to by the         */
/* A-operand for reading is Read Pointer A (RPA).           */

   Address     RPA;

/* The offset to the instruction referred to by the         */
/* A-operand for writing is Write Pointer A (WPA).          */

   Address     WPA;

/* The offset to the instruction referred to by the         */
/* B-operand for reading is Read Pointer B (RPB).           */

   Address     RPB;

/* The offset to the instruction referred to by the         */
/* A-operand for writing is Write Pointer B (WPB).          */

   Address     WPB;

/* Post-increment operands need to keep track of which      */
/* instruction to increment.                                */

   Address     PIP;

/* Before execution begins, the current instruction is      */
/* copied into the Instruction Register.                    */

   IR = Core[PC];

/* Next, the A-operand is completely evaluated.             */

/* For instructions with an Immediate A-mode, the Pointer A */
/* points to the source of the current instruction.         */

   if (IR.AMode == IMMEDIATE) {
      RPA = WPA = 0;
   } else {

/* For instructions with a Direct A-mode, the Pointer A     */
/* points to the instruction IR.ANumber away, relative to   */
/* the Program Counter.                                     */
/* Note that implementing Core as an array necessitates     */
/* doing all Address arithmetic modulus the size of Core.   */

      RPA = Fold(IR.ANumber, ReadLimit, M);
      WPA = Fold(IR.ANumber, WriteLimit, M);

/* For instructions with A-indirection in the A-operand     */
/* (A-number Indirect, A-number Predecrement,               */
/* and A-number Postincrement A-modes):                     */

      if (IR.AMode == A_INDIRECT
          || IR.AMode == A_DECREMENT
          || IR.AMode == A_INCREMENT) {

/* For instructions with Predecrement A-mode, the A-Field   */
/* of the instruction in Core currently pointed to by the   */
/* Pointer A is decremented (M - 1 is added).               */

         if (IR.AMode == A_DECREMENT) {
            Core[((PC + WPA) % M)].ANumber =
               (Core[((PC + WPA) % M)].ANumber + M - 1) % M;
         };

/* For instructions with Postincrement A-mode, the A-Field  */
/* of the instruction in Core currently pointed to by the   */
/* Pointer A will be incremented.                           */

         if (IR.AMode == A_INCREMENT) {
            PIP = (PC + WPA) % M;
         };

/* For instructions with A-indirection in the A-operand,    */
/* Pointer A ultimately points to the instruction           */
/* Core[((PC + PCA) % M)].ANumber away, relative to the     */
/* instruction pointed to by Pointer A.                     */

         RPA = Fold(
            (RPA + Core[((PC + RPA) % M)].ANumber), ReadLimit, M
         );
         WPA = Fold(
            (WPA + Core[((PC + WPA) % M)].ANumber), WriteLimit, M
         );

      };

/* For instructions with B-indirection in the A-operand     */
/* (B-number Indirect, B-number Predecrement,               */
/* and B-number Postincrement A-modes):                     */

      if (IR.AMode == B_INDIRECT
          || IR.AMode == B_DECREMENT
          || IR.AMode == B_INCREMENT) {

/* For instructions with Predecrement A-mode, the B-Field   */
/* of the instruction in Core currently pointed to by the   */
/* Pointer A is decremented (M - 1 is added).               */

         if (IR.AMode == DECREMENT) {
            Core[((PC + WPA) % M)].BNumber =
               (Core[((PC + WPA) % M)].BNumber + M - 1) % M;
         };

/* For instructions with Postincrement A-mode, the B-Field  */
/* of the instruction in Core currently pointed to by the   */
/* Pointer A will be incremented.                           */

         if (IR.AMode == INCREMENT) {
            PIP = (PC + WPA) % M;
         };

/* For instructions with B-indirection in the A-operand,    */
/* Pointer A ultimately points to the instruction           */
/* Core[((PC + PCA) % M)].BNumber away, relative to the     */
/* instruction pointed to by Pointer A.                     */

         RPA = Fold(
            (RPA + Core[((PC + RPA) % M)].BNumber), ReadLimit, M
         );
         WPA = Fold(
            (WPA + Core[((PC + WPA) % M)].BNumber), WriteLimit, M
         );

      };
   };

/* The Instruction Register A is a copy of the instruction  */
/* pointed to by Pointer A.                                 */

   IRA = Core[((PC + RPA) % M)];

/* If the A-mode was post-increment, now is the time to     */
/* increment the instruction in core.                       */

   if (IR.AMode == A_INCREMENT) {
           Core[PIP].ANumber = (Core[PIP].ANumber + 1) % M;
           }
   else if (IR.AMode == B_INCREMENT) {
           Core[PIP].BNumber = (Core[PIP].BNumber + 1) % M;
           };

/* The Pointer B and the Instruction Register B are         */
/* evaluated in the same manner as their A counterparts.    */

   if (IR.BMode == IMMEDIATE) {
      RPB = WPB = 0;
   } else {
      RPB = Fold(IR.BNumber, ReadLimit, M);
      WPB = Fold(IR.BNumber, WriteLimit, M);
      if (IR.BMode == A_INDIRECT
          || IR.BMode == A_DECREMENT
          || IR.BMode == A_INCREMENT) {
         if (IR.BMode == A_DECREMENT) {
            Core[((PC + WPB) % M)].ANumber =
               (Core[((PC + WPB) % M)].ANumber + M - 1) % M
            ;
         } else if (IR.BMode == A_INCREMENT) {
            PIP = (PC + WPB) % M;
         };
         RPB = Fold(
            (RPB + Core[((PC + RPB) % M)].ANumber), ReadLimit, M
         );
         WPB = Fold(
            (WPB + Core[((PC + WPB) % M)].ANumber), WriteLimit, M
         );
      };
      if (IR.BMode == B_INDIRECT
          || IR.BMode == B_DECREMENT
          || IR.BMode == B_INCREMENT) {
         if (IR.BMode == B_DECREMENT) {
            Core[((PC + WPB) % M)].BNumber =
               (Core[((PC + WPB) % M)].BNumber + M - 1) % M
            ;
         } else if (IR.BMode == B_INCREMENT) {
            PIP = (PC + WPB) % M;
         };
         RPB = Fold(
            (RPB + Core[((PC + RPB) % M)].BNumber), ReadLimit, M
         );
         WPB = Fold(
            (WPB + Core[((PC + WPB) % M)].BNumber), WriteLimit, M
         );
      };
   };
   IRB = Core[((PC + RPB) % M)];

   if (IR.BMode == A_INCREMENT) {
           Core[PIP].ANumber = (Core[PIP].ANumber + 1) % M;
           }
   else if (IR.BMode == INCREMENT) {
           Core[PIP].BNumber = (Core[PIP].BNumber + 1) % M;
           };

/* Execution of the instruction can now proceed.            */

   switch (IR.Opcode) {

/* Instructions with a DAT opcode have no further function. */
/* The current task's Program Counter is not updated and is */
/* not returned to the task queue, effectively removing the */
/* task.                                                    */

   case DAT: noqueue:
      break;

/* MOV replaces the B-target with the A-value and queues    */
/* the next instruction.                                    */

   case MOV:
      switch (IR.Modifier) {

/* Replaces A-number with A-number.                         */

      case A:
         Core[((PC + WPB) % M)].ANumber = IRA.ANumber;
         break;

/* Replaces B-number with B-number.                         */

      case B:
         Core[((PC + WPB) % M)].BNumber = IRA.BNumber;
         break;

/* Replaces B-number with A-number.                         */

      case AB:
         Core[((PC + WPB) % M)].BNumber = IRA.ANumber;
         break;

/* Replaces A-number with B-number.                         */

      case BA:
         Core[((PC + WPB) % M)].ANumber = IRA.BNumber;
         break;

/* Replaces A-number with A-number and B-number with        */
/* B-number.                                                */

      case F:
         Core[((PC + WPB) % M)].ANumber = IRA.ANumber;
         Core[((PC + WPB) % M)].BNumber = IRA.BNumber;
         break;

/* Replaces B-number with A-number and A-number with        */
/* B-number.                                                */

      case X:
         Core[((PC + WPB) % M)].BNumber = IRA.ANumber;
         Core[((PC + WPB) % M)].ANumber = IRA.BNumber;
         break;

/* Copies entire instruction.                               */

      case I:
         Core[((PC + WPB) % M)] = IRA;
         break;

      default:
         return(UNDEFINED);
         break;
      };

/* Queue up next instruction.                               */
      Queue(W, ((PC + 1) % M));
      break;

/* Arithmetic instructions replace the B-target with the    */
/* "op" of the A-value and B-value, and queue the next      */
/* instruction.  "op" can be the sum, the difference, or    */
/* the product.                                             */

#define ARITH(op) \
      switch (IR.Modifier) { \
      case A: \
         Core[((PC + WPB) % M)].ANumber = \
            (IRB.ANumber op IRA.ANumber) % M \
         ; \
         break; \
      case B: \
         Core[((PC + WPB) % M)].BNumber = \
            (IRB.BNumber op IRA.BNumber) % M \
         ; \
         break; \
      case AB: \
         Core[((PC + WPB) % M)].BNumber = \
            (IRB.ANumber op IRA.BNumber) % M \
         ; \
         break; \
      case BA: \
         Core[((PC + WPB) % M)].ANumber = \
            (IRB.BNumber op IRA.ANumber) % M \
         ; \
         break; \
      case F: \
      case I: \
         Core[((PC + WPB) % M)].ANumber = \
            (IRB.ANumber op IRA.ANumber) % M \
         ; \
         Core[((PC + WPB) % M)].BNumber = \
            (IRB.BNumber op IRA.BNumber) % M \
         ; \
         break; \
      case X: \
         Core[((PC + WPB) % M)].BNumber = \
            (IRB.ANumber op IRA.BNumber) % M \
         ; \
         Core[((PC + WPB) % M)].ANumber = \
            (IRB.BNumber op IRA.ANumber) % M \
         ; \
         break; \
      default: \
         return(UNDEFINED); \
         break; \
      }; \
      Queue(W, ((PC + 1) % M)); \
      break;

   case ADD: ARITH(+)
   case SUB: ARITH(+ M -)
   case MUL: ARITH(*)

/* DIV and MOD replace the B-target with the integral       */
/* quotient (for DIV) or remainder (for MOD) of the B-value */
/* by the A-value, and queues the next instruction.         */
/* Process is removed from task queue if A-value is zero.   */

#define ARITH_DIV(op) \
      switch (IR.Modifier) { \
      case A: \
         if (IRA.ANumber != 0) \
            Core[((PC + WPB) % M)].ANumber = IRB.ANumber op IRA.ANumber; \
         break; \
      case B: \
         if (IRA.BNumber != 0) \
            Core[((PC + WPB) % M)].BNumber = IRB.BNumber op IRA.BNumber; \
         else goto noqueue; \
         break; \
      case AB: \
         if (IRA.ANumber != 0) \
            Core[((PC + WPB) % M)].BNumber = IRB.BNumber op IRA.ANumber; \
         else goto noqueue; \
         break; \
      case BA: \
         if (IRA.BNumber != 0) \
            Core[((PC + WPB) % M)].ANumber = IRB.ANumber op IRA.BNumber; \
         else goto noqueue; \
         break; \
      case F: \
      case I: \
         if (IRA.ANumber != 0) \
            Core[((PC + WPB) % M)].ANumber = IRB.ANumber op IRA.ANumber; \
         if (IRA.BNumber != 0) \
            Core[((PC + WPB) % M)].BNumber = IRB.BNumber op IRA.BNumber; \
         if ((IRA.ANumber == 0) || (IRA.BNumber == 0)) \
            goto noqueue; \
         break; \
      case X: \
         if (IRA.ANumber != 0) \
            Core[((PC + WPB) % M)].BNumber = IRB.BNumber op IRA.ANumber; \
         if (IRA.BNumber != 0) \
            Core[((PC + WPB) % M)].ANumber = IRB.ANumber op IRA.BNumber; \
         if ((IRA.ANumber == 0) || (IRA.BNumber == 0)) \
            goto noqueue; \
         break; \
      default: \
         return(UNDEFINED); \
         break; \
      }; \
      Queue(W, ((PC + 1) % M)); \
      break;

   case DIV: ARITH_DIV(/)
   case MOD: ARITH_DIV(%)

/* JMP queues the sum of the Program Counter and the        */
/* A-pointer.                                               */

   case JMP:
      Queue(W, RPA);
      break;

/* JMZ queues the sum of the Program Counter and Pointer A  */
/* if the B-value is zero.  Otherwise, it queues the next   */
/* instruction.                                             */

   case JMZ:
      switch (IR.Modifier) {
      case A:
      case BA:
         if (IRB.ANumber == 0) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case B:
      case AB:
         if (IRB.BNumber == 0) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case F:
      case X:
      case I:
         if ( (IRB.ANumber == 0) && (IRB.BNumber == 0) ) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      default:
         return(UNDEFINED);
         break;
      };
      break;

/* JMN queues the sum of the Program Counter and Pointer A  */
/* if the B-value is not zero.  Otherwise, it queues the    */
/* next instruction.                                        */

   case JMN:
      switch (IR.Modifier) {
      case A:
      case BA:
         if (IRB.ANumber != 0) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case B:
      case AB:
         if (IRB.BNumber != 0) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case F:
      case X:
      case I:
         if ( (IRB.ANumber != 0) || (IRB.BNumber != 0) ) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      default:
         return(UNDEFINED);
         break;
      };
      break;

/* DJN (Decrement Jump if Not zero) decrements the B-value  */
/* and the B-target, then tests if the B-value is zero.  If */
/* the result is not zero, the sum of the Program Counter   */
/* and Pointer A is queued.  Otherwise, the next            */
/* instruction is queued.                                   */

   case DJN:
      switch (IR.Modifier) {
      case A:
      case BA:
         Core[((PC + WPB) % M)].ANumber =
            (Core[((PC + WPB) % M)].ANumber + M - 1) % M
         ;
         IRB.ANumber -= 1;
         if (IRB.ANumber != 0) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case B:
      case AB:
         Core[((PC + WPB) % M)].BNumber =
            (Core[((PC + WPB) % M)].BNumber + M - 1) % M
         ;
         IRB.BNumber -= 1;
         if (IRB.BNumber != 0) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case F:
      case X:
      case I:
         Core[((PC + WPB) % M)].ANumber =
            (Core[((PC + WPB) % M)].ANumber + M - 1) % M
         ;
         IRB.ANumber -= 1;
         Core[((PC + WPB) % M)].BNumber =
            (Core[((PC + WPB) % M)].BNumber + M - 1) % M
         ;
         IRB.BNumber -= 1;
         if ( (IRB.ANumber != 0) || (IRB.BNumber != 0) ) {
            Queue(W, RPA);
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      default:
         return(UNDEFINED);
         break;
      };
      break;

/* SEQ/CMP compares the A-value and the B-value. If there   */
/* are no differences, then the instruction after the next  */
/* instruction is queued.  Otherwise, the next instrution   */
/* is queued.                                               */

   case CMP:
      switch (IR.Modifier) {
      case A:
         if (IRA.ANumber == IRB.ANumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case B:
         if (IRA.BNumber == IRB.BNumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case AB:
         if (IRA.ANumber == IRB.BNumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case BA:
         if (IRA.BNumber == IRB.ANumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case F:
         if ( (IRA.ANumber == IRB.ANumber) &&
              (IRA.BNumber == IRB.BNumber)
         ) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case X:
         if ( (IRA.ANumber == IRB.BNumber) &&
              (IRA.BNumber == IRB.ANumber)
         ) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case I:
         if ( (IRA.Opcode == IRB.Opcode) &&
              (IRA.Modifier == IRB.Modifier) &&
              (IRA.AMode == IRB.AMode) &&
              (IRA.ANumber == IRB.ANumber) &&
              (IRA.BMode == IRB.BMode) &&
              (IRA.BNumber == IRB.BNumber)
         ) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      default:
         return(UNDEFINED);
         break;
      };
      break;

/* SNE compares the A-value and the B-value. If there       */
/* is a difference, then the instruction after the next     */
/* instruction is queued.  Otherwise, the next instrution   */
/* is queued.                                               */

   case SNE:
      switch (IR.Modifier) {
      case A:
         if (IRA.ANumber != IRB.ANumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case B:
         if (IRA.BNumber != IRB.BNumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case AB:
         if (IRA.ANumber != IRB.BNumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case BA:
         if (IRA.BNumber != IRB.ANumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case F:
         if ( (IRA.ANumber != IRB.ANumber) ||
              (IRA.BNumber != IRB.BNumber)
         ) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case X:
         if ( (IRA.ANumber != IRB.BNumber) ||
              (IRA.BNumber != IRB.ANumber)
         ) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case I:
         if ( (IRA.Opcode != IRB.Opcode) ||
              (IRA.Modifier != IRB.Modifier) ||
              (IRA.AMode != IRB.AMode) ||
              (IRA.ANumber != IRB.ANumber) ||
              (IRA.BMode != IRB.BMode) ||
              (IRA.BNumber != IRB.BNumber)
         ) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      default:
         return(UNDEFINED);
         break;
      };
      break;

/* SLT (Skip if Less Than) queues the instruction after the */
/* next instruction if A-value is less than B-value.        */
/* Otherwise, the next instruction is queued.  Note that no */
/* value is less than zero because only positive values can */
/* be represented in core.                                  */

   case SLT :
      switch (IR.Modifier) {
      case A:
         if (IRA.ANumber < IRB.ANumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case B:
         if (IRA.BNumber < IRB.BNumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case AB:
         if (IRA.ANumber < IRB.BNumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case BA:
         if (IRA.BNumber < IRB.ANumber) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case F:
      case I:
         if ( (IRA.ANumber < IRB.ANumber) &&
              (IRA.BNumber < IRB.BNumber)
         ) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      case X:
         if ( (IRA.ANumber < IRB.BNumber) &&
              (IRA.BNumber < IRB.ANumber)
         ) {
            Queue(W, ((PC + 2) % M));
         } else {
            Queue(W, ((PC + 1) % M));
         };
         break;
      default:
         return(UNDEFINED);
         break;
      };
      break;

/* SPL queues the next instruction and also queues the sum  */
/* of the Program Counter and Pointer A.                    */

   case SPL:
      Queue(W, ((PC + 1) % M));
      Queue(W, RPA);
      break;

/* NOP queues the next instruction.                         */

   case NOP:
      Queue(W, ((PC + 1) % M));
      break;

/* Any other opcode is undefined.                           */

   default:
      return(UNDEFINED);
   };

/* We are finished.                                         */

   return(SUCCESS);
}
```
 

# 6. Validation Suite
 

## 6.1 Purpose and Requirements
 This validation suite exists to help developers test the compatibility of their Core War systems with the requirements set up in this standard. 

## 6.2 Assembly To Load File Test
 

## 6.3 MARS tests
 

### 6.3.1   DAT Tests
### 6.3.2   MOV Tests
### 6.3.3   ADD Tests
### 6.3.4   SUB Tests
### 6.3.5   MUL Tests
### 6.3.6   DIV Tests
### 6.3.7   MOD Tests
### 6.3.8   JMP Tests
### 6.3.9   JMZ Tests
### 6.3.10  JMN Tests
### 6.3.11  DJN Tests
### 6.3.12  SEQ/CMP Tests
### 6.3.13  SNE Tests
### 6.3.14  SLT Tests
### 6.3.15  SPL Tests
### 6.3.16  NOP Tests
 

# 7. Glossary and Index
| Term | Definition |
| --- | --- |
| alphanumeric | Any of the characters A-Za-z0-9 and the underscore. |
| assembly file | A file containing Redcode instructions. |
| battle | A contest between two or more warriors. |
| core size | See section 4.2 |
| Core War | A game in which programs compete for control of a computer called a Memory Array Redcode Simulator. |
| Core Wars | More than one game of Core War. |
| cycle | See section 4.2 |
| Dwarf | See sections 2.7 and 3.6 |
| initial instruction See section 4.2 |
| instruction | A line of Redcode or object code indicating an action for MARS to execute. |
| instruction limit See section 4.2 |
| loader | A program or that part of a program which loads warriors into a MARS. |
| load file | A file containing a warrior's instructions in an assembled format. | Any MARS program can be used with any and all Redcode assemblers which produce load files, allowing customized Core War systems. |
| MARS | An acronym for Memory Array Redcode Simulator. | The computer in which Core War warriors run. |
| newline | A linefeed, carriage-return, or combination of linefeed and carriage-return. | Whichever newline is native to the host operating system. |
| object code | The internal representation of a MARS instruction. |
| read distance | See section 4.2 |
| Redcode | The assembly language of Core War. |
| tournament | A series of battles in which points, based upon the degree of success, are awarded for each battle and accumulated by each warrior (or programmer, depending upon the type of tournament). |
| warrior | A Redcode program. |
| whitespace | The space and tab characters. |
| write distance | See section 4.2 |
 

# A. Differences Between Standards
 

## A.1 Purpose
 This appendix lists some of the major differences between this standard and those standards which preceded it.  The purpose is to help those who are familiar with a previous standard or standards to quickly understand those items which are new or have changed. 

## A.2 Changes
 

### A.2.1 Assembly Files
 A comma is required for operand separation. 

 Parenthetical expressions are allowed. 

 There is a new pseudo-opcode, ORG, for specifying the first logical instruction. 

 There is a new operator, modulus '%', for determining the remainder of integer division. 

#### A.2.1.1 ICWS'86 to ICWS'94 Conversion
 If a modifier is missing, it is assembled according to conversion rules that depend on whether the ICWS'86 or '88 standard is emulated. By default, a MARS should use the ICWS'88 conversion rules. Emulation of ICWS'86 is optional. 

    Opcode                     A-mode     B-mode     modifier
    ---------------------------------------------------------
    DAT                        #$@<>*{}   #$@<>*{}   F
    MOV,CMP,SEQ,SNE            #          #$@<>*{}   AB
                               $@<>*{}    #          B
                               $@<>*{}    $@<>*{}    I
    ADD,SUB,MUL,DIV,MOD        #          #$@<>*{}   AB
                               $@<>*{}    #$@<>*{}   B
    SLT                        #          #$@<>*{}   AB
                               $@<>*{}    #$@<>*{}   B
    JMP,JMZ,JMN,DJN,SPL,NOP    #$@<>*{}   #$@<>*{}   B
    ---------------------------------------------------------
 

#### A.2.1.2 ICWS'88 to ICWS'94 Conversion
 The default modifier for ICWS'88 emulation is determined according to the table below. 

    Opcode                     A-mode     B-mode     modifier
    ---------------------------------------------------------
    DAT                        #$@<>*{}   #$@<>*{}   F
    MOV,CMP,SEQ,SNE            #          #$@<>*{}   AB
                               $@<>*{}    #          B
                               $@<>*{}    $@<>*{}    I
    ADD,SUB,MUL,DIV,MOD        #          #$@<>*{}   AB
                               $@<>*{}    #          B
                               $@<>*{}    $@<>*{}    F
    SLT                        #          #$@<>*{}   AB
                               $@<>*{}    #$@<>*{}   B
    JMP,JMZ,JMN,DJN,SPL,NOP    #$@<>*{}   #$@<>*{}   B
    ---------------------------------------------------------
 

### A.2.2 Load Files
 A load file format is specified for the first time.  (An object code did exist for ICWS'86). 

### A.2.3 MARS
 There are no illegal instructions. 

 The following addressing modes have been added: A-number indirect, A-number predecrement, A-number postincrement, and B-number postincrement. 

 `MUL`, `DIV`, `MOD`, `SNE`, and `NOP` have been added. `SEQ` is an alias for `CMP`. 

 Opcode modifiers have been added. 

