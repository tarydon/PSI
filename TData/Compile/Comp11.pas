program Comp11;

procedure MPrint (ch: char; n: integer);
var
  i: integer;
begin
  i := 1;
  repeat 
    Write (ch);
	 i := i + 1;
  until i > n;
  WriteLn ("");
end;

begin
  ClrScr ();
  MPrint ('+', 10);
  MPrint ('*', 5);
end.