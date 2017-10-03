using System.Collections.Generic;

namespace PathfindingPCL
{
    public class DepthFirst : Algorithm
    {
        public DepthFirst(Graph _graph, IHM _ihm) : base(_graph, _ihm) {}

        protected override void Run()
        {
            
			List<Node> notVisitedNodes = graph.NodesList(); // Création de la liste des noeuds non visités.
			Stack<Node> nodesToVisit = new Stack<Node>(); // Création de la pile.
            nodesToVisit.Push(graph.BeginningNode()); // Noeud de depart dans la pile.
            notVisitedNodes.Remove(graph.BeginningNode()); // On mentionne que le noeud de depart a été visité.
            
            Node exitNode = graph.ExitNode(); // Noeud final du graphe.

            bool exitReached = false;
            // Boucle principale
            while (nodesToVisit.Count != 0 && !exitReached)
            {
                Node currentNode = nodesToVisit.Pop(); // Dernier noeud introduit dans la pile.
                if (currentNode.Equals(exitNode)) // Dernier noeud introduit = noeud finale.
                {
                    // On a fini
                    exitReached = true;
                }
                else
                {
                    // On ajoute les voisins
                    foreach (Node node in graph.NodesList(currentNode))
						// Pour chacun des noeuds reliés au noeud courant.
                    {
                        if (notVisitedNodes.Contains(node)) // On s'assure que le noeud n'a pas déja été visité.
                        {
                            notVisitedNodes.Remove(node); // On retire le noeud des lors qu'il a été visité.
                            node.Precursor = currentNode; // le noeud courand est le precursseur du noeud visité.
                            node.DistanceFromBegin = currentNode.DistanceFromBegin + graph.CostBetweenNodes(currentNode, node);
							// Distance du noeudvisité = Distance du noeud courant + Distance de l'arc
                            nodesToVisit.Push(node); // C'est le dernier noeud introduit qui est le noeud consécutif.
                        }
                    }
                }
            }
        }
    }
}
