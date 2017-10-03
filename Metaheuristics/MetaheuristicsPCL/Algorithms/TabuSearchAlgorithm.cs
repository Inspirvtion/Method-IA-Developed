using System.Collections.Generic;

namespace MetaheuristicsPCL
{
    public abstract class TabuSearchAlgorithm : Algorithm
    {
		protected ISolution currentSolution; // La solution courante.

        protected ISolution bestSoFarSolution; // Meilleur solution trouvée jusque là.

        public override sealed void Solve(IProblem _pb, IHM _ihm)
        {
            base.Solve(_pb, _ihm);

            currentSolution = pb.RandomSolution(); // Solution aléatoire du Probleme : Solution initiale.
            bestSoFarSolution = currentSolution;
            AddToTabuList(currentSolution); // Etant donné quelle est visitée elle est tabou.

            while (!Done())
				// indique si oui ou non les critères d’arrêt sont atteints.
            {
                List<ISolution> Neighbours = pb.Neighbourhood(currentSolution);
				// Liste des solutions voisines.
                if (Neighbours != null)
					// Existe il des voisins?
                {
                    Neighbours = RemoveSolutionsInTabuList(Neighbours);
					// les solutions présentes dans la liste des positions taboues sont enlevées pour ne pas parcourir les mmes solutions.
                    ISolution bestSolution = pb.BestSolution(Neighbours);
					// Meilleur solution parmi les solutions voisines de la solution courante. 
                    if (bestSolution != null)
                    { // Car la meilleur solution pouvait etre considéré comme tabou.
                        UpdateSolution(bestSolution);
						// C’est dans cette méthode que bestSoFarSolution est utilisée.
                    }
                }
                Increment();

            }
            SendResult();
        }

        protected abstract void AddToTabuList(ISolution currentSolution);

        protected abstract List<ISolution> RemoveSolutionsInTabuList(List<ISolution> Neighbours);

        protected abstract bool Done();

        protected abstract void UpdateSolution(ISolution _bestSolution);

        protected abstract void Increment();
    }
}
