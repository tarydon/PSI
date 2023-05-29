program Comp7;
var 
  i, j: integer;
  
begin
  i := 1;
  while i < 11 do begin
     j := 1;
	  while j <= i do begin 
	    Write ('*');
		 j := j + 1;
	  end;
	  WriteLn ("");
	  i := i + 1;
  end;
  
  i := 0;
  repeat
     Write ('+');
	  i := i + 1;
  until i = 10;
  WriteLn ("");
end.
