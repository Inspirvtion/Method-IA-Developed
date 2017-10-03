		using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
	// Construction de l'individu
	// Mutation de l'individu
	// Evaluation de l'individu

    internal class MazeIndividual : Individual
    {
        // Constructeur par défaut : initialisation aléatoire
        public MazeIndividual()
        {
            genome = new List<IGene>(); // Creation du genome.
            for (int i = 0; i < Parameters.initialGenesNb; i++)
            {
                genome.Add(new MazeGene()); 
				// remplissage gu genome par des genes tirés aleatoirement.
            }
        }

        // Constructeur avec un parent (copie + mutations)
        public MazeIndividual(MazeIndividual father)
        {
            this.genome = new List<IGene>(); // Creation du genome du fils du pere passé en paramètre.
            foreach (MazeGene g in father.genome)
            {
                this.genome.Add(new MazeGene(g)); // Copie gene a gene du genome du pere au fils.
            }
			Mutate(); // Suppression | Ajout | Remplacement gène par gène.
        }

        // Constructeur avec deux parents (crossover et mutations)
        public MazeIndividual(MazeIndividual father, MazeIndividual mother) 
        {
			// Creation du genome du fils du pere et de la mere passés en paramètre.
            this.genome = new List<IGene>();
            // Crossover
            int cuttingPoint = Parameters.randomGenerator.Next(father.genome.Count); // Position du point coupure.
            foreach (MazeGene g in father.genome.Take(cuttingPoint))
            {
				this.genome.Add(new MazeGene(g)); 
				// Copie gene a gene du genome du pere au fils jusqu'au pt de coupure.
            }
            foreach (MazeGene g in mother.genome.Skip(cuttingPoint))
            {
                this.genome.Add(new MazeGene(g)); 
				// Remplissage du genome fils avec les genes du genome de la mere en commencant au pt de coupure.
            }
            // Mutation
			Mutate(); // Suppression | Ajout | Remplacement gène par gène.
        }

        internal override void Mutate()
        {
            // Suppression ?
            if (Parameters.randomGenerator.NextDouble() < Parameters.mutationDeleteRate)
            {
                int geneIndex = Parameters.randomGenerator.Next(genome.Count);
				// Tirage aleatoire de la position du gene a supprimer.
                genome.RemoveAt(geneIndex); // Suppression du gene.
            }

            // Ajout ?
            if (Parameters.randomGenerator.NextDouble() < Parameters.mutationAddRate)
            {
                genome.Add(new MazeGene()); // Ajout du nouveau gene aleatoire au genome.
            }

            // Remplacement gène par gène ?
            foreach (MazeGene g in genome)
            {
                if (Parameters.randomGenerator.NextDouble() < Parameters.mutationsRate)
                {
                    g.Mutate(); // Si condition alors Remplacement de chaque gene par un gene aleaoire
                }
            }
        }

        internal override double Evaluate()
        {
            fitness = Maze.Evaluate(this);
            return fitness;
			// l'individu ayant la plus faible fitness est le meilleur individu.
        }
    }
}
