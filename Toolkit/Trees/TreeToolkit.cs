using System;
using System.Collections.Generic;
using System.Text;

namespace Toolkit.Trees
{
    /*
 Reflection – Kathy Yost
 IT 415 – Week 6 TreeToolkit
 October 31, 2025

 Working on the TreeToolkit project helped me see how much the traversal order 
 can change the way we look at a tree. With preorder, it feels like you’re 
 walking the structure from the top down, visiting each node before its 
 children. Postorder feels the opposite—you don’t deal with a parent until 
 you’ve finished both sides. But inorder made the most sense to me because it 
 shows the values in sorted order, which helped me understand how binary search 
 trees actually organize data.

 The height part was eye-opening. I used to think it was just a measurement, 
 but I realized it tells you how deep a search might go and how efficient the 
 tree really is. The smaller the height, the faster you can find things.

 When I compared a balanced tree to a skewed one, it was obvious why 
 self-balancing trees exist. Inserting sorted values one after another made my 
 tree look more like a linked list, and the height grew fast. That directly 
 affected how many comparisons each search needed. Seeing the difference made 
 the concept of O(log n) versus O(n) performance click for me in a real way. 
 This lab made trees feel less abstract and more like something practical I 
 could use.
 */

    /// <summary>
    /// A simple binary tree node storing integer values.
    /// Provides traversals (inorder, preorder, postorder) and utilities for
    /// height (edges convention) and depth (edges from a given root).
    /// </summary>
    public class TreeNode
    {
        /// <summary>Node value.</summary>
        public int Value { get; set; }

        /// <summary>Left child.</summary>
        public TreeNode? Left { get; set; }

        /// <summary>Right child.</summary>
        public TreeNode? Right { get; set; }

        public TreeNode(int value, TreeNode? left = null, TreeNode? right = null)
        {
            Value = value;
            Left = left;
            Right = right;
        }

        /// <summary>
        /// Builds the “teaching tree” used in lecture: 
        /// 
        ///           38
        ///          /  \
        ///        27    43
        ///       /  \
        ///      3    9
        /// 
        /// </summary>
        public static TreeNode BuildTeachingTree()
        {
            return new TreeNode(38,
                left: new TreeNode(27,
                    left: new TreeNode(3),
                    right: new TreeNode(9)),
                right: new TreeNode(43));
        }

        /// <summary>
        /// Inorder traversal (Left, Root, Right).
        /// </summary>
        /// <remarks>Time: O(n). Space: O(h) recursion stack (h = height).</remarks>
        public static List<int> Inorder(TreeNode? root)
        {
            var res = new List<int>();
            void Dfs(TreeNode? n)
            {
                if (n == null) return;
                Dfs(n.Left);
                res.Add(n.Value);
                Dfs(n.Right);
            }
            Dfs(root);
            return res;
        }

        /// <summary>
        /// Preorder traversal (Root, Left, Right).
        /// </summary>
        /// <remarks>Time: O(n). Space: O(h) recursion stack.</remarks>
        public static List<int> Preorder(TreeNode? root)
        {
            var res = new List<int>();
            void Dfs(TreeNode? n)
            {
                if (n == null) return;
                res.Add(n.Value);
                Dfs(n.Left);
                Dfs(n.Right);
            }
            Dfs(root);
            return res;
        }

        /// <summary>
        /// Postorder traversal (Left, Right, Root).
        /// </summary>
        /// <remarks>Time: O(n). Space: O(h) recursion stack.</remarks>
        public static List<int> Postorder(TreeNode? root)
        {
            var res = new List<int>();
            void Dfs(TreeNode? n)
            {
                if (n == null) return;
                Dfs(n.Left);
                Dfs(n.Right);
                res.Add(n.Value);
            }
            Dfs(root);
            return res;
        }

        /// <summary>
        /// Height of a tree using the edges convention:
        /// empty tree = -1, leaf = 0.
        /// </summary>
        /// <remarks>Time: O(n). Space: O(h) recursion stack.</remarks>
        public static int Height(TreeNode? root)
        {
            if (root == null) return -1;
            return 1 + Math.Max(Height(root.Left), Height(root.Right));
        }

        /// <summary>
        /// Returns the depth (number of edges from <paramref name="root"/> to the
        /// first node whose value equals <paramref name="target"/>), or -1 if not found.
        /// </summary>
        /// <remarks>
        /// Time: O(n) in general binary trees; O(h) if called on a BST with guided search.
        /// Space: O(h) recursion stack.
        /// </remarks>
        public static int DepthOf(TreeNode? root, int target)
        {
            int Dfs(TreeNode? n, int depth)
            {
                if (n == null) return -1;
                if (n.Value == target) return depth;
                int left = Dfs(n.Left, depth + 1);
                if (left != -1) return left;
                return Dfs(n.Right, depth + 1);
            }
            return Dfs(root, 0);
        }
    }

