using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralNetworkPCL
{
    internal class DataCollection
    {
        DataPoint[] trainingPoints; // Ensemble d'exemple pour l'apprentissage.
        DataPoint[] generalisationPoints; // Ensemble d'exemple pour la generalisation.

		// Constructeur.
        internal DataCollection(String [] _content, int _outputNb, double _trainingRatio)
        {
			/* P : la chaîne correspondant à l’intégralité du fichier sous la forme d’un tableau 
			 * (une ligne par case), le nombre de sorties, et le ratio correspondant aux points d’apprentissage.*/

			// Lecture des points Exemples (ligne par ligne).
            int nbLines = _content.Length;
            List<DataPoint> points = new List<DataPoint>();
            for (int i = 0; i < nbLines; i++)
            {
                points.Add(new DataPoint(_content[i], _outputNb)); // Creation et Ajout des points d'Exemples.
            }

			/* L’ensemble d’apprentissage est créé en prenant le nombre d’exemples requis. 
			 * Ceux-ci sont choisis aléatoirement parmi les points restants.*/
            int nbTrainingPoints = (int) (_trainingRatio * nbLines); 
			// Nbre de lignes (exemples) pour l'ensemble d'apprentissage.
            trainingPoints = new DataPoint[nbTrainingPoints];
			// Creation de l'ensemble d'apprentissage.
            Random rand = new Random();
            for (int i = 0; i < nbTrainingPoints; i++)
            {
                int index = rand.Next (points.Count);
                trainingPoints[i] = points.ElementAt(index);
                points.RemoveAt(index);
            }

			/*L’ensemble de généralisation est enfin créé à partir des exemples non encore sélectionnés.*/
            generalisationPoints = points.ToArray();
        }

        internal DataPoint[] Points()
        {
			return trainingPoints; // Recuperation des points d'apprentissage (Exemples).
        }

        internal DataPoint[] GeneralisationPoints()
        {
			return generalisationPoints; // Recuperation des points de generalisation (Exemples).
        }
    }
}
