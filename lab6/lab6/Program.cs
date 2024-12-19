using System;

public class Calculator<T> where T : struct
{
    public delegate T Operation(T a, T b);
    public Operation Add { get; set; }
    public Operation Subtract { get; set; }
    public Operation Multiply { get; set; }
    public Operation Divide { get; set; }

    public T ExecuteAdd(T a, T b) => Add(a, b);
    public T ExecuteSubtract(T a, T b) => Subtract(a, b);
    public T ExecuteMultiply(T a, T b) => Multiply(a, b);
    public T ExecuteDivide(T a, T b) => Divide(a, b);
}

class Program
{
    static void Main(string[] args)
    {
        var intCalculator = new Calculator<int>
        {
            Add = (a, b) => a + b,
            Subtract = (a, b) => a - b,
            Multiply = (a, b) => a * b,
            Divide = (a, b) => b != 0 ? a / b : throw new DivideByZeroException("Division by zero")
        };

        Console.WriteLine("Int Calculator:");
        Console.WriteLine($"5 + 3 = {intCalculator.ExecuteAdd(5, 3)}");
        Console.WriteLine($"5 - 3 = {intCalculator.ExecuteSubtract(5, 3)}");
        Console.WriteLine($"5 * 3 = {intCalculator.ExecuteMultiply(5, 3)}");
        Console.WriteLine($"5 / 3 = {intCalculator.ExecuteDivide(5, 3)}");

        var doubleCalculator = new Calculator<double>
        {
            Add = (a, b) => a + b,
            Subtract = (a, b) => a - b,
            Multiply = (a, b) => a * b,
            Divide = (a, b) => b != 0 ? a / b : throw new DivideByZeroException("Division by zero")
        };

        Console.WriteLine("\nDouble Calculator:");
        Console.WriteLine($"5.5 + 3.3 = {doubleCalculator.ExecuteAdd(5.5, 3.3)}");
        Console.WriteLine($"5.5 - 3.3 = {doubleCalculator.ExecuteSubtract(5.5, 3.3)}");
        Console.WriteLine($"5.5 * 3.3 = {doubleCalculator.ExecuteMultiply(5.5, 3.3)}");
        Console.WriteLine($"5.5 / 3.3 = {doubleCalculator.ExecuteDivide(5.5, 3.3)}");

        var floatCalculator = new Calculator<float>
        {
            Add = (a, b) => a + b,
            Subtract = (a, b) => a - b,
            Multiply = (a, b) => a * b,
            Divide = (a, b) => Math.Abs(b) > 1e-6 ? a / b : throw new DivideByZeroException("Division by zero")
        };

        Console.WriteLine("\nFloat Calculator:");
        Console.WriteLine($"6.4 + 5.3 = {floatCalculator.ExecuteAdd(6.4f, 5.3f)}");
        Console.WriteLine($"7.9 - 1.7 = {floatCalculator.ExecuteSubtract(7.9f, 1.7f)}");
        Console.WriteLine($"8.7 * 16.8 = {floatCalculator.ExecuteMultiply(8.7f, 16.8f)}");
        Console.WriteLine($"9.5 / 4.8 = {floatCalculator.ExecuteDivide(9.5f, 4.8f)}");
    }
}