program Comp5;
var 
  i, j: integer;
  
begin
  if 2 < 3 then WriteLn ("2 < 3");
  if 3 <= 4 then WriteLn ("3 <= 4");
  if 2 = 2 then WriteLn ("2 = 2");
  if 2 <> 3 then WriteLn ("2 <> 3");
  if 4 > 3 then WriteLn ("4 > 3");
  if 4 >= 3 then WriteLn ("4 >= 3");
  
  if 3 < 2 then WriteLn ("3 < 2 ???");
  if 4 <= 3 then WriteLn ("4 <= 3 ???");
  if 2 = 3 then WriteLn ("2 = 3 ???");
  if 2 <> 2 then WriteLn ("2 <> 2 ???");
  if 3 > 4 then WriteLn ("3 > 4 ???");
  if 3 >= 4 then WriteLn ("3 >= 4 ???");  
  
  if (2 < 3) and (3 < 4) then WriteLn ("2 < 3 and 3 < 4");
  if (2 < 3) and (4 < 3) then WriteLn ("2 < 3 and 4 < 3 ???");
  if (2 < 3) or (4 < 3) then WriteLn ("2 < 3 or 4 < 3");
  if (3 < 2) or (4 < 3) then WriteLn ("3 < 2 or 4 < 3 ???");
  if not (3 < 2) then WriteLn ("not 3 < 2");
  if not (2 < 3) then WriteLn ("not 2 < 3 ???");
end.
