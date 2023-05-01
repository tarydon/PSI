{ Type of parameters to a function should match }
program ArgType;
var 
  f: real;

begin
  f := Atan2 (3, 4);
  f := Sin (true);
end.