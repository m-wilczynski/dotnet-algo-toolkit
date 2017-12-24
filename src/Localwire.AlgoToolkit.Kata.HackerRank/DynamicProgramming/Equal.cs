using System;
using System.IO;
using System.Linq;

namespace Localwire.AlgoToolkit.Kata.HackerRank.DynamicProgramming
{
    public class Equal
    {
        public const int ONE = 1;
        public const int TWO = 2;
        public const int FIVE = 5;

        public void SolveFromInput(TextReader input)
        {
            using (input)
            {                
                int numOfTests = Convert.ToInt32(input.ReadLine());
                for (int a0 = 0; a0 < numOfTests; a0++)
                {
                    int numOfCollegues = Convert.ToInt32(input.ReadLine());
                    int[] initialChocolates = Console.ReadLine().Split(' ').Select(el => Convert.ToInt32(el)).ToArray();
                    Console.WriteLine(SolveCase(numOfCollegues, initialChocolates));
                }
            }
        }

        public int SolveCase(int numOfCollegues, int[] initialChocolates)
        {
            int min = initialChocolates.Min(), max = initialChocolates.Max();

            while (initialChocolates.All(choco => choco != initialChocolates.Max()))
            {
                //TODO: Comeback to me :(
                return 0;
            }
            return 0;
        }

        public static int[] IncreaseByOneExceptOneIndex(int[] array, int indexToSkip)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i == indexToSkip) continue;
                array[i] += ONE;
            }
            return array;
        }

        public static int[] IncreaseByTwoExceptOneIndex(int[] array, int indexToSkip)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i == indexToSkip) continue;
                array[i] += TWO;
            }
            return array;
        }

        public static int[] IncreaseByFiveExceptOneIndex(int[] array, int indexToSkip)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i == indexToSkip) continue;
                array[i] += FIVE;
            }
            return array;
        }


    }
}
