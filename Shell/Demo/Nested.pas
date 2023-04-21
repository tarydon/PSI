program Nested;
var
   i: integer;

procedure PrintNum ();
var 
   j: integer;

   procedure PrintNum1 ();
   var
      k:integer;

   begin
   k := 30;
   Writeln(k);
   end;

begin
   j := 20;
   Writeln(j);
   PrintNum1 ();
end;

begin
   i := 10;
   Writeln(i);
   PrintNum ();
end.