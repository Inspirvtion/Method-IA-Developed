using System.Collections.Generic;
using System.Linq;

namespace MetaheuristicsPCL
{
    public class ParticleSwarmOptimizationForKnapsack : ParticleSwarmOptimizationAlgorithm
    {
		int nbIterations = 1; // Le critère d’arrêt est seulement le nombre d’itérations.
        private const int MAX_ITERATIONS = 200;

		protected override void UpdateSolutions() // Consiste à mettre à jour les différentes solutions.
        {
            List<Box> possibleBoxes = ((KnapsackProblem)pb).Boxes();

            foreach (ISolution genericSolution in currentSolutions)
			{ // On parcoure la liste de toute la population de solutions. 
				
                KnapsackSolution solution = (KnapsackSolution)genericSolution;

                if (!solution.Equals(bestSoFarSolution))
				{ /*Pour chacune, s’il ne s’agit pas de la meilleure trouvée jusqu’à présent, on va lui ajouter
				   *un élément de cette dernière, et un élément de la meilleure de la population actuelle.
				   *Les éléments tirés au sort ne sont ajoutés que s’ils ne sont pas déjà présents dans le sac à dos.*/

					//On recupere aleatoirement un element contenu dans bestActualSolution. 
                    int index = KnapsackProblem.randomGenerator.Next(0, ((KnapsackSolution)bestActualSolution).LoadedContent.Count);
                    Box element = ((KnapsackSolution)bestActualSolution).LoadedContent.ElementAt(index);

                    if (!solution.LoadedContent.Contains(element))
                    { // On s'assure que la nouvelle solution ne contient pas deja Box element.  
                        solution.LoadedContent.Add(element);
                    }
					//On recupere aleatoirement un element contenu dans bestSoFarSolution.
                    index = KnapsackProblem.randomGenerator.Next(0, ((KnapsackSolution)bestSoFarSolution).LoadedContent.Count);
                    element = ((KnapsackSolution)bestSoFarSolution).LoadedContent.ElementAt(index);
                    if (!solution.LoadedContent.Contains(element))
                    {
                        solution.LoadedContent.Add(element);
                    }

                    while (solution.Weight > ((KnapsackProblem)pb).MaxWeight)
                    {
						/*Après cet ajout, le sac peut avoir un poids trop important. On élimine alors 
						 *aléatoirement des boîtes jusqu’à repasser sous la limite du poids.*/
                        index = KnapsackProblem.randomGenerator.Next(0, solution.LoadedContent.Count);
                        solution.LoadedContent.RemoveAt(index);
                    }

                    double enableSpace = ((KnapsackProblem)pb).MaxWeight - solution.Weight;
					// On calcule l'espace disponible dans le sac.
                    List<Box> availableBoxes = possibleBoxes.Except(solution.LoadedContent).Where(x => (x.Weight <= enableSpace)).ToList();
					/* Trie : Toutes les boxes contenus dans possibleBoxes a l'exception des boxes déja contenu 
					 * dans la soution et dont le poids est inferieur ou egale a l'espace disponible.*/

                    while (enableSpace > 0 && availableBoxes.Count != 0)
                    {
						// Ajout d'un element de availableBoxes tiré au hazard dans la solution. 
                        index = KnapsackProblem.randomGenerator.Next(0, availableBoxes.Count);
                        solution.LoadedContent.Add(availableBoxes.ElementAt(index));

                        enableSpace = ((KnapsackProblem)pb).MaxWeight - solution.Weight;
                        availableBoxes = possibleBoxes.Except(solution.LoadedContent).Where(x => (x.Weight <= enableSpace)).ToList();
                    }
                }
            }
        }

        protected override void UpdateGeneralVariables()
        {
			// La meilleur solution de popilation est celle ayant la plus grande valeur.
            bestActualSolution = currentSolutions.OrderByDescending(x => x.Value).FirstOrDefault();
			// Si cette dernière est meilleur que la meilleur touvé jusque là on la met à jour.
            if (bestActualSolution.Value > bestSoFarSolution.Value)
            {
                bestSoFarSolution = bestActualSolution;
            }
        }

        protected override bool Done()
		{ /*La troisième méthode est celle permettant de vérifier que le critère d’arrêt est ou non atteint. 
		   *Il suffit donc de vérifier le nombre d’itérations.*/
            return nbIterations >= MAX_ITERATIONS;
        }

        protected override void Increment()
		{ // incrémente simplement le nombre d’itérations.
            nbIterations++;
        }

        protected override void SendResult()
		{ // afficher la meilleure solution rencontrée jusqu’à présent.
            ihm.PrintMessage(bestSoFarSolution.ToString());
        }
    }
}
