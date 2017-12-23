using System;
using System.Linq;

namespace Localwire.AlgoToolkit.Graphs.Extensions
{
    public static class CombiningExtensions
    {
        public static void CombineGraphs<TKey, TGraph>(this Node<TKey, TGraph> node) 
            where TKey : struct
            where TGraph : IGraph<TKey>
        {
            if (typeof(TGraph) == typeof(UndirectedCyclicGraph<TKey>))
            {
                CombineUndirectedCyclicGraphs(node as Node<TKey, UndirectedCyclicGraph<TKey>>);
                return;
            }

            throw new NotImplementedException();
        }

        private static void CombineUndirectedCyclicGraphs<TKey>(this Node<TKey, UndirectedCyclicGraph<TKey>> node)
            where TKey : struct
        {
            UndirectedCyclicGraph<TKey> firstGraph = node.GraphsThatIncludeThisNode
                                             .Cast<UndirectedCyclicGraph<TKey>>().First();

            foreach (var graphToCombineFrom in node.GraphsThatIncludeThisNode
                .Cast<UndirectedCyclicGraph<TKey>>()
                .Where(n => !n.Equals(firstGraph))
                .ToList())
            {
                foreach (var graphsNode in graphToCombineFrom.Nodes.Values)
                {
                    firstGraph.AddNode(graphsNode);
                }
                graphToCombineFrom.ClearNodes();
            }
        }
    }
}
