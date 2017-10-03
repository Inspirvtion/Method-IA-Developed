using System;

namespace NeuralNetworkPCL
{
    public class NeuralSystem
    {
		DataCollection data; // Des données d’apprentissage data.
		NeuralNetwork network; // Un réseau de neurones attaché network.
		IHM ihm; // Une IHM utilisée pour les affichages.

        // Configuration
        double learningRate = 0.3; // Taux d'Apprentissage. 
		double maxError = 0.005; // l’erreur sur l’ensemble d’apprentissage est inférieure à un seuil (Erreur Max).
		int maxIterations = 10001; // Le nombre maximal d’itérations a été atteint, et on s’arrête donc.

        public NeuralSystem(int _nbInputs, int _nbHidden, int _nbOutputs, String[] _data, double _trainingRatio, IHM _ihm)
        {
			// Constructeur.
            data = new DataCollection(_data, _nbOutputs, _trainingRatio);
            network = new NeuralNetwork(_nbInputs, _nbHidden, _nbOutputs);
            ihm = _ihm;
        }

		// Modifier la Configuration.
		public void LearningRate(double _rate)
        {
            learningRate = _rate;
        }

        public void MaximumError(double _error)
        {
            maxError = _error;
        }

        public void MaximumIterations(int _iterations)
        {
            maxIterations = _iterations;
        }


        public void Run()
        {
            int i = 0;
            double totalError = Double.PositiveInfinity;
            double oldError = Double.PositiveInfinity;
            double totalGeneralisationError = Double.PositiveInfinity;
            double oldGeneralisationError = Double.PositiveInfinity;
            Boolean betterGeneralisation = true;

            while (i < maxIterations && totalError > maxError && betterGeneralisation)
            {
				/* On met à jour les erreurs de l’itération précédente et on initialise les erreurs pour 
				 * cette itération.*/
                oldError = totalError;
                totalError = 0;
                oldGeneralisationError = totalGeneralisationError;
                totalGeneralisationError = 0;
                
                // Evaluation
                foreach (DataPoint point in data.Points())
                {
					/* Pour chaque point d’apprentissage, on calcule sa sortie et l’erreur commise, 
					 * et on adapte les poids du réseau.*/
                    double[] outputs = network.Evaluate(point); // Tableau des sorties des différents neurones de sortie.
                    for (int outNb = 0; outNb < outputs.Length; outNb++)
                    {
						// Calcal de l'erreur globale dans l'ensemble d'Apprentissage.
                        double error = point.Outputs[outNb] - outputs[outNb];
                        totalError += (error * error);
                    }

                    // Calcul des nouveaux poids par rétropropagation
                    network.AdjustWeights(point, learningRate);
                }

                // Généralisation
                foreach (DataPoint point in data.GeneralisationPoints())
                {
					/*Pour chaque point de généralisation, on calcule la sortie et l’erreur.*/
                    double[] outputs = network.Evaluate(point);
                    for (int outNb = 0; outNb < outputs.Length; outNb++)
                    {
						// Calcal de l'erreur globale dans l'ensemble de Généralisation.
                        double error = point.Outputs[outNb] - outputs[outNb];
                        totalGeneralisationError += (error * error);
                    }
                }
                if (totalGeneralisationError > oldGeneralisationError)
                {
                    betterGeneralisation = false; // Surapprentissage.
                }

                // Changer le taux
                if (totalError >= oldError)
                {
					/* Si l’erreur en apprentissage augmente, c’est que le taux d’apprentissage est trop fort, 
					 * on le divise alors par deux.*/
                    learningRate = learningRate / 2.0;
                }

                // Information et incrément
                ihm.PrintMsg("Iteration n°" + i + " - Total error : " + totalError + " - Gener Error : " + totalGeneralisationError + " - Rate : " + learningRate + " - Mean : " + String.Format("{0:0.00}", Math.Sqrt(totalError/data.Points().Length),"%2"));
                i++;
				// Affichage des valeurs sur l’itération en cours (erreurs et taux).
            }
        }
    }
}
