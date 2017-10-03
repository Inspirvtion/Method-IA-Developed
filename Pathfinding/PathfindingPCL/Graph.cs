using System;
using System.Collections.Generic;

namespace PathfindingPCL
{
    public interface Graph
    {
        Node BeginningNode(); // Determine le noeud de depart.

        Node ExitNode(); // Determine le noeud de sortie.

        List<Node> NodesList(); // Determine tous les noeuds du graphe.

		List<Node> NodesList(Node _currentNode); // Determine tous les noeuds du graphe relié au noeud courant.

		int NodesCount(); // Compter le nombre de nœuds.

		List<Arc> ArcsList(); // Determine tous les arcs du graphe.

		List<Arc> ArcsList(Node _currentNode); // Determine tous les arcs du graphe relié au noeud courant.

		double CostBetweenNodes(Node _fromNode, Node _toNode); // Retourner la distance entre deux nœuds.

		String ReconstructPath(); // Reconstruire le chemin à partir des prédécesseurs.

		void ComputeEstimatedDistanceToExit(); // Calculer la distance estimée à la sortie.

		void Clear(); // Remettre le graphe dans son état initial.
    }
}
