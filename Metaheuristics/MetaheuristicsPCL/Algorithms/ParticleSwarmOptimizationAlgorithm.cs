using System;
using System.Collections.Generic;

namespace MetaheuristicsPCL
{
    public abstract class ParticleSwarmOptimizationAlgorithm : Algorithm
    {
		protected List<ISolution> currentSolutions; // Les solutions courantes.
		protected ISolution bestSoFarSolution; // Meilleure solution rencontrée jusqu’alors.
		protected ISolution bestActualSolution; // Meilleure solution actuelle au sein de la population.

		protected const int NB_INDIVIDUALS = 30; // le nombre d’individus utilisés dans notre population.

        public override sealed void Solve(IProblem _pb, IHM _ihm)
        {
            base.Solve(_pb, _ihm);
            
		    // Creation de la liste de solutions courante : Population.
            currentSolutions = new List<ISolution>();
            for (int i = 0; i < NB_INDIVIDUALS; i++)
            {
                ISolution newSolution = pb.RandomSolution();
                currentSolutions.Add(newSolution);
            }

            bestActualSolution = pb.BestSolution(currentSolutions);
			// Meilleure solution actuelle au sein de la population.
			bestSoFarSolution = bestActualSolution; 
			// Meilleure solution rencontrée jusqu’alors. 

			while (!Done()) // indique si oui ou non les critères d’arrêt sont atteints.
            {
				UpdateGeneralVariables(); // Mise à jour des meilleures solutions globales.
				UpdateSolutions(); // Mise à jour des positions des solutions. 
                Increment(); // Incrementation des variables pour les critères d'arrets.
            }
            SendResult();
        }

        protected abstract void UpdateSolutions();

        protected abstract void UpdateGeneralVariables();

        protected abstract bool Done();

        protected abstract void Increment();
    }
}
