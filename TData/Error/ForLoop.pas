{ Tests for For Loop }
program ForLoop;
var 
  i, j: integer;
  a: real;

begin
  for i := 1 to 20 do begin
    WriteLn (i);
  end;
  for j := 20 downto 1 do 
    WriteLn (j);
  for a := 1 to 20 do
    WriteLn (a);
end.