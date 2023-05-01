{ Variable must be assigned before it's used }
program Assign1;
var 
  i, j, k: integer;

begin
  Read (i);      { i should now be 'assigned' }
  j := 12;
  WriteLn (i, j, k);
end.