using System;
using System.Runtime.Serialization;

namespace game.save
{
    [Serializable]
    public class SavedDataLoadException : Exception
    {
        private InvalidCastException exception;

        public SavedDataLoadException() : base("The saved object has a different serialization that the one wanted.")
        {
        }

        public SavedDataLoadException(string message) : base(message)
        {
        }

        public SavedDataLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SavedDataLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
