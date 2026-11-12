using System;
using System.Collections.Generic;
using System.Linq;

namespace ngakamodirimolema
{
    public class BinaryTreeNode<T>
    {
        public T Value { get; set; }
        public BinaryTreeNode<T>? Left { get; set; }
        public BinaryTreeNode<T>? Right { get; set; }
        public BinaryTreeNode(T value) => Value = value;
    }

    public class BinaryTree<T>
    {
        public BinaryTreeNode<T>? Root { get; protected set; }
        public IEnumerable<T> PreOrder() => Pre(Root);
        public IEnumerable<T> InOrder() => In(Root);
        public IEnumerable<T> PostOrder() => Post(Root);
        public IEnumerable<T> LevelOrder()
        {
            if (Root == null) yield break;
            var q = new Queue<BinaryTreeNode<T>>(); q.Enqueue(Root);
            while (q.Count > 0) { var n = q.Dequeue(); yield return n.Value; if (n.Left != null) q.Enqueue(n.Left); if (n.Right != null) q.Enqueue(n.Right); }
        }
        private static IEnumerable<T> Pre(BinaryTreeNode<T>? n) { if (n == null) yield break; yield return n.Value; foreach (var x in Pre(n.Left)) yield return x; foreach (var x in Pre(n.Right)) yield return x; }
        private static IEnumerable<T> In(BinaryTreeNode<T>? n) { if (n == null) yield break; foreach (var x in In(n.Left)) yield return x; yield return n.Value; foreach (var x in In(n.Right)) yield return x; }
        private static IEnumerable<T> Post(BinaryTreeNode<T>? n) { if (n == null) yield break; foreach (var x in Post(n.Left)) yield return x; foreach (var x in Post(n.Right)) yield return x; yield return n.Value; }
    }

