using System;
using System.IO;
using Toolkit;          // <-- make sure your test project references the Toolkit project
using Xunit;

namespace Tests
{
    public class RecursionHelpersTests
    {
        // ---------- Mathematical ----------

        [Fact]
        public void Factorial_Zero_IsOne()
        {
            Assert.Equal(1L, RecursionHelpers.Factorial(0));
        }

        [Fact]
        public void Factorial_Five_Is120()
        {
            Assert.Equal(120L, RecursionHelpers.Factorial(5));
        }

        [Fact]
        public void Factorial_Negative_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => RecursionHelpers.Factorial(-1));
        }

        [Fact]
        public void Fibonacci_Six_IsEight()
        {
            Assert.Equal(8L, RecursionHelpers.Fibonacci(6));
        }

        [Fact]
        public void Sum_Array_Works()
        {
            Assert.Equal(0L, RecursionHelpers.Sum(Array.Empty<int>()));
            Assert.Equal(6L, RecursionHelpers.Sum(new[] { 1, 2, 3 }));
        }

        // ---------- Problem-solving ----------

        [Theory]
        [InlineData("", true)]
        [InlineData("a", true)]
        [InlineData("aba", true)]
        [InlineData("abba", true)]
        [InlineData("ab", false)]
        [InlineData("abc", false)]
        public void IsPalindrome_Cases(string s, bool expected)
        {
            Assert.Equal(expected, RecursionHelpers.IsPalindrome(s));
        }

        [Fact]
        public void PowerSet_Count_Is_2powN()
        {
            var ps = RecursionHelpers.PowerSet(new[] { 1, 2, 3 });
            Assert.Equal(8, ps.Count); // 2^3
        }

        // ---------- Structural ----------

        [Fact]
        public void ListFilesRecursive_Returns_Files_And_Dirs()
        {
            var root = Path.Combine(Path.GetTempPath(), "recursion-tests-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(root);
            var sub = Directory.CreateDirectory(Path.Combine(root, "Sub")).FullName;
            var f1 = Path.Combine(root, "root.txt");
            var f2 = Path.Combine(sub, "sub.txt");
            File.WriteAllText(f1, "root");
            File.WriteAllText(f2, "sub");

            try
            {
                var entries = RecursionHelpers.ListFilesRecursive(root, includeDirectories: true);
                Assert.Contains(root, entries);
                Assert.Contains(sub, entries);
                Assert.Contains(f1, entries);
                Assert.Contains(f2, entries);
            }
            finally
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }
}