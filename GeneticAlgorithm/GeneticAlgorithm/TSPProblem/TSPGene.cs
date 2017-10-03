using System;

namespace GeneticAlgorithm
{
	// Les individus sont composés d’une suite de gènes, chacun d’eux représentant une ville à visiter.  

	internal class TSPGene : IGene  // herite de l'interface Gene : mutate().
    {
        City city; // Structure Ville.
        
		public TSPGene(City _city) // soit directement sous la forme d’un objet City (au moment de l’initialisation).
        {
            city = _city;
        }

		public TSPGene(TSPGene g) //  soit grâce à un autre gène qu’il faudra copier (pour la reproduction).
        {
            city = g.city;
        }

        internal int getDistance(TSPGene g)
        {
            return TSP.getDistance(city, g.city);
			// int getDistance(City _city1, City _city2)
        }

        public override string ToString()
        {
            return city.ToString();
        }

        public void Mutate()
        {
            throw new NotImplementedException();
			/*On doit forcément passer une et une seule fois par chaque ville. 
			 *Cette méthode lève donc une exception si elle est appelée.*/
        }
    }
}
