namespace Localwire.AlgoToolkit.Structures.Graphs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class Graph<TKey> where TKey : struct
    {
        private readonly Dictionary<TKey, Node<TKey>> _nodes = new Dictionary<TKey, Node<TKey>>();

        public bool AddNode(Node<TKey> node)
        {
            if (_nodes.ContainsKey(node.Key)) return false;
            _nodes.Add(node.Key, node);
            return true;
        }

        public ReadOnlyDictionary<TKey, Node<TKey>> Nodes => new ReadOnlyDictionary<TKey, Node<TKey>>(_nodes);

        public bool AddNode(TKey nodeKey)
        {
            return AddNode(new Node<TKey>(nodeKey));
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