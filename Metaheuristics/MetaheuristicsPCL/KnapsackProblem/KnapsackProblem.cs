using System;
using System.Collections.Generic;
using System.Linq;

namespace MetaheuristicsPCL
{
    public class KnapsackProblem : IProblem
    {
		protected List<Box> boxes = null; // Liste de boîtes disponibles à mettre dans les sacs.
        public double MaxWeight { get; set; } // Poids maximal.

		public static Random randomGenerator = null; // Générateur aléatoire rendu statique.

		public const int NB_NEIGHBOURS = 30; 
		// Une constante indiquant le nombre de voisins que l’on créera pour chaque solution.

        public KnapsackProblem() 
        {
			/*Le constructeur par défaut construit l’exemple présenté au début de ce chapitre, 
			 *avec un sac à dos d’une contenance de 20 kg, et 12 boîtes utilisables.*/
            boxes = new List<Box>(); // Creation des differents box.

            boxes.Add(new Box("A", 4, 15));
            boxes.Add(new Box("B", 7, 15));
            boxes.Add(new Box("C", 10, 20));
            boxes.Add(new Box("D", 3, 10));
            boxes.Add(new Box("E", 6, 11));
            boxes.Add(new Box("F", 12, 16));
            boxes.Add(new Box("G", 11, 12));
            boxes.Add(new Box("H", 16, 22));
            boxes.Add(new Box("I", 5, 12));
            boxes.Add(new Box("J", 14, 21));
            boxes.Add(new Box("K", 4, 10));
            boxes.Add(new Box("L", 3, 7));

            MaxWeight = 20;
            if (randomGenerator == null)
            {
                randomGenerator = new Random();
            }
        }

        public KnapsackProblem(int _nbBoxes, double _maxWeight, double _maxValue)
        {
			/*Un deuxième constructeur construit un nouveau problème. On lui donne alors le nombre de 
			 *boîtes disponibles, la taille du sac et la valeur maximale de chaque boîte. 
			 *Celles-ci sont ensuite créées aléatoirement.*/
            boxes = new List<Box>();
            MaxWeight = _maxWeight;
            if (randomGenerator == null)
            {
                randomGenerator = new Random();
            }

            for (int i = 0; i < _nbBoxes; i++)
            {
                boxes.Add(new Box(i.ToString(), randomGenerator.NextDouble() * _maxWeight, randomGenerator.NextDouble() * _maxValue));
            }
        }

		public List<Box> Boxes() { // Renvoit la liste des boîtes disponibles.
            return boxes;
        }

        public List<ISolution> Neighbourhood(ISolution _currentSolution)
        {
            List<ISolution> neighbours = new List<ISolution>();
            List<Box> possibleBoxes = boxes;

            for (int i = 0; i < NB_NEIGHBOURS; i++) // Creation du voisinage.
            {
                KnapsackSolution newSol = new KnapsackSolution((KnapsackSolution)_currentSolution);
				// On recupere la solution courante : Base d'une solution du voisinage.
                int index = randomGenerator.Next(0, newSol.LoadedContent.Count);
				// On recupere l'index : position d'une box de la solution en cours de création. 
                newSol.LoadedContent.RemoveAt(index);
				// On retire la boxe de la solution.

                double enableSpace = MaxWeight - newSol.Weight; // Poids disponible dans la solution.
                List<Box> availableBoxes = possibleBoxes.Except(newSol.LoadedContent).Where(x => (x.Weight <= enableSpace)).ToList();
				/* Trie : tous les boxes de possibleBoxes dont le poids est inferieur ou egale à enableSpace 
				 * et pas contenu dans newSol.LoadedContent.*/

                while (enableSpace > 0 && availableBoxes.Count != 0) 
                {// A chaque itération.
                    index = randomGenerator.Next(0, availableBoxes.Count);
					// On recupere l'index : position aleatoire d'une boxe de availableBoxes.
                    newSol.LoadedContent.Add(availableBoxes.ElementAt(index));
					// Ajout de la boite placé a cette position dans newSol.
                    enableSpace = MaxWeight - newSol.Weight;
					// Réevaluation de l'espace disponible. 
                    availableBoxes = possibleBoxes.Except(newSol.LoadedContent).Where(x => (x.Weight <= enableSpace)).ToList();
					/* Trie : tous les boxes de possibleBoxes dont le poids est inferieur ou egale à enableSpace 
				     * et pas contenu dans newSol.LoadedContent.*/
                }

                neighbours.Add(newSol); // Ajout du voisin crée dans la liste des voisins.
            }
            return neighbours;
        }

        public ISolution RandomSolution()
        {
            KnapsackSolution solution = new KnapsackSolution(); // creation d'une liste de box : loadedContent.
            List<Box> possibleBoxes = boxes; // Liste de boxes disponibles.

            double enableSpace = MaxWeight; // Espace disponibles.
            List<Box> availableBoxes = possibleBoxes.Where(x => (x.Weight <= enableSpace)).ToList();
			// on tire au sort des boîtes une à une parmi les boîtes disponibles.
			// Trie : tous les boxes de possibleBoxes dont le poids est inferieur ou egale à enableSpace.

            while (enableSpace > 0 && availableBoxes.Count != 0)
			{// Pour s’assurer qu’il s’agit d’une solution viable, on vérifie que l’espace disponible est suffisant.
                int index = randomGenerator.Next(0, availableBoxes.Count);
                solution.LoadedContent.Add(availableBoxes.ElementAt(index));
                enableSpace = MaxWeight - solution.Weight;
				// enableSpace diminue au fur et a mesure que l'on ajoute des box dans la solution.
                availableBoxes = possibleBoxes.Except(solution.LoadedContent).Where(x => (x.Weight <= enableSpace)).ToList();
				// On tire au sort des boîtes une à une parmi les boîtes disponibles et non encore utilisées.
				/* Trie : tous les boxes de possibleBoxes dont le poids est inferieur ou egale à enableSpace 
				 * et pas contenu dans solution.LoadedContent.*/
            }

            return solution;
        }

        public ISolution BestSolution(List<ISolution> _listSolutions)
        {
            return _listSolutions.Where(x => x.Value.Equals(_listSolutions.Max(y => y.Value))).FirstOrDefault();
			// chercher la solution avec la valeur maximale : 1ere solution dont la valeur est la plus grande.
        }
    }
}
