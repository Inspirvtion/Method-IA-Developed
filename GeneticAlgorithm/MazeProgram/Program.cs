using GeneticAlgorithm;
using System;

namespace MazeProgram
{
    class Program : IIHM
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Run();
        }

        public void Run()
        {
            // Init
            Parameters.crossoverRate = 0.6;
            Parameters.mutationsRate = 0.1;
            Parameters.mutationAddRate = 0.2;
            Parameters.mutationDeleteRate = 0.1;
            Parameters.minFitness = 0; //La fitness minimale visée est nulle, c’est-à-dire que l’on arrive sur la case de sortie.
            EvolutionaryProcess geneticAlgoMaze = new EvolutionaryProcess(this, "Maze");

            // Lancement
            geneticAlgoMaze.Run();

            //Init
            Parameters.crossoverRate = 0.0;
            Parameters.mutationsRate = 0.3;
            Parameters.mutationAddRate = 0.0;
            Parameters.mutationDeleteRate = 0.0;
            Parameters.minFitness = 2579;
            EvolutionaryProcess geneticAlgoTSP = new EvolutionaryProcess(this, "TSP");

            // Lancement
            geneticAlgoTSP.Run();

            while (true) ;
        }

        public void PrintBestIndividual(Individual individual, int generation)
        {
            Console.WriteLine(generation + " -> " + individual);
        }
    }
}
