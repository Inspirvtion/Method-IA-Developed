using System.Collections.Generic;

namespace MetaheuristicsPCL
{
    public interface IProblem
    {
        List<ISolution> Neighbourhood(ISolution _currentSolution); 
		// La liste des solutions voisines a la solution en cours.

        ISolution RandomSolution(); // La solution de depart tirée aléatoirement.

        ISolution BestSolution(List<ISolution> _neighbours); // La meilleur solution parmi la liste des voisins
    }
}
