program Complex;
const
   PI = 3.14;
   prompt = "Hello";
   Count = 5;
   Zero = 0;

var
   i, j, k: integer;
   a, b, f: real;
   name: string;

function Fibo (fn: integer) : integer;
var
   fi: integer;

begin
   if fn <= 2 then Fibo := 1;
   else Fibo := Fibo (fn - 2) + Fibo (fn - 1);
end;


procedure TestFibo (max: integer);
var
   tf: integer;
begin
   for tf := 1 to max do begin
      writeln ("Fibo(", tf, ") = ", Fibo (tf));
   end;
end;

function F1 (): real;
var
 x, y : real;
 
 function F2 (n : integer) : integer;
 var x, y : integer;
 function F3 () : integer;
   begin
      x := 3;
      F2 := n;
   end;
   
  begin
    y := Count;
    F3 ();
  end;
  
begin
  x := 5;
  F1 := F2 (6) * sin (pi / 8.0);
end;

procedure Greeter (msg: string);
var
   name: string;
begin
   write ("Enter your name: ");
   read (name);
   write (prompt, ", ", name, ". ", msg);
end


begin
   a := Zero;
   read (i, k, b);
   writeln (sin (-i));
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
   writeln ("F1 () = ", F1 ());
   Greeter ("Have a good day!");
end.