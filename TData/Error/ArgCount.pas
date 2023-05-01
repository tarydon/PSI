{ Number of parameters to a function should match }
program ArgCount;
var 
  a: integer;
  f: real;

function Min (i, j: integer) : integer;
begin
  if i < j then Min := i; else Min := j; 
end;

begin
  a := Min (3, 5);
  f := Atan2 (3.5, 4);
  a := Min (4, 5, 6);
end.