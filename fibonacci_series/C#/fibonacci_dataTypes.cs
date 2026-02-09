#nullable enable

//Tested using "dotnet script" in Visual Studio Code
///<link href="https://www.hanselman.com/blog/c-and-net-core-scripting-with-the-dotnetscript-global-tool" />

using System.Reflection;
using System.Numerics;
using System.Diagnostics;

/// <summary>
/// Demonstration computing Fibonacci numbers using various data types.
/// </summary>
while (true)
{
    Console.WriteLine(string.Empty);
    Console.WriteLine("Prefix the requested sequence with 'r' to use optimized recursion (e.g., 'r50' for recursive Fib(50)).");
    Console.WriteLine("Prefix with 't' to include timing. (e.g., 't50' for timed Fib(50)).");
    Console.WriteLine("Prefix with 'rt' to include recursive timing. (e.g., 'rt50' for timed recursive Fib(50)).");

    Console.WriteLine(string.Empty);
    Console.Write("Fibonacci Sequence: ");
    var read = Console.ReadLine();
    var recurse = read?.StartsWith("r", StringComparison.OrdinalIgnoreCase) == true;
    read = recurse ? read?[1..] : read;
    var timed = read?.StartsWith("t", StringComparison.OrdinalIgnoreCase) == true;
    read = timed ? read?[1..] : read;
    if (!int.TryParse(read, out var n) || n < 0)
    {
        Console.WriteLine("Invalid input, exiting...");
        return;
    }

    if (recurse)
        Console.WriteLine("If it occurs, a System.StackOverflowException means the call stack ran out of memory due to an excessive number of nested method calls.");

    Fibonacci<sbyte>(n, recurse, timed);    //Int8
    Fibonacci<byte>(n, recurse, timed);     //UInt8
    Fibonacci<short>(n, recurse, timed);    //Int16
    Fibonacci<ushort>(n, recurse, timed);   //UInt16
    Fibonacci<int>(n, recurse, timed);      //Int32
    Fibonacci<uint>(n, recurse, timed);     //UInt32
    Fibonacci<long>(n, recurse, timed);     //Int64
    Fibonacci<ulong>(n, recurse, timed);    //UInt64
    Fibonacci<Int128>(n, recurse, timed);
    Fibonacci<UInt128>(n, recurse, timed);
    Fibonacci<BigInteger>(n, recurse, timed);
}

static void Fibonacci<T>(int n, bool recurse, bool timed) where T : INumber<T>
{
    var typeNamePadded = typeof(T).Name.PadRight(12);
    try
    {
        T typeMaxValue = MaxValueHelper<T>.GetMaxValue();
        if (timed)
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();
            T fibOfN = recurse
                ? FibonacciRecurse<T>(n, typeMaxValue).Item1
                : FibonacciProgressiveLoop<T>(n, typeMaxValue);
            stopWatch.Stop();
            Console.WriteLine($"{typeNamePadded}\t{fibOfN} ({stopWatch.ElapsedTicks} ticks)");
        }
        else
        {
            if (recurse)
            {
                Console.WriteLine($"{typeNamePadded}\t{FibonacciRecurse<T>(n, typeMaxValue).Item1}");
            }
            else
            {
                Console.WriteLine($"{typeNamePadded}\t{FibonacciProgressiveLoop<T>(n, typeMaxValue)}");
            }
        }
    }
    catch (OverflowException ex)
    {
        Console.WriteLine($"{typeNamePadded}\t{ex.Message}");
    }
}

/// <summary>
/// Optimized to return current and prior values as (Item1: Fib(n), Item2: Fib(n-1)) for a single recursion tree.
/// </summary>
static (T, T) FibonacciRecurse<T>(int n, T typeMaxValue) where T : INumber<T>
{
    if (n == 0)
    {
        return (T.Zero, T.Zero);
    }
    else if (n == 1)
    {
        return (T.One, T.Zero);
    }
    else
    {
        var (fibMinus1, fibMinus2) = FibonacciRecurse<T>(n - 1, typeMaxValue);
        if (fibMinus1 > (typeMaxValue - fibMinus2))
        {
            throw new OverflowException($"Exceeded{(typeMaxValue is BigInteger ? " artificial" : string.Empty)} capacity of {typeMaxValue}, maximum that can be computed is Fib({n - 1}).");
        }
        return (fibMinus1 + fibMinus2, fibMinus1);
    }
}

/// <summary>
/// Progressive loop implementation.
/// </summary>
static T FibonacciProgressiveLoop<T>(int n, T typeMaxValue) where T : INumber<T>
{
    if (n == 0)
    {
        return T.Zero;
    }
    else if (n == 1)
    {
        return T.One;
    }
    else
    {
        T prior = T.Zero, current = T.One;
        for (int i = 2; i <= n; i++)
        {
            if (current > (typeMaxValue - prior))
            {
                throw new OverflowException($"Exceeded{(typeMaxValue is BigInteger ? " artificial" : string.Empty)} capacity of {typeMaxValue}, maximum that can be computed is Fib({i - 1}).");
            }
            T temp = prior + current;
            prior = current;
            current = temp;
        }
        return current;
    }
}

/// <summary>
/// Helper to get the MaxValue of a numeric type T since the implementation varies wiithin the framework.
/// </summary>
/// <typeparam name="T">Numeric type implementing INumber<></typeparam>
static class MaxValueHelper<T> where T : INumber<T>
{
    private static readonly T? TypeCompare = default;
    public static T GetMaxValue()
    {
        if (TypeCompare is BigInteger)
        {
            // .NET 9 enforces a maximum length of BigInteger, which is that it can contain no more than (2^31) - 1 digits
            // Using an artifical max that will permit fib(94401)
            return (T)(object)BigInteger.Pow(new BigInteger(ulong.MaxValue), 1024);
        }
        var maxValueField = typeof(T).GetField("MaxValue", BindingFlags.Public | BindingFlags.Static);
        var maxValueProperty = typeof(T).GetProperty("MaxValue", BindingFlags.Public | BindingFlags.Static);
        return (T)(
            maxValueField?.GetValue(null)
            ?? maxValueProperty?.GetValue(null)
            ?? throw new NotSupportedException("Type does not have a static public property named MaxValue."));
    }
}