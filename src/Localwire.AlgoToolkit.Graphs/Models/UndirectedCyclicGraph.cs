namespace Localwire.AlgoToolkit.Graphs
{
    using System;
    using System.Linq;

    public class UndirectedCyclicGraph<TKey> : Graph<TKey> where TKey : struct
    {
        public override bool AddEdge(TKey firstNodeKey, TKey secondNodeKey)
        {
            if (!HasNode(firstNodeKey) || !HasNode(secondNodeKey))
            {
                return false;
            }

            _nodes[firstNodeKey].AddNeighbour(_nodes[secondNodeKey]);
            _nodes[secondNodeKey].AddNeighbour(_nodes[firstNodeKey]);
            return true;
        }

        public override bool AddEdge(Node<TKey> firstNode, Node<TKey> secondNode)
        {
            if (!HasNode(firstNode) || !HasNode(secondNode))
            {
                return false;
            }

            _nodes[firstNode.Key].AddNeighbour(_nodes[secondNode.Key]);
            _nodes[secondNode.Key].AddNeighbour(_nodes[firstNode.Key]);
            return true;
        }

        public override bool RemoveEdge(TKey firstNodeKey, TKey secondNodeKey)
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

        public bool CanCombineWith(UndirectedCyclicGraph<TKey> anotherGraph, Node<TKey> edgeFirstNode, Node<TKey> edgeSecondNode)
        {
            if (anotherGraph == null) return false;
            if (anotherGraph.Equals(this)) return false;
            return (edgeFirstNode.GraphsThatIncludeThisNode.Contains(this) && edgeFirstNode.GraphsThatIncludeThisNode.Contains(anotherGraph))
                || (edgeSecondNode.GraphsThatIncludeThisNode.Contains(this) && edgeSecondNode.GraphsThatIncludeThisNode.Contains(anotherGraph));
        }

        public bool ConnectAnotherGraphToMe(UndirectedCyclicGraph<TKey> anotherGraph, Node<TKey> edgeFirstNode, Node<TKey> edgeSecondNode)
        {
            if (!CanCombineWith(anotherGraph, edgeFirstNode, edgeSecondNode)) return false;
            foreach (var node in anotherGraph.Nodes.Values)
            {
                AddNode(node);
                node.GraphsThatIncludeThisNode.Remove(anotherGraph);
            }
            return true;
        }

        public bool CanCombineWith(UndirectedCyclicGraph<TKey> anotherGraph, TKey nodeCausingCombine)
        {
            if (anotherGraph == null) return false;
            if (anotherGraph.Equals(this)) return false;
            return anotherGraph.HasNode(nodeCausingCombine);
        }

        public bool ConnectAnotherGraphToMe(UndirectedCyclicGraph<TKey> anotherGraph, TKey nodeCausingCombine)
        {
            if (!CanCombineWith(anotherGraph, nodeCausingCombine)) return false;
            foreach (var node in anotherGraph.Nodes.Values)
            {
                AddNode(node);
            }
            return true;
        }

        public static UndirectedCyclicGraph<TKey> CreateNewFromFirstEdge(Tuple<TKey, TKey> edge)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));
                
            var graph = new UndirectedCyclicGraph<TKey>();
            graph.AddEdgeWithNodes(edge.Item1, edge.Item2);
            return graph;
        }

        public static UndirectedCyclicGraph<TKey> CreateNewFromFirstEdge(Node<TKey> firstNode, Node<TKey> secondNode)
        {
            var graph = new UndirectedCyclicGraph<TKey>();
            graph.AddEdgeWithNodes(firstNode, secondNode);
            return graph;
        }
    }
}