using MetaheuristicsPCL;
using System;

namespace MetaheuristicsMainProgram
{
    class Program : IHM
    {
        static void Main(string[] _args)
        {
            Program p = new Program();
            p.Run();
        }

        protected void Run()
        {
            Console.Out.WriteLine("Métaheuristiques d'optimisation\n");

            IProblem kspb = new KnapsackProblem();
            RunAlgorithms(kspb);

            Console.Out.WriteLine("*****************************************************************\n");

            IProblem kspbRandom = new KnapsackProblem(100, 30, 20);
            RunAlgorithms(kspbRandom);

            Console.Out.WriteLine("FIN");
            while (true) ;
        }

        private void RunAlgorithms(IProblem _pb)
        {
            Algorithm algo;

            Console.Out.WriteLine("Algorithme glouton");
            algo = new GreedyAlgorithmForKnapsack();
            algo.Solve(_pb, this);
            Console.Out.WriteLine("");

            Console.Out.WriteLine("Descente de gradient");
            algo = new GradientDescentForKnapsack();
            algo.Solve(_pb, this);
            Console.Out.WriteLine("");

            Console.Out.WriteLine("Recherche tabou");
            algo = new TabuSearchForKnapsack();
            algo.Solve(_pb, this);
            Console.Out.WriteLine("");

            Console.Out.WriteLine("Recuit simulé");
            algo = new SimulatedAnnealingForKnapsack();
            algo.Solve(_pb, this);
            Console.Out.WriteLine("");

            Console.Out.WriteLine("Optimisation par essaim particulaire");
            algo = new ParticleSwarmOptimizationForKnapsack();
            algo.Solve(_pb, this);
            Console.Out.WriteLine("");
        }

        public void PrintMessage(String _message)
        {
            Console.Out.WriteLine(_message);
        }
    }
}
