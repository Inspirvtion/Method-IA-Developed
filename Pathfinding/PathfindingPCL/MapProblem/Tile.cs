using System;

namespace PathfindingPCL
{
    internal class Tile : Node // Tuile herite de Noeud.
    {
        protected TileType tileType;

        internal int Row { get; set; } // La ligne de la carte.
		internal int Col { get; set; } // La ligne de la carte.

        public Tile(TileType _type, int _row, int _col) 
        {
            tileType = _type;
            Row = _row;
            Col = _col;
        }

        internal bool IsValidPath()
        {
			/* indiquant si on peut aller sur une case ou non. Pour cela, il suffit de regarder le type 
			 * de la case : seuls les chemins, l’herbe et les ponts sont accessibles.*/
            return tileType.Equals(TileType.Bridge) || tileType.Equals(TileType.Grass) || tileType.Equals(TileType.Path);
        }

        internal double Cost()
        {
			// Nous renvoit le cout de la case
            switch (tileType)
            {
                case TileType.Path :
                    return 1;
                case TileType.Bridge:
                case TileType.Grass:
                    return 2;
                default :
                    return double.PositiveInfinity;
            }
        }

        public override string ToString()
        {
            return "[" + Row + ";" + Col + ";" + tileType.ToString() + "]";
        }
    }
}
