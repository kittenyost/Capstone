using System;

class Program
{
    /*
     * Reverse(string s) – strict, iterative, allocation-minimal.
     *
     * Improvements vs. recursive version:
     * 1) Iterative two-pointer swap avoids recursion depth/overhead.
     * 2) No Substring allocations; just one char[] buffer.
     * 3) Same strict behavior on null (throws ArgumentNullException).
     * 4) O(n) time, O(n) space (due to output buffer).
     */

    /// <summary>
    /// Reverses a string. Throws ArgumentNullException if s is null.
    /// </summary>
    public static string Reverse(string s)
    {
        if (s is null)
            throw new ArgumentNullException(nameof(s));

        int n = s.Length;
        if (n <= 1)
            return s;

        // Copy to buffer and do in-place two-pointer swap.
        char[] buffer = s.ToCharArray();
        int i = 0, j = n - 1;
        while (i < j)
        {
            (buffer[i], buffer[j]) = (buffer[j], buffer[i]);
            i++; j--;
        }
        return new string(buffer);
    }

    /// <summary>
    /// Tolerant variant: treats null as "" instead of throwing.
    /// </summary>
    public static string ReverseOrEmpty(string s)
    {
        if (s is null) return string.Empty;
        return Reverse(s);
    }

    static void Main()
    {
        // “Right” inputs: normal words, palindromes, empty, single char,
        // spaces, punctuation, Unicode (emoji), mixed case, whitespace-only.
        var tests = new (string Label, string Input, string Expected)[]
        {
            ("simple",            "hello",       "olleh"),
            ("palindrome",        "racecar",     "racecar"),
            ("empty",             "",            ""),
            ("single-char",       "A",           "A"),
            ("with-spaces",       "ab cd",       "dc ba"),
            ("punctuation",       "a,b.c!",      "!c.b,a"),
            ("unicode-emoji",     "🙂👍",          "👍🙂"),
            ("mixed-case",        "AbCdE",       "EdCbA"),
            ("whitespace-only",   "   ",         "   "),
        };

        Console.WriteLine("== Reverse() test results (strict) ==");
        foreach (var (label, input, expected) in tests)
        {
            string actual = Reverse(input);
            bool pass = actual == expected;
            Console.WriteLine(
                $"{label,-15} | input: '{input}' -> got: '{actual}' | expected: '{expected}' | {(pass ? "PASS" : "FAIL")}"
            );
        }

        // Optional: demonstrate strict null handling (throws), then tolerant variant.
        // Uncomment to see behavior.
        // try
        // {
        //     Reverse(null);
        //     Console.WriteLine("null-input (strict): FAIL (should have thrown)");
        // }
        // catch (ArgumentNullException)
        // {
        //     Console.WriteLine("null-input (strict): PASS (threw ArgumentNullException)");
        // }

        // Console.WriteLine($"null-input (tolerant): '{ReverseOrEmpty(null)}'  // expected: ''");
    }
}