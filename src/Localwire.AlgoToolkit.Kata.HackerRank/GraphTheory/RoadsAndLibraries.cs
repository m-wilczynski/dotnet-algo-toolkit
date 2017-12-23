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
                    IEnumerable<Tuple<int, int>> roads = GetRoadsFrom(input, numOfRoads);
                    Console.WriteLine(SolveCase(numOfCities, numOfRoads, libCost, roadCost, roads));
                }
            }
        }

        //At least makes it testable
        private IEnumerable<Tuple<int, int>> GetRoadsFrom(TextReader input, int numOfRoads)
        {
            for (int a1 = 0; a1 < numOfRoads; a1++)
            {
                string[] tokens_city_1 = input.ReadLine().Split(' ');
                int city_1 = Convert.ToInt32(tokens_city_1[0]);
                int city_2 = Convert.ToInt32(tokens_city_1[1]);
                yield return new Tuple<int, int>(city_1, city_2);
            }
        }

        public long SolveCase(int numOfCities, int numOfRoads, long libCost, long roadCost, IEnumerable<Tuple<int, int>> roads)
        {
            var easyCasesResult = CheckEasyCases(numOfCities, libCost, roadCost, roads);
            if (easyCasesResult.HasValue) return easyCasesResult.Value;

            HashSet<UndirectedCyclicGraph<int>> graphs = new HashSet<UndirectedCyclicGraph<int>>();

            foreach (var road in roads)
            {
                if (graphs.Count == 0)
                {
                    graphs.Add(UndirectedCyclicGraph<int>.CreateNewFromFirstEdge(road));
                }
                else 
                {
                    var graph = graphs.FirstOrDefault(gr => gr.HasNode(road.Item1) || gr.HasNode(road.Item2));
                    if (graph == null)
                    {
                       graphs.Add(UndirectedCyclicGraph<int>.CreateNewFromFirstEdge(road)); 
                    }
                    else
                    {
                        graph.AddEdgeWithNodes(road.Item1, road.Item2);
                        CombineAllGraphsYouCan(graphs, graph);
                    }
                }
            }

            return graphs.Sum(graph => roadCost * (graph.Nodes.Count - 1) + libCost);
        }

        private static void CombineAllGraphsYouCan(HashSet<UndirectedCyclicGraph<int>> allGraphs, UndirectedCyclicGraph<int> modifiedGraph)
        {
            var graphsToRemove = new HashSet<UndirectedCyclicGraph<int>>();

            foreach (var existingGraph in allGraphs.Where(gr => gr.CanCombineWith(modifiedGraph)))
            {
                modifiedGraph.ConnectAnotherGraphToMe(existingGraph);
                graphsToRemove.Add(existingGraph);
            }
            foreach (var graphToRemove in graphsToRemove)
            {
                allGraphs.Remove(graphToRemove);
            }
        }

        private long? CheckEasyCases(int numOfCities, long libCost, long roadCost, IEnumerable<Tuple<int, int>> roads)
        {
            if (roadCost == 0)
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
    }
}