program Multiple;

procedure GetMin (i,j: integer; msg: string);
var
   a,b,c:integer;
begin 
  Writeln (msg);
  if (i < j) then write (i) else write(j);
  read (a,b,c);
end;

begin
  GetMin (3,2, "Smallest number is");
end.