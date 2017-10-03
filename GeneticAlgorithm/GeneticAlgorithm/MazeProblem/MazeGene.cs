namespace GeneticAlgorithm
{
    internal class MazeGene : IGene
    {
        
		// Le gene est une direction.
		// Construction du gene.
		// Conversion du gene.
		// Mutation du gene --> par Tirage au Sort.

        public Maze.Direction direction;

        public MazeGene()
        {
            direction = (Maze.Direction)Parameters.randomGenerator.Next(4);
			// Constructeur pour créer un gène aléatoirement.
        }

        public MazeGene(MazeGene g)
        {
            direction = g.direction;
			// Constructeur pour copier un gène donné en paramètre.
        }

        public override string ToString()
        {
            return direction.ToString().Substring(0,1);
			// Affiche que la première lettre de la direction.
        }

        public void Mutate()
        {
            direction = (Maze.Direction) Parameters.randomGenerator.Next(4);
			//  refaire un tirage au sort pour une nouvelle direction (4 directions).
        }
    }
}
