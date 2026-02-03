--remove "/* ... */" commaents to run with the SQLite extension
/*
--Tested in Visual Studio Code using SQLite extension by alexcvzz
-- Does not need a real db, choose memory

--change this value to the highest desired fib(n)
create table nth as select 100;

--return only the nth value
with recursive fibCTE(n, fib, prior) as
     (select 0 as n, 0 as fib, 0 as prior
     union 
     select 1 as n, 1 as fib, 0 as prior
     union
     select n + 1,
        fib + prior,
        fib
     from fibCTE f
     where n > 0 and n < (select * from nth limit 1)
    )

select n, fib
from fibCTE
order by n desc
limit 1;

--return fib(0) through the nth value
with recursive fibCTE(n, fib, prior) as
     (select 0 as n, 0 as fib, 0 as prior
     union 
     select 1 as n, 1 as fib, 0 as prior
     union
     select n + 1,
        fib + prior,
        fib
     from fibCTE f
     where n > 0 and n < (select * from nth limit 1)
    )

select n, fib
from fibCTE
order by n;
*/