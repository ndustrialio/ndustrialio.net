using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ndustrialio.api.errors    
{
    [Serializable()]
    public class InvalidAcccessTokenException : Exception
    {
        public InvalidAcccessTokenException() : base() { }
        public InvalidAcccessTokenException(string message) : base(message) { }
        public InvalidAcccessTokenException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an 
        // exception propagates from a remoting server to the client.  
        protected InvalidAcccessTokenException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }

    }
}
