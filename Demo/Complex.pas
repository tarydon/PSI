program Complex;

label
   j1, j2, j3;

var
   i, j, k: integer;
   a, b: real;   

begin
   i := 12 + 3;

   if i < 12 then 
      i := 12;
   else
      i := 13;

   while j < 20 do begin
      k := k + j;
      j := j - 1;
   end;

   for i := 1 to 20 do 
      a := a + b;
end.