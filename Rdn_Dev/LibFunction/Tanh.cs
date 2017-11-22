using System;

namespace Rdn_Dev.LibFunction
{
    internal class Tanh : IFunctions
    {
        public double beta { get; set; }
        public Tanh()
        {
            beta = 1.0D;
        }

        public Tanh(double value)
        {
            beta = value;
        }

        double IFunctions.Compute(double paramDouble)
        {
            double var1 = Math.Pow(2.718281828459045D, beta * paramDouble);
            double var2 = Math.Pow(2.718281828459045D, -(beta * paramDouble));

            return (var1 - var2) / (var1 + var2);
        }

        double IFunctions.computeDerivative(double paramDouble)
        {
            return 1.0D - paramDouble * paramDouble;
        }
    }
}