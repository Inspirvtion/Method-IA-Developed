
namespace NeuralNetworkPCL
{
    internal class NeuralNetwork
    {
		Neuron[] hiddenNeurons; // Tableau contenant les neurones cachés.
		Neuron[] outputNeurons; // Tableau contenant les neurones de sortie.
		int nbInputs; // nombre d’entrées. 
		int nbHidden; // Nombre de neurones cachés.
		int nbOutputs; // Nombre de neurones de sorties.

        internal NeuralNetwork(int _nbInputs, int _nbHidden, int _nbOutput) // Constructeur.
        {
            nbInputs = _nbInputs;
            nbHidden = _nbHidden;
            nbOutputs = _nbOutput;

            hiddenNeurons = new Neuron[nbHidden];
            for (int i = 0; i < nbHidden; i++)
            {
                hiddenNeurons[i] = new Neuron(_nbInputs); // création des neurones cachés.
            }

            outputNeurons = new Neuron[nbOutputs];
            for (int i = 0; i < nbOutputs; i++)
            {
                outputNeurons[i] = new Neuron(_nbHidden); // création des neurones de sortie.
            }
        }

        internal double[] Evaluate(DataPoint _point) // Evaluation d'un Exemple.
        {
            foreach (Neuron n in hiddenNeurons)
            {
				n.Clear(); // réinitialiser les sorties des différents neurones cachés.
            }
            foreach (Neuron n in outputNeurons)
            {
				n.Clear(); // réinitialiser les sorties des différents neurones de sortie.
            }

            double[] hiddenOutputs = new double[nbHidden]; // Création du tableau des sorties des neurones cachés
            for (int i = 0; i < nbHidden; i++)
            {
				// Calcul des sorties des différents neurones cachés a parties de leurs entrées.
                hiddenOutputs[i] = hiddenNeurons[i].Evaluate(_point);
            }

			double[] outputs = new double[nbOutputs]; // Création du tableau des sorties des neurones de sortie.
            for (int outputNb = 0; outputNb < nbOutputs; outputNb++)
            {
				// Calcul des sorties des différents neurones de sortie a parties des sorties des neurones cachés.
                outputs[outputNb] =  outputNeurons[outputNb].Evaluate(hiddenOutputs);
            }

            return outputs; // Retourne le tableau des sorties des différents  neurones de sortie.
        }

        internal void AdjustWeights(DataPoint _point, double _learningRate)
        {
			// P : point testé et le taux d’apprentissage. 

            // Deltas pour les sorties
            double[] outputDeltas = new double[nbOutputs]; // Tableau des deltas  des différents neurones de sortie.
            for (int i = 0; i < nbOutputs; i++)
            {
				// Pour chaque neurone de sortie.
                double output = outputNeurons[i].Output; // Sortie Obtenue suit au calcul.
				double expectedOutput = _point.Outputs[i]; // Sortie Attendue.
                outputDeltas[i] = output * (1 - output) * (expectedOutput - output); // Calcul du deltas
            }

            // Deltas pour les neurones cachés
			double[] hiddenDeltas = new double[nbHidden]; // Tableau des deltas  des différents neurones cachés.
            for (int i = 0; i < nbHidden; i++)
            {
                // pour chaque neurones cachés.
                double hiddenOutput = hiddenNeurons[i].Output; // Sortie Obtenue suit au calcul.
                double sum = 0.0;
                for (int j = 0; j < nbOutputs; j++)
                {
					// Pour chaque neurones de sorties reliés au neurones cachés courant.
                    sum += outputDeltas[j] * outputNeurons[j].Weight(i);
					// Sum = deltasSorties[j] * Weight[i-j]. (i : neurones cachés courants,j : neurones de sortie).
                }
                hiddenDeltas[i] = hiddenOutput * (1 - hiddenOutput) * sum; // Calcul du deltas.
            }

            double value;
            // Ajustement des poids des neurones de sortie
            for (int i = 0; i < nbOutputs; i++)
            {
                Neuron outputNeuron = outputNeurons[i];
                for (int j = 0; j < nbHidden; j++)
                {
                    value = outputNeuron.Weight(j) + _learningRate * outputDeltas[i] * hiddenNeurons[j].Output;
					// wi += taux * di * oi 
                    outputNeuron.AdjustWeight(j, value);	
                }
                // Et biais
                value = outputNeuron.Weight(nbHidden) + _learningRate * outputDeltas[i] * 1.0;
                outputNeuron.AdjustWeight(nbHidden, value);
            }

            // Ajustement des poids des neurones cachés
            for (int i = 0; i < nbHidden; i++)
            {
                Neuron hiddenNeuron = hiddenNeurons[i];
                for (int j = 0; j < nbInputs; j++)
                {
                    value = hiddenNeuron.Weight(j) + _learningRate * hiddenDeltas[i] * _point.Inputs[j];
					//wi += taux * di * xi
                    hiddenNeuron.AdjustWeight(j, value);
                }
                // Et biais
                value = hiddenNeuron.Weight(nbInputs) + _learningRate * hiddenDeltas[i] * 1.0;
                hiddenNeuron.AdjustWeight(nbInputs, value);
            }
        }
    }
}
