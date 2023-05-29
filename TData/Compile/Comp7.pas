program Comp4;
var 
  g: integer;

function Multiply (a, b: integer) : integer;
var
  result: integer;
begin
  result := a * b;
  Multiply := result;
end
  
begin
  g := Multiply (5, 13);
  WriteLn ("5 x 13 = ", g);
end.
