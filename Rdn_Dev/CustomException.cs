using System;

namespace Rdn_Dev
{

    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException()
        {
        }

        public IllegalArgumentException(string message)
            : base(message)
        {
        }

        public IllegalArgumentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}