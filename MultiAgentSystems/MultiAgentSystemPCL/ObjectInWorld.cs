using System;

namespace MultiAgentSystemPCL
{
    public class ObjectInWorld
    {
        public double PosX; // Abcisse de l'objet.

        public double PosY; // Ordonnée de l'objet.

        public ObjectInWorld() {}

        public ObjectInWorld(double _x, double _y) // Constructeur.
        {
            PosX = _x;
            PosY = _y;	
        }

        public double DistanceTo(ObjectInWorld _object) // Distance ente l'objet courant et l'objet passé en paramètre.
        {
            return Math.Sqrt((_object.PosX - PosX) * (_object.PosX - PosX) + (_object.PosY - PosY) * (_object.PosY - PosY));
        }

        public double SquareDistanceTo(ObjectInWorld _object)
        {
            return (_object.PosX - PosX) * (_object.PosX - PosX) + (_object.PosY - PosY) * (_object.PosY - PosY);
        }
    }
}
