using System;

namespace GeneticAlgorithm
{
    internal class IndividualFactory
    {
		private static IndividualFactory instance;
		/* Classe fille instancier en fonction du besoin : On utilise donc une fabrique d’individus, 
		 * qui est un singleton. De cette façon, elle est accessible depuis tout le code, 
		 * mais une seule instance est créée à chaque lancement du code.*/

        private IndividualFactory() { }

        public static IndividualFactory getInstance() // Retourne la classe fille instancier.
        {
            if (instance == null)
            {
                instance = new IndividualFactory();
            }
            return instance;
        }

        public Individual getIndividual(String type) {
			// Creation dun individu aleatoirement.
			// Paramètre : le problème à résoudre sous la forme d’une chaîne & puis les parents potentiels.
            Individual ind = null;
            switch (type)
            {
                case "Maze" :
                    ind = new MazeIndividual();
                    break;
                case "TSP":
                    ind = new TSPIndividual();
                    break;
            }
            return ind;
        }

        public Individual getIndividual(String type, Individual father) 
        {
			// Creation d'un individu à partir d'un parent.
            Individual ind = null;
            switch (type)
            {
                case "Maze" :
                    ind = new MazeIndividual((MazeIndividual) father);
                    break;
                case "TSP":
                    ind = new TSPIndividual((TSPIndividual)father);
                    break;
            }
            return ind;
        }

        public Individual getIndividual(String type, Individual father, Individual mother)
        {
			// Creation d'un individu à partir de deux parents.
            Individual ind = null;
            switch (type)
            {
                case "Maze" :
                    ind = new MazeIndividual((MazeIndividual)father, (MazeIndividual) mother);
                    break;
                case "TSP":
                    ind = new TSPIndividual((TSPIndividual)father, (TSPIndividual)mother);
                    break;
            }
            return ind;
        }

		internal void Init(string type) // Initialiser l’environnement de vie des individus
        {
            switch (type)
            {
                case "Maze":
                    Maze.Init(Maze.Maze2);
                    break;
                case "TSP":
                    TSP.Init();
                    break;
            }
        }
    }
}
