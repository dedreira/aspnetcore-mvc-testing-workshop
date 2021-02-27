using System;

namespace Workshop.Testing.Calculator
{
    public class AwesomeCalculator
    {
        public double Add(double operator1, double operator2)
        {
            return operator1 + operator2;
        }

        public double Substract(double operator1, double operator2)
        {
            return operator1 - operator2;
        }

        public double Multiply(double operator1, double operator2)
        {
            return operator1 * operator2;
        }

        public double Divide(double operator1, double operator2)
        {
            if(operator2 == 0)
                throw new DivideByZeroException();

            return operator1 / operator2;
        }
    }
}
