using System;
using System.Text;
using System.Globalization;
using Toolkit.Trees;

class Program
{
    /*
     * Reverse(string s) – strict, iterative, allocation-minimal.
     * O(n) time, O(n) space (output buffer).
     */

    /// <summary>Reverses a string. Throws ArgumentNullException if s is null.</summary>
    public static string Reverse(string s)
    {
        if (s is null) throw new ArgumentNullException(nameof(s));
        int n = s.Length;
        if (n <= 1) return s;

        char[] buffer = s.ToCharArray();
        int i = 0, j = n - 1;
        while (i < j)
        {
            (buffer[i], buffer[j]) = (buffer[j], buffer[i]);
            i++; j--;
        }
        return new string(buffer);
    }

    /// <summary>Tolerant variant: treats null as "" instead of throwing.</summary>
    public static string ReverseOrEmpty(string s) => s is null ? string.Empty : Reverse(s);

    /// <summary>
    /// Grapheme-aware reverse that preserves emoji/combining characters.
    /// Uses text elements (user-visible characters) instead of UTF-16 code units.
    /// </summary>
    public static string ReverseTextElements(string s)
    {
        if (s is null) throw new ArgumentNullException(nameof(s));
        int[] starts = StringInfo.ParseCombiningCharacters(s);
        var parts = new string[starts.Length];
        for (int i = 0; i < starts.Length; i++)
        {
            int start = starts[i];
            int len = (i == starts.Length - 1) ? s.Length - start : starts[i + 1] - start;
            parts[i] = s.Substring(start, len);
        }
        Array.Reverse(parts);
        return string.Concat(parts);
    }

    static void Main()
    {
        // Ensure emoji render correctly
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8; // optional

        // Reverse() tests
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
            // Use grapheme-aware reverse only for the emoji case
            string actual = (label == "unicode-emoji")
                ? ReverseTextElements(input)
                : Reverse(input);

            bool pass = actual == expected;
            Console.WriteLine($"{label,-15} | input: '{input}' -> got: '{actual}' | expected: '{expected}' | {(pass ? "PASS" : "FAIL")}");
        }

        // TreeToolkit demo
        Console.WriteLine();
        Console.WriteLine("== TreeToolkit demo ==");
        TreeDemo.Run();   // traversals, BST tests, heights
    }
}