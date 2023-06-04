.assembly extern System.Runtime { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }
.assembly extern System.Console { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }
.assembly extern PSILib { .ver 1:0:0:0 }
.assembly Comp5 { .ver 0:0:0:0 }

.class Program {
    .field static int32 i
    .field static int32 j
  .method static void Main () {
    .entrypoint
    ldc.i4 1
    stsfld int32 Program::i
    br IL_0002
  IL_0001:
    ldc.i4 1
    stsfld int32 Program::j
    br IL_0004
  IL_0003:
    ldc.i4 42
    call void [System.Console]System.Console::Write (char)
    ldsfld int32 Program::j
    ldc.i4 1
    add
    stsfld int32 Program::j
  IL_0004:
    ldsfld int32 Program::j
    ldsfld int32 Program::i
    cgt
    ldc.i4.0
    ceq
    brtrue IL_0003
    ldstr ""
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    ldsfld int32 Program::i
    ldc.i4 1
    add
    stsfld int32 Program::i
  IL_0002:
    ldsfld int32 Program::i
    ldc.i4 11
    clt
    brtrue IL_0001
    ldc.i4 0
    stsfld int32 Program::i
  IL_0005:
    ldc.i4 43
    call void [System.Console]System.Console::Write (char)
    ldsfld int32 Program::i
    ldc.i4 1
    add
    stsfld int32 Program::i
    ldsfld int32 Program::i
    ldc.i4 10
    ceq
    brfalse IL_0005
    ldstr ""
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    ret
  }
}
