namespace Localwire.AlgoToolkit.Structures.Graphs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Node<TKey> where TKey : struct
    {
        public readonly TKey Key;
        private readonly Dictionary<TKey, Node<TKey>> _neighbours = new Dictionary<TKey, Node<TKey>>();

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
    }
}