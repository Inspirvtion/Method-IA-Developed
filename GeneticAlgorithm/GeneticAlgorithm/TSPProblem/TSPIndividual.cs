using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    internal class TSPIndividual : Individual
    {
        // Constructeur par défaut : initialisation aléatoire
        public TSPIndividual()
        {
            genome = new List<IGene>(); // Creation du Genome.
            List<City> cities = TSP.getCities(); // Creation d'une liste de Villes existantes.
            while (cities.Count != 0)
            {
                int index = Parameters.randomGenerator.Next(cities.Count); // On choisie l'index d'une ville au hazard.
                genome.Add(new TSPGene(cities.ElementAt(index))); // on ajoute notre ville au genome.
                cities.RemoveAt(index); // Puis on retire la ville de la list après ajout.
            }
        }

        // Constructeur avec un parent (copie + mutations)
        public TSPIndividual(TSPIndividual father)
        {
			/*reconstruit un génome en faisant une copie des gènes un à un, 
			 *puis on appelle notre opérateur de mutation.*/
            this.genome = new List<IGene>();
            foreach (TSPGene g in father.genome)
            {
                this.genome.Add(new TSPGene(g));
            }
            Mutate();
        }

        // Constructeur avec deux parents (crossover et mutations)
        public TSPIndividual(TSPIndividual father, TSPIndividual mother)
        {
			/*On choisit un point de coupure aléatoirement, et on copie les villes avant ce point depuis 
			 *le premier parent. On parcourt ensuite le deuxième parent pour ne récupérer que les villes 
			 *non encore visitées, en conservant leur ordre. Enfin, on appelle l’opérateur de mutation.*/

            this.genome = new List<IGene>();
            // Crossover
            int cuttingPoint = Parameters.randomGenerator.Next(father.genome.Count); // Point de coupure du pere. 
            foreach (TSPGene g in father.genome.Take(cuttingPoint)) // Jusqu'au point de coupure.
            {
                this.genome.Add(new TSPGene(g));
            }
            foreach (TSPGene g in mother.genome)
            {
                if (!genome.Contains(g)) // Si le genome ne contient pas deja les villes.
                {
                    this.genome.Add(new TSPGene(g));
                }
            }

            // Mutation
            Mutate();
        }

        internal override void Mutate()
        {
			/*L’opérateur de mutation consiste à changer la place d’un gène : on enlève un gène aléatoirement
			 *et on le replace à un index tiré au sort. Cette mutation ne se fait que si on tire un nombre
			 *inférieur au taux de mutation.*/
            if (Parameters.randomGenerator.NextDouble() < Parameters.mutationsRate)
            {
                int index1 = Parameters.randomGenerator.Next(genome.Count); // Tirage d'une position au hazard.
                TSPGene g = (TSPGene)genome.ElementAt(index1); // On enregistre le gene tiré.
                genome.RemoveAt(index1); // On retire ce géne du genome.
                int index2 = Parameters.randomGenerator.Next(genome.Count); // On tire un gene au hazard.
                genome.Insert(index2, g); // On insere notre 1er gene tiré a la position 2
            }
        }

        internal override double Evaluate()
        {
            int totalKm = 0;
            TSPGene oldGene = null;
            foreach (TSPGene g in genome)
            {
                if (oldGene != null)
                {
                    totalKm += g.getDistance(oldGene);
                }
                oldGene = g;
            }
            totalKm += oldGene.getDistance((TSPGene)genome.FirstOrDefault());
            fitness = totalKm;
            return fitness;
			/*parcourir la liste des villes, et demander la distance entre les villes deux à deux.
			 *Enfin, on n’oublie pas de rajouter la distance de la dernière à la première ville pour
			 *boucler notre parcours.*/
        }
    }
}
