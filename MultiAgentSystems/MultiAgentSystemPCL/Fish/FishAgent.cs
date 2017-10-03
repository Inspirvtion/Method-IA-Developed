using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiAgentSystemPCL
{
    public class FishAgent : ObjectInWorld
    {
		protected const double STEP = 3; // La distance parcourue à chaque itération (STEP) en unité arbitraire.
		protected const double DISTANCE_MIN = 5; // La distance indiquant quelle est la zone d’évitement (Pour ne pas entrer en colision avec cette derniere).
        protected const double SQUARE_DISTANCE_MIN = 25;
        //protected const double DISTANCE_MAX = 40;
		protected const double SQUARE_DISTANCE_MAX = 1600; // La distance indiquant jusqu’où va la zone d’alignement (Pour s'aligner derreire cette derniere).

		// la direction des poissons est représentée par le déplacement en x et le déplacement en y à chaque itération.
        protected double speedX;
        public double SpeedX { get { return speedX; } }

        protected double speedY;
        public double SpeedY { get { return speedY; } }

        internal FishAgent(double _x, double _y, double _dir) // Le constructeur.
        {
            PosX = _x;
            PosY = _y;
            speedX = Math.Cos(_dir);
            speedY = Math.Sin(_dir);
        }

		internal void UpdatePosition() // Ajouter à la position actuelle la vitesse multipliée par la longueur du déplacement.
        {
            PosX += STEP * SpeedX;
            PosY += STEP * SpeedY;
        }

		// Savoir si un autre poisson est proche, ie dans la zone d’alignement (donc entre la distance minimale et la distance maximale).
        private bool Near(FishAgent _fish)
        {
            double squareDistance = SquareDistanceTo(_fish);
            return squareDistance < SQUARE_DISTANCE_MAX && squareDistance > SQUARE_DISTANCE_MIN;
        }

        internal double DistanceToWall(double _wallXMin, double _wallYMin, double _wallXMax, double _wallYMax)
        {
			// Renvoit la plus petit distance.
            double min = double.MaxValue;
            min = Math.Min(min, PosX - _wallXMin);
            min = Math.Min(min, PosY - _wallYMin);
            min = Math.Min(min, _wallYMax - PosY);
            min = Math.Min(min, _wallXMax - PosX);
            return min;
        }

		protected void Normalize() // Normalisation : Vitesse du poisson constante dans le temps.
		{
			double speedLength = Math.Sqrt(SpeedX * SpeedX + SpeedY * SpeedY);
			speedX /= speedLength;
			speedY /= speedLength;
		}


		/*************S’il y a un poisson dans la zone proche, on s’aligne sur lui (règle 4)***************/
		internal void ComputeAverageDirection(FishAgent[] _fishList) // Alignement du poisson.
        {
			// Liste des poissons dans notre zone d’alignement.
            List<FishAgent> fishUsed = _fishList.Where(x => Near(x)).ToList();
            int nbFish = fishUsed.Count; // Nombre de poisson dans la zone d'alignement.
            if (nbFish >= 1)
            {
				// On initialise la somme entre la direction des autres poissons.
                double speedXTotal = 0;
                double speedYTotal = 0;

				// On calcul la somme entre la direction des autres poissons.
                foreach (FishAgent neighbour in fishUsed)
                {
                    speedXTotal += neighbour.SpeedX;
                    speedYTotal += neighbour.SpeedY;
                }
				/* La nouvelle direction du poisson est une moyenne entre la direction des autres poissons 
				 * et sa direction actuelle*/
                speedX = (speedXTotal / nbFish + speedX) /2;
                speedY = (speedYTotal / nbFish + speedY) /2;

				//Puis on normalise la direction.
                Normalize();
            }
        }
		/************************************************************************************************/


		/***********S’il y a un mur dans la zone très proche, alors on l’évite (règles 1).*****************/
        internal bool AvoidWalls(double _wallXMin, double _wallYMin, double _wallXMax, double _wallYMax)
        {
			// S'arrêter aux murs si le déplacement aurait permis de sortir de notre océan virtuel.
            if (PosX < _wallXMin)
            {
                PosX = _wallXMin;
            }
            if (PosY < _wallYMin)
            {
                PosY = _wallYMin;
            }
            if (PosX > _wallXMax)
            {
                PosX = _wallXMax;
            }
            if (PosY > _wallYMax)
            {
                PosY = _wallYMax;
            }

            // Changer de direction
            double distance = DistanceToWall(_wallXMin, _wallYMin, _wallXMax, _wallYMax);
            if (distance < DISTANCE_MIN)
            {
                if (distance == (PosX - _wallXMin))
                {
                    speedX += 0.3;
                }
                else if (distance == (PosY - _wallYMin))
                {
                    speedY += 0.3;
                }
                else if (distance == (_wallXMax - PosX))
                {
                    speedX -= 0.3;
                }
                else if (distance == (_wallYMax - PosY))
                {
                    speedY -= 0.3;
                }
                Normalize();
                return true;
            }
            return false;
        }
		/************************************************************************************************/

		/******** S’il y a un poisson dans la zone très proche, on s’en éloigne  (règle 3).********/
		internal bool AvoidFish(FishAgent _fishAgent) //  Eviter les opoissons.
        {
			double squareDistanceToFish = SquareDistanceTo(_fishAgent); // Calcul le vecteur unitaire entre l’agent et ce poisson. 

			if (squareDistanceToFish < SQUARE_DISTANCE_MIN) // Si le poisson se trouve dans la zone d'Evitement.
            {
				// Calculer le vecteur direction Diff normalisé entre nous (This) et le poisson en Parametre.
                double diffX = (_fishAgent.PosX - PosX) / Math.Sqrt(squareDistanceToFish);
                double diffY = (_fishAgent.PosY - PosY) / Math.Sqrt(squareDistanceToFish); 

				// On applique une modification de notre vecteur vitesse en y retranchant le quart de ce vecteur Diff.
				speedX = SpeedX - diffX/4; // speedX = Math.Cos(_dir);		
				speedY = SpeedY - diffY/4; // speedY = Math.Sin(_dir); 
				// Puis on normalise le vecteur direction.
                Normalize();
				return true;  // On renvoie vrai si on a dû éviter une zone.
            }
            return false;
        }
		/************************************************************************************************/


		/******** S’il y a une zone à éviter dans la zone très proche, alors on l’évite (règle 2).********/
		internal bool AvoidObstacle(List<BadZone> _obstacles) //  Eviter les obstacles.
        {
			// Calcul la zone à éviter la plus proche de laquelle on est.
            BadZone nearestObstacle = _obstacles.Where(x => SquareDistanceTo(x) < x.Radius*x.Radius).FirstOrDefault(); 

			if (nearestObstacle != null) // S’il y a effectivement un obstacle très proche de nous.
            {
				// Calculer le vecteur direction Diff normalisé entre nous et l’obstacle.
                double distanceToObstacle = DistanceTo(nearestObstacle);
                double diffX = (nearestObstacle.PosX - PosX) / distanceToObstacle;
                double diffY = (nearestObstacle.PosY - PosY) / distanceToObstacle;

				// On applique une modification de notre vecteur vitesse en y retranchant la moitié de ce vecteur Diff.
				speedX = SpeedX - diffX/2; // speedX = Math.Cos(_dir);	
				speedY = SpeedY - diffY/2; // speedY = Math.Sin(_dir);
				// Puis on normalise le vecteur direction.
                Normalize();
				return true; // On renvoie vrai si on a dû éviter une zone.
            }
            return false;
        }
		/************************************************************************************************/



        internal void Update(FishAgent[] _fishList, List<BadZone> _obstacles, double _max_width, double _max_height)
        { 
            if (!AvoidWalls(0, 0, _max_width, _max_height)) // Si on ne change pas de direction pour eviter un mur.
            {
				if (!AvoidObstacle(_obstacles)) // Si on ne change pas de direction pour eviter un obstacle. 
                {
                    double squareDistanceMin = _fishList.Where(x => !x.Equals(this)).Min(x => x.SquareDistanceTo(this));

                    if (!AvoidFish(_fishList.Where(x => x.SquareDistanceTo(this) == squareDistanceMin).FirstOrDefault()))
					{// Si on ne change pas de direction pour eviter un poisson. (Y a t'il un poisson dans la zone d'Evitement).
						
                        ComputeAverageDirection(_fishList); // Calcul de la nouvelle direction d'alignement du poisson.
                    }
                }
            }
            UpdatePosition();
        }

    }
}
