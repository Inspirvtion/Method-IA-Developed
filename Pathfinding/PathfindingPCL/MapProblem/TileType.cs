using System;

namespace PathfindingPCL
{
    public enum TileType { Grass, Tree, Water, Bridge, Path };

    internal static class TileTypeConverter
	// Convertisseur.
    {
        public static TileType TypeFromChar(Char _c) // Parametre : Caractere --> Return : TileType.
        {
            switch (_c)
            {
                case ' ':
                    return TileType.Grass;
                case '*':
                    return TileType.Tree;
                case 'X':
                    return TileType.Water;
                case '=':
                    return TileType.Bridge;
                case '.':
                    return TileType.Path;
            }
            throw new FormatException();
        }
    }
}
