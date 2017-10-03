using System;

namespace NeuralNetworkPCL
{
    internal class DataPoint // Exemple pour l'Apprentissage.
    {
        double[] inputs;  // Tableau des Entrées.    
        internal double[] Inputs
        {
            get
            {
                return inputs; // Getter.
            }
        }

        double[] outputs;  // Tableau des Sorties.
        internal double[] Outputs
        {
            get
            {
                return outputs; // Getter.
            }
        }

		// Constructeur.
        internal DataPoint(string _str, int _outputNb)  
        {
			// P : la chaîne correspondant à la ligne du fichier texte, et le nombre de sorties des exemples.

			// Le contenu est d’abord séparé sur les caractères correspondant à la touche tabulation (’\t’).
            string[] content = _str.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

			inputs = new double[content.Length - _outputNb]; // Nbre d'elements : content.Length - _outputNb.
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = Double.Parse(content[i]); // Ajout des elements au tableau.
            }

            outputs = new double[_outputNb];
            for (int i = 0; i < _outputNb; i++)
            {
                outputs[i] = Double.Parse(content[content.Length - _outputNb + i]); // On remplit avec le reste.
            }
        }
    }
}
