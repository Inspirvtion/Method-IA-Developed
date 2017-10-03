using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm
{
    struct Case // Case du labyrinthe. 
    {
        public int i; // Coordonnées.
        public int j;

        public Case(int _i, int _j) // Constructeur.
        {
            i = _i;
            j = _j;
        }
    }

    public static class Maze
    {
		// Extraction de toutes les portes du labyrinthe.
		// Determination de la case entrante et sortante.
		// Evaluation
		// Savoir si une case est un carrefour
		// Savoir s'il existe un carrefour entre deux portes.

        /*
         * Générateurs de labyrinthes :
         * http://www.desmoulins.fr/index.php?pg=divers!jeux!labyrinthes&ses=1
         * http://www.echodelta.net/mafalda/rectang/dessinlab.php4
         * 
         */

        public static String Maze1 = "*--*--*--*--*\n" +
                                     "E           |\n" +
                                     "*  *  *--*--*\n" +
                                     "|  |  |     |\n" +
                                     "*  *--*  *  *\n" +
                                     "|        |  |\n" +
                                     "*  *--*--*  *\n" +
                                     "|        |  S\n" +
                                     "*--*--*--*--*";

        public static String Maze2 = "*--*--*--*--*--*--*\n" + 
                                     "E        |  |     |\n" + 
                                     "*--*--*  *  *  *--*\n" + 
                                     "|     |     |     |\n" + 
                                     "*  *  *  *  *  *  *\n" + 
                                     "|  |  |  |     |  |\n" + 
                                     "*--*  *  *--*--*  *\n" +
                                     "|     |  |     |  |\n" +
                                     "*  *--*--*  *  *  *\n" +
                                     "|  |        |  |  |\n" +
                                     "*  *  *  *--*  *  *\n" +
                                     "|     |     |     S\n" + 
                                     "*--*--*--*--*--*--*";

        private static List<Tuple<Case, Case>> paths; // liste des portes.
        private static Case entrance; // Case entrante.
        private static Case exit; // Case Sortante.

        public enum Direction { Top, Bottom, Left, Right }; // Les differentes directions.

        public static void Init(String s) // Initialisation de l'environnement
        {
            paths = new List<Tuple<Case, Case>>();

            String[] lines = s.Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            int nbLines = 0;
            foreach (String line in lines)
            {
                if (nbLines % 2 != 0)  // On entre ici au 2,4,6... tour de boucle
                {
                    // Ligne impaire, donc contenu d'un couloir
                    int index = line.IndexOf('E');
                    if (index != -1)
                    {
                        if (index == line.Length - 1)
                        {
                            index--;
                        }
                        entrance = new Case(nbLines / 2, index / 3);
                    }
                    else
                    {
                        index = line.IndexOf('S');
                        if (index != -1)
                        {
                            if (index == line.Length-1)
                            {
                                index--;
                            }
                            exit = new Case(nbLines / 2, index / 3);
                        }
                    }
                    for (int column = 0; column < line.Length / 3; column++) // Par chaque case du couloir
                    {
                        String caseStr = line.Substring(column * 3, 3);
                        if (!caseStr.Contains("|") && !caseStr.Contains("E") && !caseStr.Contains("S"))
                        {
                            paths.Add(new Tuple<Case, Case>(new Case(nbLines / 2, column - 1), new Case(nbLines / 2, column)));
							// Creation d'un passage horizontal entre la case et celle d’avant.
                        }
                    }  
                }
                else // On entre ici au 1er,3,5 tour de boucle.
                {
                    // Ligne paire, donc murs
                    String[] cases = line.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
                    int column = 0;
                    foreach (String mur in cases) {
                        if (mur.Equals("  "))
                        {
                            paths.Add(new Tuple<Case, Case>(new Case(nbLines / 2 - 1, column), new Case(nbLines / 2, column)));
							// Creation d'un passage vertical, de la case du dessus à celle du dessous.
                        }
                        column++;	
                    }
                }
                nbLines++;
            }
        }

        internal static double Evaluate(MazeIndividual individual)
        {
            Case currentPosition = entrance; // La case courant est case de depart.
            
            bool end = false;
			foreach (MazeGene g in individual.genome) // on s'arrete lorsqu’il n’y a plus de gènes à appliquer.
            {
                switch (g.direction)
                {
                    case Direction.Bottom :
                        while (IsPossible(currentPosition, new Case(currentPosition.i + 1, currentPosition.j)) && !end)
						/*La direction demandée est gardée jusqu’à ce qu’il ne soit plus possible d’avancer
						 *ou qu’on arrive à un carrefour.*/
                        {
						    currentPosition.i++; // on change à chaque déplacement la case sur laquelle on est.
                            end = IsJunction(currentPosition) || currentPosition.Equals(exit);
                        }
                        end = false;
                        break;
                    case Direction.Top:
                        while (IsPossible(currentPosition, new Case(currentPosition.i - 1, currentPosition.j)) && !end)
                        {
						   currentPosition.i--; // on change à chaque déplacement la case sur laquelle on est.  
                            end = IsJunction(currentPosition) || currentPosition.Equals(exit); 
                        }
                        end = false;
                        break;
                    case Direction.Right:
                        while (IsPossible(currentPosition, new Case(currentPosition.i, currentPosition.j + 1)) && !end)
                        {
						    currentPosition.j++; // on change à chaque déplacement la case sur laquelle on est.
                            end = IsJunction(currentPosition) || currentPosition.Equals(exit);
                        }
                        end = false;
                        break;
                    case Direction.Left:
                        while (IsPossible(currentPosition, new Case(currentPosition.i, currentPosition.j - 1)) && !end)
                        {
						    currentPosition.j--; // on change à chaque déplacement la case sur laquelle on est.
                            end = IsJunction(currentPosition) || currentPosition.Equals(exit);
                        }
                        end = false;
                        break;
                }
				if (currentPosition.Equals(exit)) { 
					// On s’arrête lorsqu’on arrive sur la case d’arrivée.
                    return 0; 
                }
            }

            int distance = Math.Abs(exit.i - currentPosition.i) + Math.Abs(exit.j - currentPosition.j);
			// On calcule enfin la distance de Manhattan à la sortie, que l’on renvoie.
            return distance;
        }

        private static bool IsPossible(Case pos1, Case pos2)
        {
            return paths.Contains(new Tuple<Case, Case>(pos1, pos2)) || paths.Contains(new Tuple<Case, Case>(pos2, pos1));
			// Détermine s’il est possible d’aller d’une case à une autre
			// Existe il une porte entre ces deux cases.
        }

        private static bool IsJunction(Case pos)
        {
            int nbRoads = paths.Count(x => (x.Item1.Equals(pos) || x.Item2.Equals(pos)));
			// Toutes les portes dont la case passée en parametre est une entrée ou une sortie.
            return nbRoads > 2;
			// Savoir si une case est un carrefour
        }
    }
}
