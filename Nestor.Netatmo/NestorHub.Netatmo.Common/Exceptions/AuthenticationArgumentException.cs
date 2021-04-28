using System;
using System.Runtime.Serialization;

namespace NestorHub.Netatmo.Common.Exceptions
{
    [Serializable]
    public class AuthenticationArgumentException : Exception
    {
        protected AuthenticationArgumentException(SerializationInfo info, StreamingContext ctx)
            : base(info, ctx)
        { }

        public AuthenticationArgumentException()
            : base("Username or password doesn't be empty")
        {}
    }
}