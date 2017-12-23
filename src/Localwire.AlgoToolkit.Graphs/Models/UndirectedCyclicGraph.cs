namespace Localwire.AlgoToolkit.Graphs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class UndirectedCyclicGraph<TKey> : IGraph<TKey> where TKey : struct
    {
        protected readonly Dictionary<TKey, Node<TKey, UndirectedCyclicGraph<TKey>>> _nodes = new Dictionary<TKey, Node<TKey, UndirectedCyclicGraph<TKey>>>();

        public ReadOnlyDictionary<TKey, Node<TKey, UndirectedCyclicGraph<TKey>>> Nodes => new ReadOnlyDictionary<TKey, Node<TKey, UndirectedCyclicGraph<TKey>>>(_nodes);

        public bool AddNode(TKey nodeKey)
        {
            if (_nodes.ContainsKey(nodeKey)) return false;
            return AddNode(new Node<TKey, UndirectedCyclicGraph<TKey>>(nodeKey));
        }

        public bool AddNode(Node<TKey, UndirectedCyclicGraph<TKey>> node)
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

        public bool HasNode(Node<TKey, UndirectedCyclicGraph<TKey>> node)
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

        public bool AddEdgeWithNodes(Node<TKey, UndirectedCyclicGraph<TKey>> firstNode, Node<TKey, UndirectedCyclicGraph<TKey>> secondNode)
        {
            AddNode(firstNode);
            AddNode(secondNode);
            return AddEdge(firstNode, secondNode);
        }

        public bool AddEdge(TKey firstNodeKey, TKey secondNodeKey)
        {
            if (!HasNode(firstNodeKey) || !HasNode(secondNodeKey))
            {
                return false;
            }

            _nodes[firstNodeKey].AddNeighbour(_nodes[secondNodeKey]);
            _nodes[secondNodeKey].AddNeighbour(_nodes[firstNodeKey]);
            return true;
        }

        public bool AddEdge(Node<TKey, UndirectedCyclicGraph<TKey>> firstNode, Node<TKey, UndirectedCyclicGraph<TKey>> secondNode)
        {
            if (!HasNode(firstNode) || !HasNode(secondNode))
            {
                return false;
            }

            _nodes[firstNode.Key].AddNeighbour(_nodes[secondNode.Key]);
            _nodes[secondNode.Key].AddNeighbour(_nodes[firstNode.Key]);
            return true;
        }

        public bool RemoveEdge(TKey firstNodeKey, TKey secondNodeKey)
        {
            if (!_nodes.ContainsKey(firstNodeKey) || !_nodes.ContainsKey(secondNodeKey))
            {
                return false;
            }

            if (!_nodes[firstNodeKey].HasNeighbour(secondNodeKey) || !_nodes[secondNodeKey].HasNeighbour(firstNodeKey))
            {
                return false;
            }

            _nodes[firstNodeKey].RemoveNeighbour(secondNodeKey);
            _nodes[secondNodeKey].RemoveNeighbour(firstNodeKey);
            return true;
        }

        public bool ClearNodes()
        {
            foreach (var node in _nodes.Values)
            {
                node.GraphsThatIncludeThisNode.Remove(this);
            }
            _nodes.Clear();
            return true;
        }

        public bool CanCombineWith(UndirectedCyclicGraph<TKey> anotherGraph, Node<TKey, UndirectedCyclicGraph<TKey>> edgeFirstNode, Node<TKey, UndirectedCyclicGraph<TKey>> edgeSecondNode)
        {
            if (anotherGraph == null) return false;
            if (anotherGraph.Equals(this)) return false;
            return (edgeFirstNode.GraphsThatIncludeThisNode.Contains(this) && edgeFirstNode.GraphsThatIncludeThisNode.Contains(anotherGraph))
                || (edgeSecondNode.GraphsThatIncludeThisNode.Contains(this) && edgeSecondNode.GraphsThatIncludeThisNode.Contains(anotherGraph));
        }

        public bool ConnectAnotherGraphToMe(UndirectedCyclicGraph<TKey> anotherGraph, Node<TKey, UndirectedCyclicGraph<TKey>> edgeFirstNode, Node<TKey, UndirectedCyclicGraph<TKey>> edgeSecondNode)
        {
            if (!CanCombineWith(anotherGraph, edgeFirstNode, edgeSecondNode)) return false;
            foreach (var node in anotherGraph.Nodes.Values)
            {
                AddNode(node);
            }
            return anotherGraph.ClearNodes();
        }

        public static UndirectedCyclicGraph<TKey> CreateNewFromFirstEdge(Tuple<TKey, TKey> edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));
                
            var graph = new UndirectedCyclicGraph<TKey>();
            graph.AddEdgeWithNodes(edge.Item1, edge.Item2);
            return graph;
        }

        public static UndirectedCyclicGraph<TKey> CreateNewFromFirstEdge(Node<TKey, UndirectedCyclicGraph<TKey>> firstNode, Node<TKey, UndirectedCyclicGraph<TKey>> secondNode)
        {
            var graph = new UndirectedCyclicGraph<TKey>();
            graph.AddEdgeWithNodes(firstNode, secondNode);
            return graph;
        }
    }
}