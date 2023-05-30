.assembly extern System.Runtime { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }
.assembly extern System.Console { .publickeytoken = (B0 3F 5F 7F 11 D5 0A 3A) .ver 7:0:0:0 }
.assembly extern PSILib { .ver 1:0:0:0 }
.assembly Hello { .ver 0:0:0:0 }

.class Program {
  .method static void Main () {
    .entrypoint
    .line 3,3 : 5,10 'P:/TData/Compile/Comp0.pas'
    ldstr "Hello, World!"
    call void [System.Console]System.Console::WriteLine (string)
    ret
  }
}
