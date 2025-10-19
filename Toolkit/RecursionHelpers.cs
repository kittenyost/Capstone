using System;
using System.Collections.Generic;
using System.IO;

namespace Toolkit
{
    /// <summary>
    /// Pure-recursive helpers demonstrating mathematical, structural, and problem-solving recursion.
    /// Each method documents base/recursive cases and expected time complexity.
    /// </summary>
    public static class RecursionHelpers
    {
        // ---------- MATHEMATICAL RECURSION ----------

        /// <summary>
        /// Computes n! recursively.
        /// <para><b>Base case:</b> n == 0 or 1 ⇒ 1.</para>
        /// <para><b>Recursive case:</b> n! = n × (n − 1)!</para>
        /// <para><b>Time:</b> O(n) • <b>Space (stack):</b> O(n)</para>
        /// </summary>
        /// <param name="n">Non-negative integer.</param>
        /// <returns>Factorial of <paramref name="n"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="n"/> &lt; 0.</exception>
        public static long Factorial(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative");
            if (n == 0 || n == 1) return 1;
            checked { return n * Factorial(n - 1); }
        }

        /// <summary>
        /// Computes Fibonacci(n) recursively using the classic definition.
        /// <para><b>Base cases:</b> F(0)=0, F(1)=1.</para>
        /// <para><b>Recursive case:</b> F(n)=F(n−1)+F(n−2)</para>
        /// <para><b>Time:</b> O(2^n) naive • <b>Space:</b> O(n)</para>
        /// </summary>
        /// <param name="n">Index n ≥ 0.</param>
        /// <returns>Fibonacci number at <paramref name="n"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="n"/> &lt; 0.</exception>
        public static long Fibonacci(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative");
            if (n == 0) return 0;
            if (n == 1) return 1;
            checked { return Fibonacci(n - 1) + Fibonacci(n - 2); }
        }

        /// <summary>
        /// Sums an integer array recursively.
        /// <para><b>Base case:</b> empty slice ⇒ 0.</para>
        /// <para><b>Recursive case:</b> a[i] + Sum(a, i+1)</para>
        /// <para><b>Time:</b> O(n) • <b>Space:</b> O(n)</para>
        /// </summary>
        /// <param name="values">Array to sum (non-null).</param>
        /// <returns>Total sum of all elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is null.</exception>
        public static long Sum(int[] values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            return SumAt(values, 0);

            static long SumAt(int[] a, int i)
            {
                if (i >= a.Length) return 0;
                return a[i] + SumAt(a, i + 1);
            }
        }

        // ---------- STRUCTURAL RECURSION ----------

        /// <summary>
        /// Recursively lists all files (and optionally directories) under <paramref name="rootPath"/>.
        /// <para><b>Base case:</b> no more entries ⇒ return accumulated list.</para>
        /// <para><b>Recursive case:</b> process one entry, then recurse; descend into subdirectories.</para>
        /// <para><b>Time:</b> O(N) over entries • <b>Space:</b> O(H) for directory depth + output size</para>
        /// </summary>
        /// <param name="rootPath">Existing directory path.</param>
        /// <param name="includeDirectories">If true, include directory paths in results.</param>
        /// <returns>All paths discovered under the root.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="rootPath"/> is null.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when <paramref name="rootPath"/> does not exist.</exception>
        public static List<string> ListFilesRecursive(string rootPath, bool includeDirectories = true)
        {
            if (rootPath == null) throw new ArgumentNullException(nameof(rootPath));
            if (!Directory.Exists(rootPath)) throw new DirectoryNotFoundException(rootPath);

            var results = new List<string>();
            var dirs = Directory.GetDirectories(rootPath);
            var files = Directory.GetFiles(rootPath);

            if (includeDirectories) results.Add(rootPath);

            AddFiles(files, 0, results);
            AddDirs(dirs, 0, includeDirectories, results);

            return results;

            static void AddFiles(string[] files, int i, List<string> acc)
            {
                if (i >= files.Length) return;
                acc.Add(files[i]);
                AddFiles(files, i + 1, acc);
            }

            static void AddDirs(string[] dirs, int i, bool includeDir, List<string> acc)
            {
                if (i >= dirs.Length) return;
                var d = dirs[i];
                if (includeDir) acc.Add(d);

                var childDirs = Directory.GetDirectories(d);
                var childFiles = Directory.GetFiles(d);
                AddFiles(childFiles, 0, acc);
                AddDirs(childDirs, 0, includeDir, acc);

                // next sibling
                AddDirs(dirs, i + 1, includeDir, acc);
            }
        }

