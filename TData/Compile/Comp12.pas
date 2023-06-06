program Comp12;
const
  max = 100000;
var 
  i: integer;
  count: integer;  
  done: boolean;

function IsPrime (n: integer): boolean;
var
  limit: real;
  numer: integer;
  prime: boolean;
  done: boolean;
begin
  prime := true;
  done := false;
  numer := 2;
  limit := Sqrt (n + 0.1);
  while not done do begin
    if n mod numer = 0 then begin
      prime := false;
      done := true;
    end;
    numer := numer + 1;
    if numer > limit then 
      done := true;
  end;
  IsPrime := prime;
end
  
begin
  i := 1;
  count := 0; 
  done := false;
  while not done do begin 
    i := i + 1;
    if IsPrime (i) then begin
      count := count + 1;
      if count = max then begin
        WriteLn ("Prime(", max, ") = ", i);
        done := true;
      end;       
    end;
  end;
end.
