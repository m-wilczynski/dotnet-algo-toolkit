using System;
using System.Collections.Generic;
using System.IO;

namespace Localwire.AlgoToolkit.Kata.HackerRank
{
    public class RoadsAndLibraries
    {
        public void SolveFromInput(StreamReader input)
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
                    SolveCase(numOfCities, numOfRoads, libCost, roadCost, roads);
                }
            }
        }

        public int SolveCase(int numOfCities, int numOfRoads, long libCost, long roadCost, IEnumerable<Tuple<int, int>> roads)
        {
            return 0;
        }

        //At least makes it testable
        private IEnumerable<Tuple<int, int>> GetRoadsFrom(StreamReader input, int numOfRoads)
        {
            for (int a1 = 0; a1 < numOfRoads; a1++)
            {
                string[] tokens_city_1 = input.ReadLine().Split(' ');
                int city_1 = Convert.ToInt32(tokens_city_1[0]);
                int city_2 = Convert.ToInt32(tokens_city_1[1]);
                yield return new Tuple<int, int>(city_1, city_2);
            }
        }
    }
}