        // ---------- PROBLEM-SOLVING RECURSION ----------

        /// <summary>
        /// Recursively checks if a string is a palindrome (case-sensitive, exact match).
        /// <para><b>Base case:</b> length ≤ 1 ⇒ true.</para>
        /// <para><b>Recursive case:</b> s[0] == s[^1] AND IsPalindrome(mid)</para>
        /// <para><b>Time:</b> O(n) • <b>Space:</b> O(n) for recursion depth</para>
        /// </summary>
        /// <param name="s">Input string (non-null).</param>
        /// <returns><c>true</c> if palindrome; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="s"/> is null.</exception>
        public static bool IsPalindrome(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            return IsPalRange(s, 0, s.Length - 1);

            static bool IsPalRange(string s, int left, int right)
            {
                if (left >= right) return true;
                if (s[left] != s[right]) return false;
                return IsPalRange(s, left + 1, right - 1);
            }
        }

        /// <summary>
        /// Builds the power set (the set of all subsets) of the given list via recursion.
        /// <para><b>Base case:</b> index == n ⇒ return list containing the empty subset.</para>
        /// <para><b>Recursive case:</b> P(i) = P(i+1) ∪ { each subset with items[i] appended }.</para>
        /// <para><b>Time:</b> O(2^n · n) to copy elements • <b>Space:</b> O(2^n · n) output + O(n) stack</para>
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <param name="items">Input list (non-null).</param>
        /// <returns>List of all subsets of <paramref name="items"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="items"/> is null.</exception>
        public static List<List<T>> PowerSet<T>(IList<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            return Build(items, 0);

            static List<List<T>> Build(IList<T> src, int i)
            {
                if (i == src.Count)
                    return new List<List<T>> { new List<T>() }; // empty subset

                var without = Build(src, i + 1);           // subsets that don't include src[i]
                var with = CopyWith(without, src[i], 0);   // same subsets but with src[i] added
                return Concat(without, with, 0);           // without ∪ with

                static List<List<T>> CopyWith(List<List<T>> subsets, T value, int k)
                {
                    if (k >= subsets.Count) return new List<List<T>>();
                    var head = new List<T>(subsets[k]) { value };
                    var rest = CopyWith(subsets, value, k + 1);
                    rest.Insert(0, head);
                    return rest;
                }

                static List<List<T>> Concat(List<List<T>> a, List<List<T>> b, int i)
                {
                    if (i == 0)
                    {
                        var start = Clone(a, 0);
                        return AppendAll(start, b, 0);
                    }
                    return a;
                }

                static List<List<T>> Clone(List<List<T>> src, int i)
                {
                    if (i >= src.Count) return new List<List<T>>();
                    var head = new List<List<T>> { new List<T>(src[i]) };
                    var tail = Clone(src, i + 1);
                    return PrependAll(head, tail, 0);
                }

                static List<List<T>> PrependAll(List<List<T>> head, List<List<T>> tail, int i)
                {
                    if (i >= tail.Count) return head;
                    head.Add(tail[i]);
                    return PrependAll(head, tail, i + 1);
                }

                static List<List<T>> AppendAll(List<List<T>> acc, List<List<T>> src, int i)
                {
                    if (i >= src.Count) return acc;
                    acc.Add(src[i]);
                    return AppendAll(acc, src, i + 1);
                }
            }
        }
    }
}