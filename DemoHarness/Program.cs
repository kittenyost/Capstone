using System;
using System.Linq;
using Toolkit;  // namespace from your Toolkit project

Console.WriteLine("=== Recursion Demo ===");
Console.WriteLine($"Factorial(5) = {RecursionHelpers.Factorial(5)}");
Console.WriteLine($"Fibonacci(6) = {RecursionHelpers.Fibonacci(6)}");
Console.WriteLine($"Sum([1,2,3,4]) = {RecursionHelpers.Sum(new[] { 1, 2, 3, 4 })}");
Console.WriteLine($"IsPalindrome(\"abba\") = {RecursionHelpers.IsPalindrome("abba")}");
var ps = RecursionHelpers.PowerSet(new[] { 'A', 'B', 'C' });
Console.WriteLine($"PowerSet count = {ps.Count}");
foreach (var subset in ps.Take(5))
    Console.WriteLine("  {" + string.Join(",", subset) + "}");
Console.WriteLine("=== Done ===");