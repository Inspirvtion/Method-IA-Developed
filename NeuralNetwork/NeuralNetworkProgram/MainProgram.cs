using NeuralNetworkPCL;
using System;
using System.IO;
using System.Linq;

namespace NeuralNetworkProgram
{
    class MainProgram : IHM
    {
        static void Main(string[] args)
        {
            MainProgram prog = new MainProgram();
            prog.Run();
        }

        private void Run()
        {
            // Problème du OU Exclusif (XOR)
            /*String[] content = ReadFile("xor.txt", true);
            NeuralSystem system = new NeuralSystem(2, 2, 1, content, 1.0, this);*/

            // Problème Abalone
            String[] content = ReadFile("abalone_norm.txt", true); // Le fichier est ajouté au projet comme contenu, et avec la propriété "copier si plus récent"
            NeuralSystem system = new NeuralSystem(10, 4, 1, content, 0.8, this); //NeuralSystem(int _nbInputs, int _nbHidden, int _nbOutputs, String[] _data, double _trainingRatio, IHM _ihm).
            system.LearningRate(0.1);

            system.Run();

            while (true) ;
        }

        private String[] ReadFile(String _filename, bool _removeFirstLine) // Creation du Contenu.
        {

			// ReadFile qui se contente de récupérer toutes les lignes d’un fichier indiqué.
            String[] content = File.ReadAllLines(@_filename);

            if (_removeFirstLine)
            {
                content = content.Skip(1).ToArray();
            }

            return content;
        }

        public void PrintMsg(string _msg)
        {
            Console.Out.WriteLine(_msg);
        }
    }
}
