namespace Localwire.AlgoToolkit.Graphs
{
    using System;
    using System.Linq;

    public class UndirectedCyclicGraph<TKey> : Graph<TKey> where TKey : struct
    {
        public override bool AddEdge(TKey firstNodeKey, TKey secondNodeKey)
        {
            if (!_nodes.ContainsKey(firstNodeKey) || !_nodes.ContainsKey(secondNodeKey))
            {
                return false;
            }

            _nodes[firstNodeKey].AddNeighbour(_nodes[secondNodeKey]);
            _nodes[secondNodeKey].AddNeighbour(_nodes[firstNodeKey]);
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

        public bool CanCombineWith(UndirectedCyclicGraph<TKey> anotherGraph)
        {
            if (anotherGraph == null) return false;
            if (anotherGraph.Equals(this)) return false;
            return anotherGraph.Nodes.Values.Any(node => HasNode(node));
        }


        public bool ConnectAnotherGraphToMe(UndirectedCyclicGraph<TKey> anotherGraph)
        {
            if (!CanCombineWith(anotherGraph)) return false;
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
    }
}