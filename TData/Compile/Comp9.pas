program Comp9;
var 
  i, calls: integer;
  
function Fibo (n: integer) : integer;
var
  result: integer;
begin
  calls := calls + 1;
  if n < 3 then 
    result := 1;
  else 
    result := Fibo (n - 1) + Fibo (n - 2);
  Fibo := result;
end;
  
begin
  i := 1;
  repeat
    calls := 0;
    WriteLn ("Fibo(", i, ") = ", Fibo (i), ". Calls = ", calls);
	 i := i + 1;
  until i = 21;
end.
