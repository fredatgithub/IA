using System;

namespace Rdn_Dev.LibFunction
{
    internal class Gaussien:
        IFunctions

   
    {
        private static double a = 1.0D / Math.Sqrt(6.283185307179586D);

        public  double Compute(double paramDouble)
        {
            return calculate(paramDouble);
        }

        public  double computeDerivative(double paramDouble)
        {
            return paramDouble * calculate(paramDouble);
        }

        private double calculate(double paramDouble)
        {
            return a * Math.Exp(-(paramDouble * paramDouble) / 2.0D);
        }
    }
}
