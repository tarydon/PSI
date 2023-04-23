program Complex;
var
   i, j, k: integer;
   a, b: real;
   name: string;


function Fibo (fn: integer) : integer;
var
   fi: integer;
begin
   if fn <= 2 then Fibo := 1;
   else Fibo := Fibo (n - 2) + Fibo (n - 1);
end;


procedure TestFibo (max: integer);
var
   tf: integer;
begin
   for tf := 1 to max do begin
      writeln ("Fibo(", tf, ") = ", Fibo (tf));
   end;
end;


procedure Greeter (msg: string);
var
   name: string;
begin
   write ("Enter your name: ");
   read (name);
   write ("Hello, ", name, ". ", msg);
end;


begin
   i := 12 + 3;

   if i < 12 then 
      i := 12;
   else 
      j := 13;

   while j < 20 do begin
      k := k + j;
      j := j - 1;
   end;

   for i := 1 to 20 do 
      a := a + b;

   repeat 
      writeln ("Hello");
      j := j - 1;
   until j <= 0;

   TestFibo (5);
   Greeter ("Have a good day!");
end.