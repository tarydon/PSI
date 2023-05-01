{ If condition should be of boolean type }
program IfThen;
var
  i, j: real;

begin
  i := 10; j := 15;
  if i <> j then 
    WriteLn ("i < j");
  else
    WriteLn ("j > i");

  if i then 
    WriteLn ("i < j");
end.