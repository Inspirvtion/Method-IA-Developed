using System.Collections.Generic;
using System.Linq;

namespace PathfindingPCL
{
    public class AStar : Algorithm
    {
        public AStar(Graph _graph, IHM _ihm) : base(_graph, _ihm) { }

        protected override void Run()
        {
            // Initialisation
            graph.ComputeEstimatedDistanceToExit();
            List<Node> nodesToVisit = graph.NodesList();
            bool exitReached = false;

            // Boucle principale
            while (nodesToVisit.Count != 0 && !exitReached)
            {
                Node currentNode = nodesToVisit.FirstOrDefault();
                foreach (Node newNode in nodesToVisit)
                {
                    if ((newNode.DistanceFromBegin + newNode.EstimatedDistance) < (currentNode.DistanceFromBegin + currentNode.EstimatedDistance))
                    {
                        currentNode = newNode;
                    }
                }
                
                if (currentNode == graph.ExitNode())
                {
                    // Sortie trouvée : on a fini
                    exitReached = true;
                }
                else
                {
                    // On applique tous les arcs sortant du noeud
                    List<Arc> arcsFromCurrentNode = graph.ArcsList(currentNode);

                    foreach (Arc arc in arcsFromCurrentNode)
                    {
                        if (arc.FromNode.DistanceFromBegin + arc.Cost < arc.ToNode.DistanceFromBegin)
                        {
                            arc.ToNode.DistanceFromBegin = arc.FromNode.DistanceFromBegin + arc.Cost;
                            arc.ToNode.Precursor = arc.FromNode;
                        }
                    }
                    
                    nodesToVisit.Remove(currentNode);
                }
            }
        }
    }
}