    public sealed class BinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private sealed class Node { public TKey Key; public TValue Value; public Node? Left, Right; public Node(TKey k, TValue v) { Key = k; Value = v; } }
        private Node? _root;
        public int Count { get; private set; }
        public void Insert(TKey key, TValue value) { _root = Insert(_root, key, value); }
        private Node Insert(Node? n, TKey k, TValue v) { if (n == null) { Count++; return new Node(k, v); } int c = k.CompareTo(n.Key); if (c < 0) n.Left = Insert(n.Left, k, v); else if (c > 0) n.Right = Insert(n.Right, k, v); else n.Value = v; return n; }
        public bool TrySearch(TKey key, out TValue value) { var n = _root; while (n != null) { int c = key.CompareTo(n.Key); if (c == 0) { value = n.Value; return true; } n = c < 0 ? n.Left : n.Right; } value = default!; return false; }
        public bool Delete(TKey key) { if (!TrySearch(key, out _)) return false; _root = Delete(_root, key); Count--; return true; }
        private Node? Delete(Node? n, TKey k) { if (n == null) return null; int c = k.CompareTo(n.Key); if (c < 0) n.Left = Delete(n.Left, k); else if (c > 0) n.Right = Delete(n.Right, k); else { if (n.Left == null) return n.Right; if (n.Right == null) return n.Left; var s = n.Right; while (s.Left != null) s = s.Left; n.Key = s.Key; n.Value = s.Value; n.Right = Delete(n.Right, s.Key); } return n; }
        public int Height => HeightOf(_root);
        private static int HeightOf(Node? n) => n == null ? 0 : 1 + Math.Max(HeightOf(n.Left), HeightOf(n.Right));
        public IEnumerable<KeyValuePair<TKey, TValue>> InOrder() { foreach (var p in Walk(_root)) yield return p; }
        private static IEnumerable<KeyValuePair<TKey, TValue>> Walk(Node? n) { if (n == null) yield break; foreach (var x in Walk(n.Left)) yield return x; yield return new(n.Key, n.Value); foreach (var x in Walk(n.Right)) yield return x; }
    }

    public sealed class AvlTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private sealed class Node { public TKey Key; public TValue Value; public Node? Left, Right; public int Height = 1; public Node(TKey k, TValue v) { Key = k; Value = v; } }
        private Node? _root; public int Count { get; private set; } public int RotationCount { get; private set; } public int LastComparisons { get; private set; }
        private static int H(Node? n) => n?.Height ?? 0; private static void Fix(Node n) => n.Height = 1 + Math.Max(H(n.Left), H(n.Right)); private static int B(Node n) => H(n.Left) - H(n.Right);
        private Node RotateRight(Node y) { RotationCount++; var x = y.Left!; y.Left = x.Right; x.Right = y; Fix(y); Fix(x); return x; }
        private Node RotateLeft(Node x) { RotationCount++; var y = x.Right!; x.Right = y.Left; y.Left = x; Fix(x); Fix(y); return y; }
        private Node Balance(Node n) { Fix(n); if (B(n) > 1) { if (B(n.Left!) < 0) n.Left = RotateLeft(n.Left!); return RotateRight(n); } if (B(n) < -1) { if (B(n.Right!) > 0) n.Right = RotateRight(n.Right!); return RotateLeft(n); } return n; }
        public void Insert(TKey key, TValue value) { _root = Add(_root, key, value); }
        private Node Add(Node? n, TKey k, TValue v) { if (n == null) { Count++; return new Node(k, v); } int c = k.CompareTo(n.Key); if (c < 0) n.Left = Add(n.Left, k, v); else if (c > 0) n.Right = Add(n.Right, k, v); else { n.Value = v; return n; } return Balance(n); }
        public bool TrySearch(TKey key, out TValue value) { LastComparisons = 0; var n = _root; while (n != null) { LastComparisons++; int c = key.CompareTo(n.Key); if (c == 0) { value = n.Value; return true; } n = c < 0 ? n.Left : n.Right; } value = default!; return false; }
        public int Height => H(_root); public IEnumerable<KeyValuePair<TKey, TValue>> InOrder() { foreach (var x in Walk(_root)) yield return x; }
        private static IEnumerable<KeyValuePair<TKey, TValue>> Walk(Node? n) { if (n == null) yield break; foreach (var x in Walk(n.Left)) yield return x; yield return new(n.Key, n.Value); foreach (var x in Walk(n.Right)) yield return x; }
    }

    public sealed class RedBlackTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private sealed class Node { public TKey Key; public TValue Value; public bool Red; public Node? Left, Right, Parent; public Node(TKey k, TValue v) { Key = k; Value = v; Red = true; } }
        private Node? _root;
        public int ColourChanges { get; private set; } public int RotationCount { get; private set; }
        public void Insert(TKey key, TValue value)
        {
            Node? parent = null, current = _root;
            while (current != null) { parent = current; int c = key.CompareTo(current.Key); if (c == 0) { current.Value = value; return; } current = c < 0 ? current.Left : current.Right; }
            var node = new Node(key, value) { Parent = parent }; if (parent == null) _root = node; else if (key.CompareTo(parent.Key) < 0) parent.Left = node; else parent.Right = node; FixInsert(node);
        }
        private static bool IsRed(Node? n) => n?.Red == true;
        private void Recolour(Node n, bool red) { if (n.Red != red) ColourChanges++; n.Red = red; }
        private void RotateLeft(Node x) { RotationCount++; var y=x.Right!; x.Right=y.Left; if(y.Left!=null)y.Left.Parent=x; y.Parent=x.Parent; if(x.Parent==null)_root=y; else if(x==x.Parent.Left)x.Parent.Left=y; else x.Parent.Right=y; y.Left=x; x.Parent=y; }
        private void RotateRight(Node x) { RotationCount++; var y=x.Left!; x.Left=y.Right; if(y.Right!=null)y.Right.Parent=x; y.Parent=x.Parent; if(x.Parent==null)_root=y; else if(x==x.Parent.Right)x.Parent.Right=y; else x.Parent.Left=y; y.Right=x; x.Parent=y; }
        private void FixInsert(Node z)
        {
            while (z.Parent != null && IsRed(z.Parent)) { var p=z.Parent; var g=p.Parent!; if(p==g.Left) { var u=g.Right; if(IsRed(u)) { Recolour(p,false); Recolour(u!,false); Recolour(g,true); z=g; } else { if(z==p.Right){z=p; RotateLeft(z); p=z.Parent!; g=p.Parent!;} Recolour(p,false); Recolour(g,true); RotateRight(g); } } else { var u=g.Left; if(IsRed(u)) { Recolour(p,false); Recolour(u!,false); Recolour(g,true); z=g; } else { if(z==p.Left){z=p; RotateRight(z); p=z.Parent!; g=p.Parent!;} Recolour(p,false); Recolour(g,true); RotateLeft(g); } } } if(_root!=null) Recolour(_root,false);
        }
        public IEnumerable<KeyValuePair<TKey, TValue>> InOrder() { foreach(var x in Walk(_root)) yield return x; }
        private static IEnumerable<KeyValuePair<TKey,TValue>> Walk(Node? n) { if(n==null)yield break; foreach(var x in Walk(n.Left))yield return x; yield return new(n.Key,n.Value); foreach(var x in Walk(n.Right))yield return x; }
        public IEnumerable<KeyValuePair<TKey, TValue>> Range(TKey from, TKey to) => InOrder().Where(x => x.Key.CompareTo(from) >= 0 && x.Key.CompareTo(to) <= 0);
    }

    public sealed class MaxHeap<T>
    {
        private readonly List<T> _items = new(); private readonly Comparison<T> _comparison;
        public MaxHeap(Comparison<T> comparison) => _comparison = comparison;
        public int Count => _items.Count; public T Peek() => _items.Count == 0 ? throw new InvalidOperationException("Heap is empty.") : _items[0];
        public void Add(T item) { _items.Add(item); Up(_items.Count - 1); }
        public T Extract() { var r = Peek(); var last = _items[^1]; _items.RemoveAt(_items.Count - 1); if (_items.Count > 0) { _items[0] = last; Down(0); } return r; }
        private void Up(int i) { while (i > 0) { int p = (i - 1) / 2; if (_comparison(_items[i], _items[p]) <= 0) break; (_items[i], _items[p]) = (_items[p], _items[i]); i = p; } }
        private void Down(int i) { while (true) { int l = i * 2 + 1, r = l + 1, b = i; if (l < _items.Count && _comparison(_items[l], _items[b]) > 0) b = l; if (r < _items.Count && _comparison(_items[r], _items[b]) > 0) b = r; if (b == i) return; (_items[i], _items[b]) = (_items[b], _items[i]); i = b; } }
        public IEnumerable<T> Snapshot() => _items.ToList();
    }

    public sealed class WeightedGraph<T> where T : notnull
    {
        private readonly Dictionary<T, List<(T To, double Weight)>> _edges = new();
        public IEnumerable<T> Nodes => _edges.Keys; public void AddNode(T n) { if (!_edges.ContainsKey(n)) _edges[n] = new(); }
        public void AddEdge(T a, T b, double weight = 1) { AddNode(a); AddNode(b); _edges[a].Add((b, weight)); _edges[b].Add((a, weight)); }
        public IReadOnlyList<T> BreadthFirst(T start) => Traverse(start, false); public IReadOnlyList<T> DepthFirst(T start) => Traverse(start, true);
        private IReadOnlyList<T> Traverse(T start, bool depth) { var result = new List<T>(); if (!_edges.ContainsKey(start)) return result; var seen = new HashSet<T>(); var pending = new Stack<T>(); pending.Push(start); while (pending.Count > 0) { var n = pending.Pop(); if (!seen.Add(n)) continue; result.Add(n); var next = _edges[n].Select(x => x.To).Reverse(); foreach (var x in next) pending.Push(x); } return result; }
        public List<(T From, T To, double Weight)> MinimumSpanningForest(IEnumerable<T> included) { var set = new HashSet<T>(included); var edges = _edges.SelectMany(p => p.Value.Select(e => (From: p.Key, e.To, e.Weight))).Where(e => set.Contains(e.From) && set.Contains(e.To) && Comparer<T>.Default.Compare(e.From, e.To) < 0).OrderBy(e => e.Weight).ToList(); var uf = new UnionFind<T>(set); var result = new List<(T, T, double)>(); foreach (var e in edges) if (uf.Union(e.From, e.To)) result.Add(e); return result; }
    }
    public sealed class UnionFind<T> where T : notnull
    { private readonly Dictionary<T, T> _parent = new(); public UnionFind(IEnumerable<T> values) { foreach (var v in values) _parent[v] = v; } private T Find(T x) { if (!EqualityComparer<T>.Default.Equals(_parent[x], x)) _parent[x] = Find(_parent[x]); return _parent[x]; } public bool Union(T a, T b) { var x = Find(a); var y = Find(b); if (EqualityComparer<T>.Default.Equals(x, y)) return false; _parent[x] = y; return true; } }
}
