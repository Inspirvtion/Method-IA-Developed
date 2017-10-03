using System.Collections.Generic;
using System.Linq;

namespace PathfindingPCL
{
    public class Dijkstra : Algorithm
    {
        public Dijkstra(Graph _graph, IHM _ihm) : base(_graph, _ihm) { }

        protected override void Run()
        {
            // Initialisation
			List<Node> nodesToVisit = graph.NodesList(); // Création de la liste des noeuds non visités.
            bool exitReached = false;

            // Boucle principale
            while (nodesToVisit.Count != 0 && !exitReached)
            {
                Node currentNode = nodesToVisit.FirstOrDefault();
				foreach (Node newNode in nodesToVisit) // noeud qui n'a pas déja été visité.
                {
					// Retrouvé le noeud qui n'a pas déja été visité et dont la distance est la plus courte.
					if (newNode.DistanceFromBegin < currentNode.DistanceFromBegin) // distance la plus courte.
                    {
                        currentNode = newNode;
                    }
                } 
                
				if (currentNode == graph.ExitNode()) // Dernier noeud courant = noeud finale.
                {
                    exitReached = true;
                }
                else
                {
                    List<Arc> arcsFromCurrentNode = graph.ArcsList(currentNode);
					// Liste des Arcs connectés au noeud courant.

                    foreach (Arc arc in arcsFromCurrentNode)
                    {
						// Pour chacun des arcs reliés au noeud courant.
                        if (arc.FromNode.DistanceFromBegin + arc.Cost < arc.ToNode.DistanceFromBegin)
                        {
                            arc.ToNode.DistanceFromBegin = arc.FromNode.DistanceFromBegin + arc.Cost;
							// Distance du noeud B = Distance du noeud A + Distance de l'arc (AB).
							arc.ToNode.Precursor = arc.FromNode; //le noeud A est le precursseur du noeud B de l'arc (AB).
                        }
                    }

                    nodesToVisit.Remove(currentNode);
                }
            }
        }
    }
}