    /// <summary>
    /// A minimal Binary Search Tree (BST) over integers.
    /// Supports Insert (no duplicates) and Contains.
    /// </summary>
    public class Bst
    {
        /// <summary>Root node of the BST.</summary>
        public TreeNode? Root { get; private set; }

        /// <summary>
        /// Inserts a value into the BST; duplicates are ignored.
        /// </summary>
        /// <param name="value">Value to insert.</param>
        /// <remarks>
        /// Average: O(log n), Worst (skewed): O(n).
        /// Space: O(h) recursion or O(1) iterative; here iterative O(1).
        /// </remarks>
        public void Insert(int value)
        {
            if (Root == null)
            {
                Root = new TreeNode(value);
                return;
            }

            var cur = Root;
            TreeNode? parent = null;

            while (cur != null)
            {
                parent = cur;
                if (value == cur.Value) return; // ignore duplicates
                cur = (value < parent.Value) ? parent.Left : parent.Right;
            }

            if (value < parent!.Value) parent.Left = new TreeNode(value);
            else parent.Right = new TreeNode(value);
        }

        /// <summary>
        /// Returns true if value exists in the BST.
        /// </summary>
        /// <remarks>
        /// Average: O(log n), Worst (skewed): O(n).
        /// Space: O(1).
        /// </remarks>
        public bool Contains(int value)
        {
            var cur = Root;
            while (cur != null)
            {
                if (value == cur.Value) return true;
                cur = (value < cur.Value) ? cur.Left : cur.Right;
            }
            return false;
        }

        /// <summary>
        /// Computes height (edges convention) of a subtree rooted at <paramref name="node"/>.
        /// Provided as a static helper to meet the assignment’s requirement.
        /// </summary>
        public static int Height(TreeNode? node) => TreeNode.Height(node);
    }

    /// <summary>
    /// Small console demo you can call from your app to produce the deliverable outputs.
    /// </summary>
    public static class TreeDemo
    {
        public static void Run()
        {
            var sb = new StringBuilder();

            // 1) Teaching tree traversals and height/depth
            var teaching = TreeNode.BuildTeachingTree();
            var inorder = TreeNode.Inorder(teaching);     // Expected: 3, 27, 9, 38, 43
            var preorder = TreeNode.Preorder(teaching);   // Expected: 38, 27, 3, 9, 43
            var postorder = TreeNode.Postorder(teaching); // Expected: 3, 9, 27, 43, 38
            int heightTeaching = TreeNode.Height(teaching); // Expected: 2 (edges)

            sb.AppendLine("Teaching Tree Traversals:");
            sb.AppendLine($"Inorder:   {string.Join(", ", inorder)}   (expected: 3, 27, 9, 38, 43)");
            sb.AppendLine($"Preorder:  {string.Join(", ", preorder)} (expected: 38, 27, 3, 9, 43)");
            sb.AppendLine($"Postorder: {string.Join(", ", postorder)} (expected: 3, 9, 27, 43, 38)");
            sb.AppendLine($"Height (edges): {heightTeaching}          (expected: 2)");
            sb.AppendLine($"Depth of 38: {TreeNode.DepthOf(teaching, 38)} (expected: 0)");
            sb.AppendLine($"Depth of 27: {TreeNode.DepthOf(teaching, 27)} (expected: 1)");
            sb.AppendLine($"Depth of 3:  {TreeNode.DepthOf(teaching, 3)}  (expected: 2)");
            sb.AppendLine();

            // 2) BST with lecture sequence
            var bst = new Bst();
            int[] lectureSeq = { 50, 30, 70, 20, 40, 60, 80 };
            foreach (var v in lectureSeq) bst.Insert(v);

            sb.AppendLine("BST: Lecture insertion sequence {50,30,70,20,40,60,80}");
            sb.AppendLine($"Contains(60): {bst.Contains(60)} (expected: True)");
            sb.AppendLine($"Contains(25): {bst.Contains(25)} (expected: False)");
            sb.AppendLine($"Height (edges): {Bst.Height(bst.Root)} (expected: 2)");
            sb.AppendLine();

            // 3) Skewed BST (sorted insertions)
            var skewed = new Bst();
            int[] sorted = { 10, 20, 30, 40, 50 };
            foreach (var v in sorted) skewed.Insert(v);

            sb.AppendLine("BST: Sorted insertion sequence {10,20,30,40,50} (skewed)");
            sb.AppendLine($"Contains(40): {skewed.Contains(40)} (expected: True)");
            sb.AppendLine($"Contains(5):  {skewed.Contains(5)}  (expected: False)");
            sb.AppendLine($"Height (edges): {Bst.Height(skewed.Root)} (expected: 4)");

            Console.WriteLine(sb.ToString());
        }
    }
}