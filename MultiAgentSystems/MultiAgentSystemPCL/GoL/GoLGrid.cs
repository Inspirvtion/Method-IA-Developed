using System;

namespace MultiAgentSystemPCL
{
    public delegate void gridUpdated(bool[][] newGrid);
    public class GoLGrid
    {
		protected int WIDTH; // largeur en nombre de cellules.
		protected int HEIGHT; // hauteur en nombre de cellules.
		bool[][] gridCells = null; // un tableau à deux dimensions contenant toutes les cellules.

        public event gridUpdated gridUpdatedEvent = null; // Mise à jour terminée.

        public GoLGrid(int _width, int _height, double _cellDensity) // Constructeur.
        {
			// paramètres : la largeur, la hauteur de la grille et la densité en cellules vivantes au départ.
            WIDTH = _width;
            HEIGHT = _height;

            Random randomGen = new Random();

            gridCells = new bool[WIDTH][]; // Création de la grille.
            for(int i = 0; i < WIDTH; i++) {
                gridCells[i] = new bool[HEIGHT];
                for (int j = 0; j < HEIGHT; j++)
                {
                    if (randomGen.NextDouble() < _cellDensity)
                    {
                        gridCells[i][j] = true;
                    }
                }
            }
        }

        public void Update(bool _withUpdate = true)
        {
            if (_withUpdate)
            {
                bool[][] newGrid = new bool[WIDTH][];
                for (int i = 0; i < WIDTH; i++)
                {
                    newGrid[i] = new bool[HEIGHT];
                    for (int j = 0; j < HEIGHT; j++)
                    {
                        int count = NbCellAround(i, j); // Agit sur gridCells.
                        if (count == 3 || (count == 2 && gridCells[i][j]))
                        {
							/* Si elle a trois voisines, elle sera vivante dans la prochaine grille.
		                     * Si elle a deux voisines et qu’elle était vivante, alors elle le restera.*/

                            newGrid[i][j] = true;
                        }
                    }
                }

				gridCells = newGrid; // On remplace ensuite l’ancienne grille par la nouvelle calculée.
            }

			if (gridUpdatedEvent != null) // On déclenche l’évènement indiquant la fin de la mise à jour
            {
                gridUpdatedEvent(gridCells);
            }
        }

		// Connaître le nombre de voisines vivantes.
        private int NbCellAround(int _cellRow, int _cellCol) // Position de la cellule cible sur la grille.
        {
            int count = 0;
			/* Ne pas sortir de la grille, aussi on commence par vérifier les coordonnées minimales et maximales
			 * testées par rapport aux dimensions de l’environnement*/
            int row_min = (_cellRow - 1 < 0 ? 0 : _cellRow - 1);
            int row_max = (_cellRow + 1 > WIDTH-1 ? WIDTH-1 : _cellRow + 1);
            int col_min = (_cellCol - 1 < 0 ? 0 : _cellCol - 1); ;
            int col_max = (_cellCol + 1 > HEIGHT-1 ? HEIGHT-1 : _cellCol + 1); ;

            for (int row = row_min; row <= row_max; row++)
            {
                for (int col = col_min; col <= col_max; col++)
                {
                    if (gridCells[row][col] && !(row == _cellRow && col == _cellCol)) {
						// La cellule doit etre vivante : gridCells[row][col] = true et different de la cellule cible.
                        count++;
                    }
                }
            }
            return count;
        }

		// Pour changer l’état d’une cellule : inverser la valeur du booléen.
        public void ChangeState(int _row, int _col)
        {
            gridCells[_row][_col] = !gridCells[_row][_col];
        }
    }
}
