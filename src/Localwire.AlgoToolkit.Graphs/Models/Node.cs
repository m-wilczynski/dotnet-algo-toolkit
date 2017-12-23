namespace Localwire.AlgoToolkit.Graphs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Node<TKey> where TKey : struct
    {
        public readonly TKey Key;
        private readonly Dictionary<TKey, Node<TKey>> _neighbours = new Dictionary<TKey, Node<TKey>>();

        public readonly HashSet<Graph<TKey>> GraphsThatIncludeThisNode = new HashSet<Graph<TKey>>();

        public Node(TKey key)
        {
            if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                throw new ArgumentNullException(nameof(key));
            Key = key;
        }

        public ReadOnlyDictionary<TKey, Node<TKey>> Neighbours => new ReadOnlyDictionary<TKey, Node<TKey>>(_neighbours);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var casted = obj as Node<TKey>;
            return casted == null ? false : Equals(casted);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public bool Equals(Node<TKey> otherNode)
        {
            if (otherNode == null) return false;
            return Key.Equals(otherNode.Key);
        }

        public bool AddNeighbour(Node<TKey> node)
        {
            if (node == null) return false;
            if (_neighbours.ContainsKey(node.Key)) return false;
            _neighbours.Add(node.Key, node);
            return true;
        }

        public bool RemoveNeighbour(TKey nodeKey)
        {
            if (!_neighbours.ContainsKey(nodeKey)) return false;
            _neighbours.Remove(nodeKey);
            return true;
        }

        public bool RemoveNeighbour(Node<TKey> node)
        {
            if (node == null) return false;
            return RemoveNeighbour(node.Key);
        }

        public bool HasNeighbour(TKey nodeKey)
        {
            return _neighbours.ContainsKey(nodeKey);
        }

        public bool HasNeighbour(Node<TKey> node)
        {
            if (node == null) return false;
            return HasNeighbour(node.Key);
        }

        public int CombineGraphsIBelongTo()
        {
            if (GraphsThatIncludeThisNode.Count == 1)
            {
                return 1;
            }

            if (GraphsThatIncludeThisNode.Count == 0)
            {
                return 0;
            }

            UndirectedCyclicGraph<int> firstGraph = GraphsThatIncludeThisNode.Cast<UndirectedCyclicGraph<int>>().First();

            foreach (var graphToCombineFrom in GraphsThatIncludeThisNode
                .Cast<UndirectedCyclicGraph<int>>()
                .Where(n => !n.Equals(firstGraph))
                .ToList())
            {
                foreach (var graphsNode in graphToCombineFrom.Nodes.Values)
                {
                    firstGraph.AddNode(graphsNode);
                }
                graphToCombineFrom.ClearNodes();
            }

            return GraphsThatIncludeThisNode.Count;
        }
    }
}