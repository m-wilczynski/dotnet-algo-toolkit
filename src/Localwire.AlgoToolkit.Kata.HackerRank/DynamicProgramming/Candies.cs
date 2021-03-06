﻿namespace Localwire.AlgoToolkit.Kata.HackerRank.DynamicProgramming
{
    using System;
    using System.IO;
    using System.Linq;

    public class Candies
    {
        public void SolveFromInput(TextReader input)
        {
            using (input)
            {
                int n = Convert.ToInt32(input.ReadLine());
                int[] childrenScores = new int[n];
                for (int arr_i = 0; arr_i < n; arr_i++)
                {
                    childrenScores[arr_i] = Convert.ToInt32(input.ReadLine());
                }

                Console.WriteLine(SolveCase(childrenScores));
            }
        }

        public long SolveCase(int[] childrenScores)
        {
            var childrenCandies = new ChildrenCandies(childrenScores.Length);

            if (ShouldReverse(childrenScores, 0.2))
                childrenScores = childrenScores.Reverse().ToArray();

            for (var i = 0; i < childrenScores.Length; i++)
            {
                if (i == 0)
                {
                    childrenCandies.AddCandy(0, 1);
                    continue;
                }
                childrenCandies.AddCandy(i, HowManyCandiesToGive(childrenScores, i, childrenCandies));
            }

            return childrenCandies.TotalCandies;
        }

        public int HowManyCandiesToGive(int[] childrenScores, int childIndex, ChildrenCandies childrenCandies)
        {
            if (childIndex == 0 || childrenScores[childIndex - 1] == childrenScores[childIndex]) return 1;
            if (childrenScores[childIndex - 1] < childrenScores[childIndex])
            {
                return childrenCandies.CandiesOf(childIndex - 1) + 1;
            }
            else if (childrenScores[childIndex - 1] > childrenScores[childIndex])
            {
                GoBackAndFixStuff(childrenScores, childIndex, childrenCandies);
                return 1;
            }
            throw new InvalidOperationException("This should be unreachable");
        }

        private void GoBackAndFixStuff(int[] childrenScores, int childIndex, ChildrenCandies childrenCandies)
        {
            if (childIndex == 1)
            {
                childrenCandies.AddCandy(0, 1);
                return;
            }

            var currentChildCandies = childrenCandies.CandiesOf(childIndex);
            //Apply setting minimal candy prematurely for check below
            if (currentChildCandies == 0) currentChildCandies = 1;

            if (childrenCandies.CandiesOf(childIndex - 1) <= currentChildCandies)
            {
                childrenCandies.AddCandy(childIndex - 1, 1);
            }

            if (childrenScores[childIndex - 2] > childrenScores[childIndex - 1])
            {
                GoBackAndFixStuff(childrenScores, childIndex - 1, childrenCandies);
            }
        }

        private bool ShouldReverse(int[] array, double percentToCheckForPattern)
        {
            for (var i = 0; i < array.Length * percentToCheckForPattern; i++)
            {
                if (i == 0) continue;
                if (array[i] > array[i - 1]) return false;
            }
            return true;
        }

        public class ChildrenCandies
        {
            private long _totalCandies = 0;
            private readonly int[] _childrenCandiesCount;

            public long TotalCandies => _totalCandies;
            private long SecretTotal => _childrenCandiesCount.Sum();

            public ChildrenCandies(int childrenCount)
            {
                _childrenCandiesCount = new int[childrenCount];
            }

            public void AddCandy(int childIndex, int numOfCandies)
            {
                _childrenCandiesCount[childIndex] += numOfCandies;
                _totalCandies += numOfCandies;
            }

            public int CandiesOf(int childIndex)
            {
                return _childrenCandiesCount[childIndex];
            }
        }
    }
}
