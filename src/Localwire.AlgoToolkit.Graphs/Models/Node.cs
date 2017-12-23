namespace Localwire.AlgoToolkit.Graphs
{
    using Localwire.AlgoToolkit.Graphs.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class Node<TKey, TGraph> 
        where TKey : struct
        where TGraph : IGraph<TKey>
    {
        public readonly TKey Key;
        private readonly Dictionary<TKey, Node<TKey, TGraph>> _neighbours = new Dictionary<TKey, Node<TKey, TGraph>>();

        public readonly HashSet<TGraph> GraphsThatIncludeThisNode = new HashSet<TGraph>();

        public Node(TKey key)
        {
            if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
                throw new ArgumentNullException(nameof(key));
            Key = key;
        }

        public ReadOnlyDictionary<TKey, Node<TKey, TGraph>> Neighbours => new ReadOnlyDictionary<TKey, Node<TKey, TGraph>>(_neighbours);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var casted = obj as Node<TKey, TGraph>;
            return casted == null ? false : Equals(casted);
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public bool Equals(Node<TKey, TGraph> otherNode)
        {
            if (otherNode == null) return false;
            return Key.Equals(otherNode.Key);
        }

        public bool AddNeighbour(Node<TKey, TGraph> node)
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

        public bool RemoveNeighbour(Node<TKey, TGraph> node)
        {
            if (node == null) return false;
            return RemoveNeighbour(node.Key);
        }

        public bool HasNeighbour(TKey nodeKey)
        {
            return _neighbours.ContainsKey(nodeKey);
        }

        public bool HasNeighbour(Node<TKey, TGraph> node)
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

            this.CombineGraphs();
            
            return GraphsThatIncludeThisNode.Count;
        }
    }
}