using Rdn_Dev.Events;
using Rdn_Dev.LibFunction;
using System;

namespace Rdn_Dev
{
    internal class Program
    {
        /* definitions des classes necessaires aux reseaux de neuronnes pour leur construction */

        private const double KA = 0.746;  // pour l'instant cette constante est au pif a ameliorer
        private const int MAXNEURONNE = 150; // le nombre maximal qu'une couche peut avoir de neuronne
        private const double MU = 0.3;
        private const double NU = 0.1;
        private const int KPRECISION = 100;
        private const int NB_EX = 6; //63
        public static void Main()
        {
            double[][] inputs = {
                 new[] {0.0D, 0.0D,0.0D, 0.0D},
                 new[] {0.0D, 0.0D,0.0D, 1.0D},
                 new[] {0.0D, 0.0D,1.0D, 0.0D},
                 new[] {0.0D, 1.0D,0.0D, 0.0D},
                 new[] {1.0D, 0.0D,0.0D, 0.0D},
                 new[] {1.0D, 1.0D,1.0D, 1.0D}
             };

            double[][] desiredOutputs = {
                    new[] {0.0D},
                    new[] {0.0D},
                    new[] {1.0D},
                    new[] {0.0D},
                    new[] {0.0D},
                    new[] {1.0D}
                };

            int NeuroneOutput = 0;
            if (desiredOutputs.Length > 0)
            {
                NeuroneOutput = (int)desiredOutputs[0].Length;
            }

            NeuralLayer[] hiddenLayers = new[] { new NeuralLayer(10) };

            IFunctions f = new Sigmoid();
            NeuronalNetwork nn = new NeuronalNetwork(5, NeuroneOutput, hiddenLayers, f);

            nn.randomizeWeight(-1.0D, 1.0D);

            NeuralNetworkLearningEvent e = new NeuralNetworkLearningEvent(null, 0, 0.0D);

            //  nn.addLearningListener(e);

            nn.learn(inputs, desiredOutputs, 10000, 0.9D);

            for (int i = 0; i < inputs.Length; i++)
            {
                double[] output = nn.compute(inputs[i]);
                for (int j = 0; j < output.Length; j++)
                {
                    Console.WriteLine(inputs[i][0] + ";" + inputs[i][1] + " = " + output[j]);
                }
            }

            Console.WriteLine("press any key to exit:");
            Console.ReadKey();
        }
    }
}