namespace Localwire.AlgoToolkit.Graphs
{
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
            return anotherGraph.Nodes.Values.Any(node => HasNode(node));
        }


        public bool CombineGraphs(UndirectedCyclicGraph<TKey> anotherGraph)
        {
            if (!CanCombineWith(anotherGraph)) return false;
            foreach (var node in anotherGraph.Nodes.Values)
            {
                AddNode(node);
            }
            return true;
        }
    }
}