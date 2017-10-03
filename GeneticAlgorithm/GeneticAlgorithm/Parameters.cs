using System;

namespace GeneticAlgorithm
{
    public static class Parameters
    {
		public static int individualsNb = 20; // Le nombre d’individus par génération
		public static int generationsMaxNb = 50; // Le nombre maximal de générations
		public static int initialGenesNb = 10; // Le nombre initial de gènes si le génome est de taille variable.
		public static int minFitness = 0; // La fitness à atteindre

		public static double mutationsRate = 0.20; // le taux de mutations. 
		public static double mutationAddRate = 0.20; // le taux d’ajout de gènes.
		public static double mutationDeleteRate = 0.10; // le taux de suppression de gènes.
		public static double crossoverRate = 0.60; //  le taux de crossover .

        public static Random randomGenerator = new Random();
		// générateur aléatoire qui pourra ensuite être utilisé dans tout le code, sans avoir à être recréé.
    }
}
