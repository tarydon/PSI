program Comp5;
var 
  g: integer;

function Minimum (a, b: integer) : integer;
var
  result: integer;
begin
  if a < b then 
     result := a;
  else 
     result := b;
  Minimum := result;
end
  
begin
  g := Minimum (13, 5);
  WriteLn ("Minimum (13, 5) = ", g);
  if 3 < 4 then begin
     WriteLn ("Three is less than four.");
	  WriteLn ("But you know that already.");
  end;
end.
