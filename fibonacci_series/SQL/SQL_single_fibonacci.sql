SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
SET ANSI_NULLS ON;

--Tested in Visual Studio Code using https://sqliteonline.com/
-- 1) Select Demo Server: MS SQL
-- 2) Click "Click to Connect", then Ok
-- 3) Copy this query into the query area
-- 3) Click "Run"

--change this value to the highest desired fib(n)
-- Maximum without overflow for BIGINT is fib(92)
DECLARE @nth INT = 92;
DECLARE @zero BIGINT = 0;
DECLARE @one BIGINT = 1;

--return fib(0) through the nth value
WITH
   fibCTE(n, fib, priorFib)
   AS
   (
      SELECT 0 as n, @ZERO as fib, @ZERO as priorFib
      UNION
         SELECT 1 as n, @ONE as fib, @ZERO as priorFib
      UNION ALL
         SELECT n + 1,
            fib + priorFib,
            fib
         FROM fibCTE f
         WHERE n > 0 AND n < @nth
   )

SELECT TOP 1
   n, fib
FROM fibCTE
ORDER BY n DESC;
