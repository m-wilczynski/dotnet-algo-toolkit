namespace Localwire.AlgoToolkit.Structures.Graphs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public abstract class Graph<TKey> where TKey : struct
    {
        protected readonly Dictionary<TKey, Node<TKey>> _nodes = new Dictionary<TKey, Node<TKey>>();

        public ReadOnlyDictionary<TKey, Node<TKey>> Nodes => new ReadOnlyDictionary<TKey, Node<TKey>>(_nodes);

        public abstract bool AddEdge(TKey firstNodeKey, TKey secondNodeKey);
        public abstract bool RemoveEdge(TKey firstNodeKey, TKey secondNodeKey);

        public bool AddNode(TKey nodeKey)
        {
            if (_nodes.ContainsKey(nodeKey)) return false;
            return AddNode(new Node<TKey>(nodeKey));
        }

        public bool AddNode(Node<TKey> node)
        {
            if (node == null) return false;
            if (_nodes.ContainsKey(node.Key)) return false;
            _nodes.Add(node.Key, node);
            return true;
        }

        public bool RemoveNode(TKey nodeKey)
        {
            if (!_nodes.ContainsKey(nodeKey)) return false;
            _nodes.Remove(nodeKey);
            return true;
        }

        public bool HasNode(TKey nodeKey)
        {
            return _nodes.ContainsKey(nodeKey);
        }
    }
}