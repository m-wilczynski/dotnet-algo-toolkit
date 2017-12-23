namespace Localwire.AlgoToolkit.Console.Playground
{
    using System;
    using System.IO;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Environments;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Running;
    using Localwire.AlgoToolkit.Kata.HackerRank;

    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(BenchmarkRunner.Run<BenchmarkDummy>());
            using (TextReader input = File.OpenText(@"C:\\Users\\micha_000\\Desktop\\input01.txt"))
            {
                new RoadsAndLibraries().SolveFromInput(input);
            }
            Console.WriteLine("DONE");
            Console.ReadLine();
        }
    }

    [Config(typeof(DummyConfig))]
    public class BenchmarkDummy
    {
        [Benchmark]
        public void RunBenchmark() 
        {
            using (TextReader input = File.OpenText(@"C:\\Users\\micha_000\\Desktop\\input04.txt"))
            {
                new RoadsAndLibraries().SolveFromInput(input);
            }
            Console.WriteLine("DONE");
        }

        private class DummyConfig : ManualConfig
        {
            public DummyConfig()
            {
                Add(new Job(EnvMode.RyuJitX64, EnvMode.Clr, RunMode.Dry)
                    {
                        Env = { Runtime = Runtime.Core },
                        Run = { LaunchCount = 2, WarmupCount = 1, TargetCount = 1 }
                    });
            }
        }
    }
}
