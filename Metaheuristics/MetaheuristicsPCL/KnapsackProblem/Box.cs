using System;

namespace MetaheuristicsPCL
{
    public class Box
    {
        public double Weight { get; set; } // Poid du Box.
        public double Value { get; set; } // Valeur du Box.

        String Name { get; set; } // Nom du Box.

        public Box(String _name, double _weight, double _value) // Constructeur.
        {
            Name = _name;
            Weight = _weight;
            Value = _value;
        }

		public override string ToString() // Affichage du Box : A (4Kg , 15).
        {
            return Name + " (" + Weight + ", " + Value + ")"; 
        }
    }
}
