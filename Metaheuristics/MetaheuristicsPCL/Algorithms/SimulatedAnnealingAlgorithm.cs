using System.Collections.Generic;

namespace MetaheuristicsPCL
{
    public abstract class SimulatedAnnealingAlgorithm : Algorithm
    {
		protected ISolution currentSolution; // La solution courante.
		protected ISolution bestSoFarSolution; // Meilleur solution trouvée jusque là.
        protected double temperature; 
		// La temperature. permettant de calculer la probabilité de validation d'une soution. P = e(-AE/T).

        public override sealed void Solve(IProblem _pb, IHM _ihm)
        {
            base.Solve(_pb, _ihm);

			currentSolution = pb.RandomSolution(); // Solution aléatoire du Probleme : Solution initiale.
            bestSoFarSolution = currentSolution;

            InitTemperature(); // Initialisation de la temperature.
			while (!Done()) // indique si oui ou non les critères d’arrêt sont atteints.
            {
                List<ISolution> Neighbours = pb.Neighbourhood(currentSolution);
				// Liste des solutions voisines.
				if (Neighbours != null) // // Existe il des voisins?
                {
                    ISolution bestSolution = pb.BestSolution(Neighbours);
					// Meilleur solution parmi les solutions voisines de la solution courante.
                    UpdateSolution(bestSolution);
					// C’est dans cette méthode que la température et bestSoFarSolution sont utilisées.
                }
                Increment();
                UpdateTemperature();
            }
            SendResult();
        }

        protected abstract void UpdateTemperature();

        protected abstract void InitTemperature();

        protected abstract bool Done();

        protected abstract void UpdateSolution(ISolution _bestSolution);

        protected abstract void Increment();
    }
}
