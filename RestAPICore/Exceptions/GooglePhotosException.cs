using RestAPICore.Models;
using System;

namespace RestAPICore.Exceptions
{
    public class GooglePhotosException : Exception
    {
        public GooglePhotosException()
        {
        }

        public GooglePhotosException(Error error)
            : base(error is object && error.error is object && error.error.message is object ? error.error.message : "unknown")
        {
        }
    }
}