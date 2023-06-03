.assembly extern System.Runtime { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }
.assembly extern System.Console { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }
.assembly extern PSILib { .ver 1:0:0:0 }
.assembly Comp4 { .ver 0:0:0:0 }

.class Program {
    .field static int32 i
    .field static int32 j
  .method static void Main () {
    .entrypoint
    ldc.i4 2
    ldc.i4 3
    clt
    brfalse IL_0002
  IL_0001:
    ldstr "2 < 3"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0003
  IL_0002:
  IL_0003:
    ldc.i4 3
    ldc.i4 4
    cgt
    ldc.i4.0
    ceq
    brfalse IL_0005
  IL_0004:
    ldstr "3 <= 4"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0006
  IL_0005:
  IL_0006:
    ldc.i4 2
    ldc.i4 2
    ceq
    brfalse IL_0008
  IL_0007:
    ldstr "2 = 2"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0009
  IL_0008:
  IL_0009:
    ldc.i4 2
    ldc.i4 3
    ceq
    ldc.i4.0
    ceq
    brfalse IL_0011
  IL_0010:
    ldstr "2 <> 3"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0012
  IL_0011:
  IL_0012:
    ldc.i4 4
    ldc.i4 3
    cgt
    brfalse IL_0014
  IL_0013:
    ldstr "4 > 3"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0015
  IL_0014:
  IL_0015:
    ldc.i4 4
    ldc.i4 3
    clt
    ldc.i4.0
    ceq
    brfalse IL_0017
  IL_0016:
    ldstr "4 >= 3"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0018
  IL_0017:
  IL_0018:
    ldc.i4 3
    ldc.i4 2
    clt
    brfalse IL_0020
  IL_0019:
    ldstr "3 < 2 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0021
  IL_0020:
  IL_0021:
    ldc.i4 4
    ldc.i4 3
    cgt
    ldc.i4.0
    ceq
    brfalse IL_0023
  IL_0022:
    ldstr "4 <= 3 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0024
  IL_0023:
  IL_0024:
    ldc.i4 2
    ldc.i4 3
    ceq
    brfalse IL_0026
  IL_0025:
    ldstr "2 = 3 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0027
  IL_0026:
  IL_0027:
    ldc.i4 2
    ldc.i4 2
    ceq
    ldc.i4.0
    ceq
    brfalse IL_0029
  IL_0028:
    ldstr "2 <> 2 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0030
  IL_0029:
  IL_0030:
    ldc.i4 3
    ldc.i4 4
    cgt
    brfalse IL_0032
  IL_0031:
    ldstr "3 > 4 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0033
  IL_0032:
  IL_0033:
    ldc.i4 3
    ldc.i4 4
    clt
    ldc.i4.0
    ceq
    brfalse IL_0035
  IL_0034:
    ldstr "3 >= 4 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0036
  IL_0035:
  IL_0036:
    ldc.i4 2
    ldc.i4 3
    clt
    ldc.i4 3
    ldc.i4 4
    clt
    and
    brfalse IL_0038
  IL_0037:
    ldstr "2 < 3 and 3 < 4"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0039
  IL_0038:
  IL_0039:
    ldc.i4 2
    ldc.i4 3
    clt
    ldc.i4 4
    ldc.i4 3
    clt
    and
    brfalse IL_0041
  IL_0040:
    ldstr "2 < 3 and 4 < 3 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0042
  IL_0041:
  IL_0042:
    ldc.i4 2
    ldc.i4 3
    clt
    ldc.i4 4
    ldc.i4 3
    clt
    or
    brfalse IL_0044
  IL_0043:
    ldstr "2 < 3 or 4 < 3"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0045
  IL_0044:
  IL_0045:
    ldc.i4 3
    ldc.i4 2
    clt
    ldc.i4 4
    ldc.i4 3
    clt
    or
    brfalse IL_0047
  IL_0046:
    ldstr "3 < 2 or 4 < 3 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0048
  IL_0047:
  IL_0048:
    ldc.i4 3
    ldc.i4 2
    clt
    ldc.i4.0
    ceq
    brfalse IL_0050
  IL_0049:
    ldstr "not 3 < 2"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0051
  IL_0050:
  IL_0051:
    ldc.i4 2
    ldc.i4 3
    clt
    ldc.i4.0
    ceq
    brfalse IL_0053
  IL_0052:
    ldstr "not 2 < 3 ???"
    call void [System.Console]System.Console::Write (string)
    call void [System.Console]System.Console::WriteLine ()
    br IL_0054
  IL_0053:
  IL_0054:
    ret
  }
}
