using System;

namespace Rdn_Dev.LibFunction
{
    internal class Argtanh:IFunctions
    {

        //\forall x\in]-1,1[ artanh(x)=\frac12~\ln\left(\frac{1+x}{1-x}\right)=\frac12(\ln(1+x)-\ln(1-x)),

        double IFunctions.Compute(double paramDouble)
        {
            return 0.5D*(Math.Log((1.0D+paramDouble)/(1.0D-paramDouble)));
        }

        double IFunctions.computeDerivative(double paramDouble)
        {
            return 1.0D/(1.0D-Math.Pow(paramDouble,2));
        }
    }
}
