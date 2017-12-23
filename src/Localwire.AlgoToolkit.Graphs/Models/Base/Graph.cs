namespace Localwire.AlgoToolkit.Graphs
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public abstract class Graph<TKey> where TKey : struct
    {
        protected readonly Dictionary<TKey, Node<TKey>> _nodes = new Dictionary<TKey, Node<TKey>>();

        public ReadOnlyDictionary<TKey, Node<TKey>> Nodes => new ReadOnlyDictionary<TKey, Node<TKey>>(_nodes);

        public abstract bool AddEdge(TKey firstNodeKey, TKey secondNodeKey);
        public abstract bool AddEdge(Node<TKey> firstNode, Node<TKey> secondNode);
        public abstract bool RemoveEdge(TKey firstNodeKey, TKey secondNodeKey);
        public abstract bool ClearNodes();

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
            node.GraphsThatIncludeThisNode.Add(this);
            return true;
        }

        public bool RemoveNode(TKey nodeKey)
        {
            if (!_nodes.ContainsKey(nodeKey)) return false;
            _nodes[nodeKey].GraphsThatIncludeThisNode.Remove(this);
            _nodes.Remove(nodeKey);
            return true;
        }

        public bool HasNode(TKey nodeKey)
        {
            return _nodes.ContainsKey(nodeKey);
        }

        public bool HasNode(Node<TKey> node)
        {
            if (node == null) return false;
            return HasNode(node.Key);
        }

        public bool AddEdgeWithNodes(TKey firstNodeKey, TKey secondNodeKey)
        {
            AddNode(firstNodeKey);
            AddNode(secondNodeKey);
            return AddEdge(firstNodeKey, secondNodeKey);
        }

        public bool AddEdgeWithNodes(Node<TKey> firstNode, Node<TKey> secondNode)
        {
            AddNode(firstNode);
            AddNode(secondNode);
            return AddEdge(firstNode, secondNode);
        }
    }
}