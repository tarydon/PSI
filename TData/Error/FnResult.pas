{ Function name must be assigned to }
program FnResult;
var
  i: integer;

function TestFunc (n:integer): boolean;
begin
  if n < 1 then
    WriteLn ("Negative");
end;

begin
  WriteLn (TestFunc (12));
end.