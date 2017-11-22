using System;

namespace Rdn_Dev.LibFunction
{
    internal class Sigmoid : IFunctions

    {

        public double beta { get; set; }

        public Sigmoid()
        {
            beta = 1.0D;
        }

        public Sigmoid(double value)
        {
            beta = value;
        }

        public double Compute(double paramDouble)
        {
            //return 1.0D / (1.0D + Math.Pow(2.718281828459045D, -(beta * paramDouble)));
            return 1.0D / (1.0D + Math.Exp(-paramDouble));
        }

        public double computeDerivative(double paramDouble)
        {
            double s = Compute(paramDouble);

            return s * (1 - s);
        }
    }
}
