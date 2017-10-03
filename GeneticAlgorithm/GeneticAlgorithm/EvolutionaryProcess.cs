using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    public class EvolutionaryProcess
    {
		// Controle le processus evolutionnaire.
		protected List<Individual> population; // La population active, qui est une liste d’individus.
		protected int generationNb = 0; // Le numéro de la génération active.
		protected IIHM program = null; // Une référence vers la classe qui sert d’IHM.
		protected double bestFitness; // La meilleure fitness rencontrée jusqu’ici.
		protected string problem; // Le nom du problème à résoudre.

        

		public EvolutionaryProcess(IIHM _program, string _problem) // Constructeur.
        {
			// P :  la chaîne représentant le problème à résoudre et la référence vers l’IHM.
            program = _program; // IHM
            problem = _problem; // Probleme à resoudre.
            IndividualFactory.getInstance().Init(problem);
			// internal void Init(string type) : Initialiser l’environnement de vie des individus.
            population = new List<Individual>(); // Liste d'individus.
            for (int i = 0; i < Parameters.individualsNb; i++)
            {
                population.Add(IndividualFactory.getInstance().getIndividual(problem));
				// public Individual getIndividual(String type) : Creation dun individu aleatoirement.
            }
        }

        

		public void Run()
        {
			/*À chaque itération, il faut :
			 *Lancer l’évaluation de chaque individu.
			 *Récupérer le meilleur individu et lancer son affichage.
			 *Créer une nouvelle population (en utilisant l’élitisme pour conserver la meilleure solution jusqu’alors puis la sélection du ou des parents si le crossover s’applique).
			 *Appliquer la survie des descendants (qui deviennent la population en cours).*/


            bestFitness = Parameters.minFitness + 1;
            while (generationNb < Parameters.generationsMaxNb && bestFitness > Parameters.minFitness)
            {
                // Evaluation
                foreach (Individual ind in population)
                {
					ind.Evaluate(); 
					// Logique floue (Test) [Extraction des delais et la distance, Application de la logique floue].
                }

                
                Individual bestInd = population.OrderBy(x => x.Fitness).FirstOrDefault();
				// Meilleur individu (stats)
                program.PrintBestIndividual(bestInd, generationNb);
				// Affichage du meilleur individu à chaque generation.
				bestFitness = bestInd.Fitness; 
				// (Graphe (Abcisse : generationNb , Ordonnée : bestInd)).

                // Sélection et reproduction
                List<Individual> newGeneration = new List<Individual>(); // Création de la nouvelle génération.
                // Elitisme :
                newGeneration.Add(bestInd); // Ajout du meilleur individu.
                for (int i = 0; i < Parameters.individualsNb - 1; i++)
                {
                    // Un ou deux parents ?
                    if (Parameters.randomGenerator.NextDouble() < Parameters.crossoverRate)
                    {
                        // Choisir parents
                        Individual father = Selection();
                        Individual mother = Selection();

                        // Reproduction
                        newGeneration.Add(IndividualFactory.getInstance().getIndividual(problem, father, mother));
                    }
                    else
                    {
                        // Choisir parent
                        Individual father = Selection();

                        // Reproduction
                        newGeneration.Add(IndividualFactory.getInstance().getIndividual(problem, father));
                    }
                }

                // Survie
                Survival(newGeneration);

                generationNb++;
            }
        }

        

		private void Survival(List<Individual> newGeneration)
        {
			// A chaque génération, tous les enfants deviennent les adultes, qui, eux, disparaissent.
            // Remplacement
            population = newGeneration;
        }

        

		private Individual Selection()
        {
            // Roulette biaisée sur le rang Pour le CROSSOVER.
            
			int totalRanks = Parameters.individualsNb * (Parameters.individualsNb + 1) / 2;
			// Nombre total de parts élémentaires sur la roue.
            int rand = Parameters.randomGenerator.Next(totalRanks); // Exemple : Part 9 sur totalRanks.
			// On choisit donc une part aléatoirement dans celles présentes. 


            int indIndex = 0; // Index sur l'individu tiré.
			int nbParts = Parameters.individualsNb; 
			// N :  Part tirée au sort appartient au premier individu qui possède N parts.
			// l'index 0 a N premeiers parts.
            int totalParts = 0; // Initialisation du nombre total de parts.

            while(totalParts < rand) {
                indIndex++;
				totalParts += nbParts; // N = N + (N-1) + (N-2). (3 tours de boucle).
				nbParts --; // N --> N-1 --> N-2 (3 tours de boucle).
            }
			// je sors de la boucle avec index 3 car Part 9 est contenu dans index 3.

            return population.OrderBy(x => x.Fitness).ElementAt(indIndex); 
			// Individu selectionné = individu ayant la 3eme plus grande fitness parmi les individus. 
			// On retourne enfin l’individu à l’index voulu (en n’oubliant pas de les trier par fitness croissante d’abord).
        }
    }
}
