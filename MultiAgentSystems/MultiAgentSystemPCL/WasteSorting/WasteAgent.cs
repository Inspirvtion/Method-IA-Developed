using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiAgentSystemPCL
{
    public class WasteAgent : ObjectInWorld
    {
		protected const double STEP = 3; // la taille d’un pas.
		protected const double CHANGE_DIRECTION_PROB = 0.05; // probabilité de changer de direction à chaque pas de temps. 

		protected Waste load = null; // La charge actuellement portée, de type Waste.
		protected double speedX; // Le vecteur vitesse exprimé par son abcisse speedX.
		protected double speedY; // Le vecteur vitesse exprimé par son ordonné speedY.
		protected WasteEnvironment env; // Une référence vers l’environnement dans lequel il évolue.
		protected bool busy = false; // Un booléen indiquant s’il est actuellement occupé à poser ou prendre une charge, ou non.


        public WasteAgent(double _posX, double _posY, WasteEnvironment _env) // Constructeur.
        {
			// Par : position X et Y et evironnement à l'origine de la création de l'agent.
            PosX = _posX;
            PosY = _posY;
            env = _env;
            speedX = env.randomGenerator.NextDouble() - 0.5;
            speedY = env.randomGenerator.NextDouble() - 0.5;
            Normalize();
        }

		// Normaliser les vecteurs vitesse.
        private void Normalize()
        {
            double length = Math.Sqrt(speedX * speedX + speedY * speedY);
            speedX = speedX / length;
            speedY = speedY / length;
        }

		// 
        public void UpdatePosition(double _maxWidth, double _maxHeight) {
			// Les positions sont incrémentées de la vitesse multipliée par le pas.
            PosX += STEP * speedX;
            PosY += STEP * speedY;
			// On vérifie ensuite que celles-ci ne sortent pas de la zone autorisée.
            if (PosX < 0)
            {
                PosX = 0;
            }
            if (PosY < 0)
            {
                PosY = 0;
            }
            if (PosX > _maxWidth)
            {
                PosX = _maxWidth;
            }
            if (PosY > _maxHeight)
            {
                PosY = _maxHeight;
            }
        }

        internal void UpdateDirection(List<Waste> _wasteList)
        {
            /* Où aller ?*/
			// Trie : Tous les dechets pour lesquels l'agent se trouve dans la zone d'influence.	
            List<Waste> inZone = _wasteList.Where(x => DistanceTo(x) < x.Zone).OrderBy(x => DistanceTo(x)).ToList();
            
			Waste goal;
            if (load == null) // Si nous ne portons aucune charge
            {
                goal = inZone.FirstOrDefault(); // Nous nous dirigeons vers le dechet le plus proche.
            }
            else // Sinon
            {
				goal = inZone.Where(x => x.Type == load.Type).FirstOrDefault(); // Nous nous dirigeons vers le Tas le plus proche ayant le meme type que le dechet porté.
            }

            /* Avons-nous un but ?*/
			if (goal == null || busy) // Nous avons aucun element dans inZone (l'agent ne se trouve dans aucune zone d'influence) | element venant juste d'etre recupéré ou posé.
            {
                // Pas de but, se déplacer aléatoirement
                if (env.randomGenerator.NextDouble() < CHANGE_DIRECTION_PROB)
                {
                    speedX = env.randomGenerator.NextDouble() - 0.5;
                    speedY = env.randomGenerator.NextDouble() - 0.5;
                }
                if (busy && goal == null)
                {
                    busy = false;
                }
            }
			else // Nous avons au moins un element dans inZone (l'agent se trouve dans une zone d'influence).
            {
                // Aller au but
                speedX = goal.PosX - PosX;
                speedY = goal.PosY - PosY;

                // But atteint ? Prendre ou déposer une charge
                if (DistanceTo(goal) < STEP)
                {
                    if (load == null) // Porte pas de charge.
                    {
                        if (env.randomGenerator.NextDouble() < goal.ProbaToTake())
                        {
							load = env.TakeWaste(goal); // Retrait de la charge a l'environnement et stockage dans load.
                        }
                    }
                    else // Porte une charge.
                    {
                        env.SetWaste(goal); // Ajout de la charge a l'environnement.
                        load = null;
                    }
					busy = true; // ne pas redéposer de suite l’objet récupéré ou de reprendre un élément que l’on viendrait juste de poser
                }
            }

            Normalize(); // On normalise.
        }

		// Indique si l'agent est loaded ou non.
        public bool isLoaded()
        {
            return load != null;
        }
    }
}
