
namespace MultiAgentSystemPCL
{
    public class Waste : ObjectInWorld
    {
		// Constante permettant de savoir à quelle vitesse la probabilité de prendre un élément sur un tas diminue avec la taille.
        private const double PROBA_DECREASE = 0.6;

		protected int type; // Une indiquant le type du déchet, sous la forme d’un entier.
        public int Type
        {
            get { return type; }
        }

		protected int size = 1; // Une indiquant la taille du tas en nombre de déchets posés à cet endroit.
        public int Size
        {
            get { return size; }
        }

		public int Zone // Chaque tas a une zone d’influence représentant sa portée.
        {
			// un élément seul a une visibilité de 10, et chaque élément supplémentaire rajoute 8 de visibilité.
            get { return 10 + (8 * size - 1); }
        }

        public Waste(double _posX, double _posY, int _type) // Constructeur. 
        { 
			// Par :  la position et le type d’un tas
            PosX = _posX;
            PosY = _posY;
            type = _type;
        }

        public Waste(Waste _goal) // Constructeur
        {
			// Par : copie d’un élément existant (mais avec une taille initiale de 1).
            PosX = _goal.PosX;
            PosY = _goal.PosY;
            type = _goal.type;
        }

		internal void Decrease() // agent prenant un élément.
        {
            size--;
        }

		internal void Increase() // agent posant un élément.
        {
            size++;
        }

        internal double ProbaToTake()
		{ /*Ici, elle est de 0.6. Cela signifie que si la probabilité de prendre un élément seul est de 100 %,
		   *la probabilité de prendre un élément sur une pile de 2 est de 60 %, celle d’en prendre un sur une
		   *pile de trois est de 60*0.6 = 36 %, celle d’en prendre un sur une pile de quatre de 36*0.6 = 21.6 %
		   *et ainsi de suite.*/
            double proba = 1.0;
            for (int i = 1; i < size; i++)
            {
                proba *= PROBA_DECREASE;
            }
            return proba;
        }
    }
}
