using System;

/*
BUG REPORT & FIX SUMMARY (BST)

1) BUG in Insert (left-branch recursion):
   Original code:
       if (value < current.Value)
           if (current.Left == null)
               current.Left = new Node(value);
           else
               Insert(current.Right, value);  // ❌ WRONG subtree
   Problem: When going left, the recursive call mistakenly went to current.Right,
   which can misplace nodes and break BST ordering.

   FIX:
       else
           Insert(current.Left, value);

2) BUG in Insert (right-branch handling):
   Original code:
       else if (value > current.Value)
           current.Right = new Node(value); // ❌ Always overwrites/right-inserts without checking
   Problem: This unconditionally creates a new right child, overwriting any existing subtree,
   and never recurses. It loses data and breaks structure.

   FIX:
       else if (value > current.Value)
           if (current.Right == null)
               current.Right = new Node(value);
           else
               Insert(current.Right, value);

   (Duplicate policy) If value == current.Value, we simply ignore (no duplicates).

3) BUG in Search:
   Original code returned true when current == null:
       if (current == null)
           return true; // ❌ should be false (value not found)
   Problem: That makes searches for missing values incorrectly succeed.

   FIX:
       if (current == null) return false;

All other logic left intact. Tests below verify correct behavior.
*/

class Node
{
    public int Value;
    public Node Left;
    public Node Right;

    public Node(int value) => Value = value;
}

class BST
{
    public Node Root;

    public void Insert(int value)
    {
        if (Root == null) Root = new Node(value);
        else Insert(Root, value);
    }

    private void Insert(Node current, int value)
    {
        if (value < current.Value)
        {
            if (current.Left == null) current.Left = new Node(value);
            else Insert(current.Left, value);               // ✅ recurse left
        }
        else if (value > current.Value)
        {
            if (current.Right == null) current.Right = new Node(value);
            else Insert(current.Right, value);              // ✅ recurse right
        }
        else
        {
            // value == current.Value → ignore duplicate (policy: no duplicates)
        }
    }

    public bool Search(int value) => Search(Root, value);

    private bool Search(Node current, int value)
    {
        if (current == null) return false;                  // ✅ not found
        if (current.Value == value) return true;
        return (value < current.Value)
            ? Search(current.Left, value)
            : Search(current.Right, value);
    }

    // (Optional) Inorder traversal for quick visual verification (sorted order)
    public void PrintInorder()
    {
        void Dfs(Node n)
        {
            if (n == null) return;
            Dfs(n.Left);
            Console.Write(n.Value + " ");
            Dfs(n.Right);
        }
        Dfs(Root);
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        var bst = new BST();

        // Required test sequence:
        // Insert: 10, 5, 15, 3, 7, 12, 18
        int[] values = { 10, 5, 15, 3, 7, 12, 18 };
        foreach (var v in values) bst.Insert(v);

        Console.WriteLine("Inorder after inserts (should be sorted):");
        bst.PrintInorder(); // Expected: 3 5 7 10 12 15 18

        // Required searches:
        Console.WriteLine($"Search(7)  → {bst.Search(7)}   (expected: True)");
        Console.WriteLine($"Search(11) → {bst.Search(11)}  (expected: False)");

        // Extra tests to show correctness:

        // 1) Duplicate handling (should not change structure)
        bst.Insert(7); // duplicate
        Console.Write("Inorder after inserting duplicate 7 (unchanged): ");
        bst.PrintInorder(); // Expected unchanged: 3 5 7 10 12 15 18

        // 2) Edge: search min/max and a far-miss
        Console.WriteLine($"Search(3)  → {bst.Search(3)}   (expected: True)");
        Console.WriteLine($"Search(18) → {bst.Search(18)}  (expected: True)");
        Console.WriteLine($"Search(100)→ {bst.Search(100)} (expected: False)");
    }
}