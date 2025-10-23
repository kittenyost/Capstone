using System;
using System.Diagnostics;

namespace Toolkit.Algorithms
{
    /// <summary>
    /// Helper algorithms for searching and sorting integer arrays.
    /// All methods are static for easy reuse and testing.
    /// </summary>
    public static class SortingSearchingHelpers
    {
        // ----------------------
        // SEARCHING
        // ----------------------

        /// <summary>
        /// Performs a linear search for <paramref name="target"/> in <paramref name="arr"/>.
        /// </summary>
        /// <param name="arr">Array to scan (not modified).</param>
        /// <param name="target">Value to find.</param>
        /// <returns>The index of the first occurrence, or -1 if not found.</returns>
        /// <remarks>
        /// Time Complexity: O(n) in the worst/average case; O(1) in the best case if the target is at index 0.
        /// Space Complexity: O(1).
        /// </remarks>
        public static int LinearSearch(int[] arr, int target)
        {
            if (arr == null) throw new ArgumentNullException(nameof(arr));
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == target) return i;
            }
            return -1;
        }

        /// <summary>
        /// Performs binary search for <paramref name="target"/> in a sorted array <paramref name="arr"/> (ascending).
        /// </summary>
        /// <param name="arr">Sorted array (ascending). Not modified.</param>
        /// <param name="target">Value to find.</param>
        /// <returns>The index of an occurrence, or -1 if not found.</returns>
        /// <remarks>
        /// Preconditions: <paramref name="arr"/> must be sorted in ascending order.
        /// Time Complexity: O(log n).
        /// Space Complexity: O(1).
        /// </remarks>
        public static int BinarySearch(int[] arr, int target)
        {
            if (arr == null) throw new ArgumentNullException(nameof(arr));
            int lo = 0;
            int hi = arr.Length - 1;

            while (lo <= hi)
            {
                // Overflow-safe mid calculation:
                int mid = lo + ((hi - lo) / 2);
                int value = arr[mid];

                if (value == target) return mid;
                if (value < target) lo = mid + 1;
                else hi = mid - 1;
            }
            return -1;
        }

        // ----------------------
        // SORTING
        // ----------------------

        /// <summary>
        /// In-place Bubble Sort (ascending).
        /// </summary>
        /// <param name="arr">Array to sort (modified in place).</param>
        /// <remarks>
        /// Time Complexity: O(n²) worst/average; O(n) best if already sorted (with early-exit optimization).
        /// Space Complexity: O(1).
        /// </remarks>
        public static void BubbleSort(int[] arr)
        {
            if (arr == null) throw new ArgumentNullException(nameof(arr));
            int n = arr.Length;
            bool swapped;
            for (int pass = 0; pass < n - 1; pass++)
            {
                swapped = false;
                for (int i = 0; i < n - 1 - pass; i++)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        (arr[i], arr[i + 1]) = (arr[i + 1], arr[i]);
                        swapped = true;
                    }
                }
                if (!swapped) break; // Early exit if already sorted
            }
        }

        /// <summary>
        /// Merge Sort (ascending). Returns a new sorted array; does not modify the input.
        /// </summary>
        /// <param name="arr">Array to sort (not modified).</param>
        /// <returns>New array containing the sorted values.</returns>
        /// <remarks>
        /// Time Complexity: O(n log n) in best/average/worst cases.
        /// Space Complexity: O(n) additional space for the merge buffers.
        /// </remarks>
        public static int[] MergeSort(int[] arr)
        {
            if (arr == null) throw new ArgumentNullException(nameof(arr));
            if (arr.Length <= 1) return (int[])arr.Clone();

            int[] buffer = (int[])arr.Clone();
            int[] aux = new int[arr.Length];
            TopDownSplitMerge(buffer, 0, arr.Length, aux);
            return buffer;
        }

        private static void TopDownSplitMerge(int[] a, int start, int end, int[] aux)
        {
            int length = end - start;
            if (length <= 1) return;

            int mid = start + (length / 2);
            TopDownSplitMerge(a, start, mid, aux);
            TopDownSplitMerge(a, mid, end, aux);
            Merge(a, start, mid, end, aux);
        }

        private static void Merge(int[] a, int start, int mid, int end, int[] aux)
        {
            int i = start;
            int j = mid;
            int k = start;

            // Copy to auxiliary
            for (int t = start; t < end; t++)
                aux[t] = a[t];

            while (i < mid && j < end)
            {
                if (aux[i] <= aux[j]) a[k++] = aux[i++];
                else a[k++] = aux[j++];
            }
            while (i < mid) a[k++] = aux[i++];
            while (j < end) a[k++] = aux[j++];
        }

        // ----------------------
        // TIMING HARNESS
        // ----------------------

        /// <summary>
        /// Generates a random integer array.
        /// </summary>
        /// <param name="length">Number of elements.</param>
        /// <param name="seed">Optional seed for repeatable runs.</param>
        public static int[] GenerateRandomArray(int length, int? seed = null)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            var rng = seed.HasValue ? new Random(seed.Value) : new Random();
            var arr = new int[length];
            for (int i = 0; i < length; i++)
                arr[i] = rng.Next(int.MinValue, int.MaxValue);
            return arr;
        }

        /// <summary>
        /// Prints timing results for sorts and searches to the console.
        /// Run this from your DemoHarness (Program.cs) or a test project.
        /// </summary>
        /// <param name="seed">Optional seed for reproducibility.</param>
        public static void PrintTimingReport(int? seed = 12345)
        {
            int[] sizes = new[] { 100, 1_000, 10_000 };

            Console.WriteLine("== Sorting & Searching Timing Report ==");
            Console.WriteLine($"Seed: {(seed.HasValue ? seed.Value.ToString() : "random")}");
            Console.WriteLine();

            foreach (int n in sizes)
            {
                Console.WriteLine($"-- n = {n} --");

                // Prepare data
                int[] baseArr = GenerateRandomArray(n, seed);

                // ---- Sorting timings ----
                // Bubble (in-place) - work on a copy so runs are independent
                int[] bubbleArr = (int[])baseArr.Clone();
                var sw = Stopwatch.StartNew();
                BubbleSort(bubbleArr);
                sw.Stop();
                Console.WriteLine($"BubbleSort: {sw.ElapsedMilliseconds} ms");

                // Merge (out-of-place)
                sw.Restart();
                int[] mergeArr = MergeSort(baseArr);
                sw.Stop();
                Console.WriteLine($"MergeSort:  {sw.ElapsedMilliseconds} ms");

                // ---- Searching timings ----
                // Use sorted array for fair searching comparisons
                int[] sorted = mergeArr; // already sorted from previous step

                // Choose targets: one present, one absent
                int present = sorted[n / 2];
                int absent = int.MaxValue; // likely not present

                // Linear search (present)
                sw.Restart();
                int idx1 = LinearSearch(sorted, present);
                sw.Stop();
                Console.WriteLine($"LinearSearch (present): {sw.ElapsedTicks} ticks (idx={idx1})");

                // Binary search (present)
                sw.Restart();
                int idx2 = BinarySearch(sorted, present);
                sw.Stop();
                Console.WriteLine($"BinarySearch (present): {sw.ElapsedTicks} ticks (idx={idx2})");

                // Linear search (absent)
                sw.Restart();
                int idx3 = LinearSearch(sorted, absent);
                sw.Stop();
                Console.WriteLine($"LinearSearch (absent):  {sw.ElapsedTicks} ticks (idx={idx3})");

                // Binary search (absent)
                sw.Restart();
                int idx4 = BinarySearch(sorted, absent);
                sw.Stop();
                Console.WriteLine($"BinarySearch (absent):  {sw.ElapsedTicks} ticks (idx={idx4})");

                Console.WriteLine();
            }

            Console.WriteLine("Note: Ticks have higher resolution than milliseconds; compare like-with-like.");
            Console.WriteLine("For stable numbers, build in Release and run without the debugger (Ctrl+F5).");
        }

        /// <summary>
        /// A tiny sanity check to verify basic behavior.
        /// </summary>
        public static void RunSanityCheck()
        {
            int[] arr = new[] { 5, 1, 4, 2, 8, 0, 2 };
            int[] sortedByMerge = MergeSort(arr);
            BubbleSort(arr); // in-place

            if (arr.Length != sortedByMerge.Length) throw new Exception("Length mismatch.");
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != sortedByMerge[i])
                    throw new Exception("Sorting mismatch between BubbleSort and MergeSort.");
            }

            int target = arr[arr.Length / 2];
            int i1 = LinearSearch(arr, target);
            int i2 = BinarySearch(arr, target); // valid because arr is sorted now
            if (i1 == -1 || i2 == -1) throw new Exception("Search failed to find existing item.");

            Console.WriteLine("Sanity check passed.");
        }
    }
}

/*


*/
