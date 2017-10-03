using System;
using System.Collections.Generic;

namespace PathfindingPCL
{
    public class BellmanFord : Algorithm
    {
        public BellmanFord(Graph _graph, IHM _ihm) : base(_graph, _ihm) {}

        protected override void Run()
        {
            // Initialisation
            bool distanceChanged = true; 
			/* Est évalué à chaque étape, si sa valeur ne change pas alors les resultats obtenus entre les deux
			 * étapes sont identiques --> On sort de la boucle*/ 
            int i = 0;
            List<Arc> arcsList = graph.ArcsList(); // Liste de tous les Arcs du graphe.

            // Boucle principale
            int nbLoopMax = graph.NodesCount() - 1; // On doit éffectuer Nnoeud - 1 .
            while (i < nbLoopMax && distanceChanged)
            {
                distanceChanged = false;
                foreach (Arc arc in arcsList) // Pour chaque Arcs du graphe.
                {
                    if (arc.FromNode.DistanceFromBegin + arc.Cost < arc.ToNode.DistanceFromBegin)
                    {
						/* Pour chaque Arc on doit enregistrer le noeud de départ et la distance de ce noeud 
						 *depuis le noeud de depart.*/
                        arc.ToNode.DistanceFromBegin = arc.FromNode.DistanceFromBegin + arc.Cost;
                        arc.ToNode.Precursor = arc.FromNode;
                        distanceChanged = true;
                    }
                }
                i++;
            }

            // Test si boucle négative.
			// les resultats obtenus entre la derniere étape de la boucle et celle ci doivent etre identiques.
            foreach (Arc arc in arcsList)
            {
                if (arc.FromNode.DistanceFromBegin + arc.Cost < arc.ToNode.DistanceFromBegin)
                {
                    // Impossible de trouver un chemin
                    throw new Exception();
                }
            }
        }
    }
}
