using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public abstract class Individual
    {
        protected double fitness = -1;
        public double Fitness
        {
            get {
				return fitness; // Récupérer la fitness d’un individu.
                }
        }

		internal List<IGene> genome; // récupérer le génome d’un individu.

		internal abstract void Mutate(); // Mutation de l’individu.

		internal abstract double Evaluate(); // Evalueation de l'individu.

        public override string ToString()
        {
            String gen = fitness + " : ";
            gen += String.Join(" - ", genome);
            return gen;
			/*On se contente d’afficher la valeur de fitness, suivie du génome. Pour cela, on utilise 
			 *la fonction Join() qui permet de transformer une liste en une chaîne avec le délimiteur choisi.*/
        }
    }
    
}
