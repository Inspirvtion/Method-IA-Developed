
namespace MetaheuristicsPCL
{
    public class GradientDescentForKnapsack : GradientDescentAlgorithm
    {
        int nbIterationsWithoutUpdate = 0;
        private const int MAX_ITERATIONS_WITHOUT_UPDATE = 50;

        protected override bool Done()
        {
            return nbIterationsWithoutUpdate >= MAX_ITERATIONS_WITHOUT_UPDATE;
			// retourne true si apres N tentative l'on ne renconter de meilleur solution.
        }

        protected override void Increment()
        {
            nbIterationsWithoutUpdate++;
			// le nombre d’itérations sans amélioration à incrémenter.
        }

        protected override void UpdateSolution(ISolution _bestSolution)
		{/*on regarde simplement si la valeur est plus forte que celle en cours. 
		  *Si oui, on remplace et on remet à 0 le compteur indiquant le nombre d’itérations sans amélioration.*/
            if (_bestSolution.Value > currentSolution.Value)
            {
                currentSolution = _bestSolution;
                nbIterationsWithoutUpdate = 0;
            }
        }

		protected override void SendResult() // Afficher le resultat construit dans l'algorithme.
        {
            ihm.PrintMessage(currentSolution.ToString());
        }
    }
}
