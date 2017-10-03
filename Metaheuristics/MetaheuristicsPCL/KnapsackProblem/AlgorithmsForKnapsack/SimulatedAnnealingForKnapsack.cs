using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaheuristicsPCL
{
    public class SimulatedAnnealingForKnapsack : SimulatedAnnealingAlgorithm
    {
        int nbIterationsWithoutUpdate = 1;
        int nbIterations = 1;
        private const int MAX_ITERATIONS_WITHOUT_UPDATE = 30;
        private const int MAX_ITERATIONS = 100;
		
		protected override void UpdateTemperature() // Mise à jour de la temperature.
        {
            temperature *= 0.9;
        }

		protected override void InitTemperature() // Initialisation de la temperature.
        {
            temperature = 5;
        }

        protected override bool Done()
        {
			//  On vérifie si on a dépassé nos deux nombres maximums d’itérations.
            return nbIterationsWithoutUpdate >= MAX_ITERATIONS_WITHOUT_UPDATE && nbIterations >= MAX_ITERATIONS;
        }

        protected override void UpdateSolution(ISolution _bestSolution)
        {
            double seuil = 0.0;
			if (_bestSolution.Value < currentSolution.Value) // solution entraînant une perte de qualité.
            {
				// Calcule la probabilité de l’accepter grâce à la loi de Metropolis.
                seuil = Math.Exp(-1 * (currentSolution.Value - _bestSolution.Value) / currentSolution.Value / temperature);
            }
            if (_bestSolution.Value > currentSolution.Value || KnapsackProblem.randomGenerator.NextDouble() < seuil)
            {
				// nombre aléatoire inférieur à cette probabilité ou si la solution proposée est meilleure.
				currentSolution = _bestSolution; // on met à jour la position (solution).
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
    }
}
