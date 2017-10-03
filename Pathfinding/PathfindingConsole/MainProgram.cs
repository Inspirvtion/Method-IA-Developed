using System;
using PathfindingPCL;

namespace PathfindingConsole
{
    class MainProgram : IHM
    {
        static void Main(string[] _args)
        {
            MainProgram p = new MainProgram(); // Creation de l'IHM.
            p.Run(); // Execution de l'IHM.
            
            while (true) ;
        }

        private void Run()
        {
            // 1ère carte
            String mapStr = "..  XX   .\n"
                          + "*.  *X  *.\n"
                          + " .  XX ...\n"
                          + " .* X *.* \n"
                          + " ...=...  \n"
                          + " .* X     \n"
                          + " .  XXX*  \n"
                          + " .  * =   \n"
                          + " .... XX  \n"
                          + "   *.  X* ";
			Map map = new Map(mapStr, 0, 0, 9, 9); 
			// Map(String _map, int _beginRow, int _beginColumn, int _exitRow, int _exitColumn)
            RunAllAlgorithms(map);

            // 2ème carte
            mapStr = "...*     X .*    *  \n"
                   + " *..*   *X .........\n"
                   + "   .     =   *.*  *.\n"
                   + "  *.   * XXXX .    .\n"
                   + "XXX=XX   X *XX=XXX*.\n"
                   + "  *.*X   =  X*.  X  \n"
                   + "   . X * X  X . *X* \n"
                   + "*  .*XX=XX *X . XXXX\n"
                   + " ....  .... X . X   \n"
                   + " . *....* . X*. = * ";
            map = new Map(mapStr, 0, 0, 9, 19);
			// Map(String _map, int _beginRow, int _beginColumn, int _exitRow, int _exitColumn)
            RunAllAlgorithms(map);
        }

        private void RunAllAlgorithms(Graph _graph)
        {
            // Résolution par une recherche en profondeur
            RunAlgorithm("Depth-First", _graph);

            // Résolution par une recherche en largeur
            RunAlgorithm("Breadth-First", _graph);

            // Résolution par Bellman-Ford
            RunAlgorithm("Bellman-Ford", _graph);

            // Résolution par Dijkstra
            RunAlgorithm("Dijkstra", _graph);

            // Résolution par A*
            RunAlgorithm("A*", _graph);
        }

        private void RunAlgorithm(string _algoName, Graph _graph)
        {
            // Variables
            DateTime beginning;
            DateTime end;
            TimeSpan duration;
            Algorithm algo = null;

            // Création de l'algorithme
            switch (_algoName)
            {
                case "Depth-First":
				    algo = new DepthFirst(_graph, this); // Algorithm(Graph _graph, IHM _ihm).
                    break;
                case "Breadth-First" :
                    algo = new BreadthFirst(_graph, this);
                    break;
                case "Bellman-Ford" :
                    algo = new BellmanFord(_graph, this);
                    break;
                case "Dijkstra":
                    algo = new Dijkstra(_graph, this);
                    break;
                case "A*":
                    algo = new AStar(_graph, this);
                    break;
            }
            
            // Résolution
            Console.Out.WriteLine("Algorithme : " + _algoName);
            beginning = DateTime.Now;
            algo.Solve();
            end = DateTime.Now;
            duration = end - beginning;
            Console.Out.WriteLine("Durée (ms) : " + duration.TotalMilliseconds.ToString() + "\n");
        }

        public void PrintResult(String _path, double _distance)
        {
            Console.Out.WriteLine("Chemin (taille : " + _distance + ") : " + _path);
        }
    }
}
