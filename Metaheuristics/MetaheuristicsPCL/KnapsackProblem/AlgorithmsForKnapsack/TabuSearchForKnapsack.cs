using System.Collections.Generic;
using System.Linq;

namespace MetaheuristicsPCL
{
    public class TabuSearchForKnapsack : TabuSearchAlgorithm
    {
        int nbIterationsWithoutUpdate = 1;
        int nbIterations = 1;
		private const int MAX_ITERATIONS_WITHOUT_UPDATE = 30; 
		// nombre d’itérations pendant lesquelles on ne trouve plus d’améliorations
        private const int MAX_ITERATIONS = 100; 
		// nombre d’itérations
        private const int TABU_SEARCH_MAX_SIZE = 50;
		// La taille de la liste des éléments tabous.

        List<KnapsackSolution> tabuSolutions = new List<KnapsackSolution>(); // Crèation de la liste tabous.

        protected override bool Done()
        {
			//  On vérifie si on a dépassé nos deux nombres maximums d’itérations.
            return nbIterationsWithoutUpdate >= MAX_ITERATIONS_WITHOUT_UPDATE && nbIterations >= MAX_ITERATIONS;
        }

        protected override void UpdateSolution(ISolution _bestSolution)
        {
			// On se deplace de meilleur solution à meilleur solution en enregistrant la meilleur solution trouvé jusque là.

			// on vérifie simplement si la meilleure solution proposée est contenue dans la liste des positions taboues
            if (!tabuSolutions.Contains((KnapsackSolution)_bestSolution))               
            {
				currentSolution = _bestSolution; // on met à jour la position (solution).
				AddToTabuList((KnapsackSolution)_bestSolution); // On l'ajoute au positions tabous (solutions).
                if (_bestSolution.Value > bestSoFarSolution.Value)
                {
					/*si on a atteint une solution meilleure que celle obtenue jusqu’alors, on met à jour la
					 *variable bestSoFarSolution. Enfin, on remet à zéro le nombre d’itérations sans mises à jour. */
                    bestSoFarSolution = _bestSolution;
                    nbIterationsWithoutUpdate = 0;
                }
            }
        }

        protected override void Increment()
        {
            nbIterationsWithoutUpdate++;
            nbIterations++;
        }

		protected override void SendResult() // Afficher le resultat construit dans l'algorithme.
        {
            ihm.PrintMessage(bestSoFarSolution.ToString());
        }

        protected override void AddToTabuList(ISolution _solution)
        {
			// on vérifie d’abord si on a atteint le nombre maximal de positions (solutions).
            while (tabuSolutions.Count >= TABU_SEARCH_MAX_SIZE)
            {
				// on enlève la toute première de la liste
                tabuSolutions.RemoveAt(0);
            }
			// ajout de la nouvelle position.
            tabuSolutions.Add((KnapsackSolution)_solution);
        }

        protected override List<ISolution> RemoveSolutionsInTabuList(List<ISolution> Neighbours)
        {
            return Neighbours.Except(tabuSolutions).ToList();
			// Trie : Toutes les solutions de Neighbours sauf ceux considérées tabous.  
        }
    }
}
