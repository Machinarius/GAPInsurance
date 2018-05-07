using System;

namespace GAPInsurance.Domain.Exceptions {
  public class InvalidCoveragesException : Exception {
    public InvalidCoveragesException() : base("Invalid coverage values detected") { }

    public InvalidCoveragesException(string message) : base(message) { }
  }
}
