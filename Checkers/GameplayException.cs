using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    public class GameplayException : Exception
    {
        public GameplayException() { }
        public GameplayException(string message) : base(message) { }
        public GameplayException(string message, Exception inner) : base(message, inner) { }
        protected GameplayException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
