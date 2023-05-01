{ While loop condition should be bool }
program WhileLoop;
var 
  i: integer;
  msg: string;

begin 
  i := 0;
  while i < 10 do begin
    i := i + 1;
    WriteLn (i);
  end;

  msg := 'C';
  while msg do begin
    WriteLn (msg);
  end;
end.