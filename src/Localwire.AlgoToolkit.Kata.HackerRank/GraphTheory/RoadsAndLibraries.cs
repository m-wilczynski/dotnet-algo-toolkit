namespace Localwire.AlgoToolkit.Kata.HackerRank
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Localwire.AlgoToolkit.Graphs;

    public class RoadsAndLibraries
    {
        public void SolveFromInput(TextReader input)
        {
            using (input)
            {
                int q = Convert.ToInt32(input.ReadLine());
                for (int a0 = 0; a0 < q; a0++)
                {
                    string[] tokens_n = input.ReadLine().Split(' ');
                    int numOfCities = Convert.ToInt32(tokens_n[0]);
                    int numOfRoads = Convert.ToInt32(tokens_n[1]);
                    long libCost = Convert.ToInt64(tokens_n[2]);
                    long roadCost = Convert.ToInt64(tokens_n[3]);
                    IEnumerable<Tuple<int, int>> roads = GetRoadsFrom(input, numOfRoads, IsEasyCase(numOfRoads, libCost, roadCost));
                    Console.WriteLine(SolveCase(numOfCities, numOfRoads, libCost, roadCost, roads));
                }
            }
        }

        //At least makes it testable
        private IEnumerable<Tuple<int, int>> GetRoadsFrom(TextReader input, int numOfRoads, bool rewinding = false)
        {
            for (int a1 = 0; a1 < numOfRoads; a1++)
            {
                string[] tokens_city_1 = input.ReadLine().Split(' ');
                if (!rewinding)
                {
                    int city_1 = Convert.ToInt32(tokens_city_1[0]);
                    int city_2 = Convert.ToInt32(tokens_city_1[1]);
                    yield return new Tuple<int, int>(city_1, city_2);
                }
                else
                {
                    yield return null;
                }
            }
        }

        public long SolveCase(int numOfCities, int numOfRoads, long libCost, long roadCost, IEnumerable<Tuple<int, int>> roads)
        {
            var easyCasesResult = CheckEasyCases(numOfCities, numOfRoads, libCost, roadCost, roads);
            if (easyCasesResult.HasValue) return easyCasesResult.Value;

            HashSet<UndirectedCyclicGraph<int>> graphs = new HashSet<UndirectedCyclicGraph<int>>();
            Node<int>[] nodes = Enumerable.Range(1, numOfCities).Select(idx => new Node<int>(idx)).ToArray();

            foreach (var road in roads)
            {
                var firstNode = nodes[road.Item1 - 1];
                var secondNode = nodes[road.Item2 - 1];

                if (graphs.Count == 0)
                {
                    graphs.Add(UndirectedCyclicGraph<int>.CreateNewFromFirstEdge(firstNode, secondNode));
                }
                else 
                {
                    UndirectedCyclicGraph<int> graph = firstNode.GraphsThatIncludeThisNode.FirstOrDefault() as UndirectedCyclicGraph<int> ?? 
                        secondNode.GraphsThatIncludeThisNode.FirstOrDefault() as UndirectedCyclicGraph<int>;

                    if (graph == null)
                    {
                       graphs.Add(UndirectedCyclicGraph<int>.CreateNewFromFirstEdge(firstNode, secondNode)); 
                    }
                    else
                    {
                        graph.AddEdgeWithNodes(firstNode, secondNode);
                        CombineAllGraphsYouCan(graphs, graph, firstNode, secondNode);
                    }
                }
            }

            return graphs.Sum(graph => roadCost * (graph.Nodes.Count - 1) + libCost) + nodes.Count(n => n.GraphsThatIncludeThisNode.Count == 0) * libCost;
        }

        private static void CombineAllGraphsContaining(HashSet<Node<int>> nodes)
        {
            foreach (var node in nodes)
            {

            }
        }

        private static void CombineAllGraphsYouCan(HashSet<UndirectedCyclicGraph<int>> allGraphs, 
            UndirectedCyclicGraph<int> modifiedGraph, Node<int> edgeFirstNode, Node<int> edgeSecondNode)
        {
            var graphsToRemove = new HashSet<UndirectedCyclicGraph<int>>();

            foreach (var existingGraph in allGraphs)
            {
                if (modifiedGraph.ConnectAnotherGraphToMe(existingGraph, edgeFirstNode, edgeSecondNode))
                    graphsToRemove.Add(existingGraph);
            }
            foreach (var graphToRemove in graphsToRemove)
            {
                allGraphs.Remove(graphToRemove);
            }
        }

        private long? CheckEasyCases(int numOfCities, int numOfRoads, long libCost, long roadCost, IEnumerable<Tuple<int, int>> roads)
        {
            if (numOfRoads == 0)
            {
                //Rewind
                foreach (var road in roads) { }
                return libCost * numOfCities;
            }
            if (libCost <= roadCost)
            {
                //Rewind
                foreach (var road in roads) { }
                return libCost * numOfCities;
            }
            return null;
        }

        private bool IsEasyCase(int numOfRoads, long libCost, long roadCost)
        {
            return numOfRoads == 0 || libCost <= roadCost;
        }
    }
}