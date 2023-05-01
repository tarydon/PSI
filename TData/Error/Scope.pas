{ Scoping of variables }
program Scope;
var 
  a: integer;
  b: integer;

procedure Test ();
var 
  a: string;
begin
  b := 12;
  a := 4;
end;

begin
end.

