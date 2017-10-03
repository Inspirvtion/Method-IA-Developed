using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiAgentSystemPCL
{
	/*L’environnement du banc de poissons est un océan virtuel. Celui-ci est mis à jour à la demande, 
	 *de manière asynchrone. Il est donc nécessaire que l’océan puisse lever un évènement lorsque la 
	 *mise à jour est complète (pour lancer la mise à jour de l’interface graphique).
	 *Nous utilisons un delegate pour cela. Le fichier Ocean.cs commence donc par déclarer un delegate 
	 *qui prend en paramètres les poissons et les obstacles.*/
    public delegate void OceanUpdated(FishAgent[] _fish, List<BadZone> _obstacles);

    public class Ocean
    {
        public event OceanUpdated oceanUpdatedEvent;

		FishAgent[] fishList = null; // Tableau de poissons.
        List<BadZone> obstacles = null; // Liste d'obstacles.

        Random randomGenerator;

        protected double MAX_WIDTH;
        protected double MAX_HEIGHT;

        public Ocean(int _fishNb, double _width, double _height)
        {
            MAX_WIDTH = _width; // Largeur de l'ocean.
            MAX_HEIGHT = _height; // Longueur de l'ocean.
            randomGenerator = new Random();
			 
			// Creation d'une liste obstacles vide.
			obstacles = new List<BadZone>();
			// Creation de poissons.
            fishList = new FishAgent[_fishNb];
            for (int i = 0; i < _fishNb; i++)
            {
				// PosX = _x; PosY = _y; speedX = Math.Cos(_dir); speedY = Math.Sin(_dir); (chacun est positionné et dirigé aléatoirement).  
                fishList[i] = new FishAgent(randomGenerator.NextDouble() * MAX_WIDTH, randomGenerator.NextDouble() * MAX_HEIGHT, randomGenerator.NextDouble() * 2 * Math.PI);
            }
        }

		// Mise à jour de l'environnement.
        public void UpdateEnvironnement()
        {
            UpdateObstacles();
            UpdateFish();
            if (oceanUpdatedEvent != null)
            {
                oceanUpdatedEvent(fishList, obstacles);
            }
        }

		// Mise à jour des obstacles.
        private void UpdateObstacles()
        {
            foreach (BadZone obstacle in obstacles)
            {
				obstacle.Update(); // Baisser son temps restant à vivre 
            }
			obstacles.RemoveAll(x => x.Dead()); // Supprimer les zones qui ont atteint leur fin de vie.
        }

		// Mise à jour des poissons.
        private void UpdateFish()
        {
            foreach (FishAgent fish in fishList)
            {
                fish.Update(fishList, obstacles, MAX_WIDTH, MAX_HEIGHT); // Changement de direction d'Alignement.
            }
        }

		// Ajouter des obstacles à la liste.
        public void AddObstacle(double _posX, double _posY, double _radius) {
            obstacles.Add(new BadZone(_posX, _posY, _radius));
        }
    }
}
