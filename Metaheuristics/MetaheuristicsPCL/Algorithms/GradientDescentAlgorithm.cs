using System.Collections.Generic;

namespace MetaheuristicsPCL
{
    public abstract class GradientDescentAlgorithm : Algorithm
    {
        protected ISolution currentSolution;
		
        public override sealed void Solve(IProblem _pb, IHM _ihm)
        {
            base.Solve(_pb, _ihm);

            currentSolution = pb.RandomSolution();
			// La solution aléatoire du probleme.
            while (!Done())
				// indique si oui ou non les critères d’arrêt sont atteints.	
            {
                List<ISolution> Neighbours = pb.Neighbourhood(currentSolution);
				// Liste des solutions voisines.
                if (Neighbours != null)
					// Existe il des voisins.
                {
                    ISolution bestSolution = pb.BestSolution(Neighbours);
					// Meilleur solution parmi les solutions voisines de la solution courante. 
                    UpdateSolution(bestSolution);
					// Change ou non la meilleur solution, selon la présence ou l’absence d’une amélioration.
                }
                Increment();
            }
            SendResult();
        }

        protected abstract bool Done();

        protected abstract void UpdateSolution(ISolution _bestSolution);

        protected abstract void Increment();
    }
}
