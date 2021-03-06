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
            Node<int, UndirectedCyclicGraph<int>>[] nodes = Enumerable.Range(1, numOfCities).Select(idx => new Node<int, UndirectedCyclicGraph<int>>(idx)).ToArray();

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
                    }
                }
            }

            var nodesWithNoRoads = CombineAllGraphsOf(nodes);

            return new HashSet<UndirectedCyclicGraph<int>>(nodes.SelectMany(n => n.GraphsThatIncludeThisNode))
                .Sum(graph => roadCost * (graph.Nodes.Count - 1) + libCost) + nodesWithNoRoads * libCost;
        }

        //Return nodes that belong to no graph (ie. have no road)
        public static int CombineAllGraphsOf(ICollection<Node<int, UndirectedCyclicGraph<int>>> nodes)
        {
            var noRoadNodes = 0;
            foreach (var node in nodes)
            {
                if (node.CombineGraphsIBelongTo() == 0)
                {
                    noRoadNodes++;
                }
            }

            return noRoadNodes;
        }

        public static long? CheckEasyCases(int numOfCities, int numOfRoads, long libCost, long roadCost, IEnumerable<Tuple<int, int>> roads)
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

        public static bool IsEasyCase(int numOfRoads, long libCost, long roadCost)
        {
            return numOfRoads == 0 || libCost <= roadCost;
        }
    }
}