using System.Collections.Generic;
using System.Linq;

namespace MetaheuristicsPCL
{
    public class GreedyAlgorithmForKnapsack : GreedyAlgorithm
    {
        KnapsackSolution solution; // Reference de la solution.

        protected override void ConstructSolution() // Constrution de la solution.
        {
            KnapsackProblem problem = (KnapsackProblem) pb; // Probleme pb passer en parametre.
            List<Box> boxes = problem.Boxes(); // Liste de boxes du probleme pb.

			solution = new KnapsackSolution(); // Creation de la solution (loadedContent = new List<Box> ()).

            foreach(Box currentBox in boxes.OrderByDescending(x => x.Value / x.Weight))
            {
				// L'on classe les boxes par ordre decroissant (Valeur/Kg).

                double spaceLeft = problem.MaxWeight - solution.Weight;
				// On evalue a chaque tour de boucle la valeur de l'espace disponible. 
                if (currentBox.Weight < spaceLeft)
                {
					// S'il y a encore de l'espace on ajout le box. 
                    solution.LoadedContent.Add(currentBox);
                }
            }
        }

        protected override void SendResult() // Afficher le resultat construit dans l'algorithme.
        {
            ihm.PrintMessage(solution.ToString());
        }
    }
}
