using System;

namespace NeuralNetworkPCL
{
    class Neuron
    {
		double[] weights; // Tableau des Poids le reliant aux différentes Entrées et au biais (derniere valeur).
		int nbInputs; // Nombre d'entrées.
        
        double output; // Sortie : Calculé grace a la fonction d'Evaluation.
        internal double Output
        {
            get
            {
                return output;
            }
        }

        internal Neuron(int _nbInputs) // Constructeur.
        {
			// P : Nombre d'entrées

            nbInputs = _nbInputs;
			output = Double.NaN; // La sortie est initialisée à NaN (Not A Number) de manière à différencier le fait qu’elle soit calculée ou non.

            Random generator = new Random();

            weights = new double[(nbInputs + 1)]; // Nombre d'entrées + Biais.
            for (int i = 0; i < (nbInputs + 1); i++)
            {
				weights[i] = generator.NextDouble() * 2.0 - 1.0; // initialisation aléatoirement entre -1 et +1.
            }
        }

        internal double Evaluate(DataPoint _point) // P : Exemple.
        {
            double[] inputs = _point.Inputs;
			return Evaluate(inputs); // Appele de la methode : internal double Evaluate(double[] _inputs). --A--
        }

        internal double Evaluate(double[] _inputs) // P : tableau de valeurs.    ---A---
        {
            if (output.Equals(Double.NaN)) // Si la sortie n'a pas encore été calculée.
            {
                double x = 0.0; // Valeur de la fonction agrégation.

                for (int i = 0; i < nbInputs; i++)
                {
					x += _inputs[i] * weights[i]; // x = Sum(Xi * Wi).
                }
                x += weights[nbInputs]; // plus le poids du biais.

                output = 1.0 / (1.0 + Math.Exp(-1.0 * x)); // Fontion d'activation.
            }

            return output;
        }

        internal void Clear()
        {
			//La dernière méthode permet de réinitialiser la sortie, de manière à pouvoir traiter un nouvel exemple.
            output = Double.NaN;
        }

        internal double Weight(int index) // Renvoit le poids de l'index demandé.
        {
            return weights[index];
        }

        internal void AdjustWeight(int index, double value)
        {
			weights[index] = value; // modifie la valeur d’un poids donné, nécessaire à l’apprentissage.
        }
    }
}
