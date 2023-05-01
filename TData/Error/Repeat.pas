{ Repeat statement condition should be bool }
program Repeat1;
var 
  i: integer;
  msg: string;

begin
  i := 0;
  repeat 
    i := i + 1;
  until i > 11;

  msg := "Hello";
  repeat
    i := i - 1;
  until msg;
end.