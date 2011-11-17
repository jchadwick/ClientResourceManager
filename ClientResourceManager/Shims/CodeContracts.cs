// CodeContracts.cs: Lightweight .NET 3 shim for Code Contracts

#if(!DOTNET4)

namespace System.Diagnostics.Contracts
{
    public static class Contract
    {
        public static void Requires(bool condition, string userMessage = null)
        {
            if (condition == false)
                throw new ContractException(userMessage ?? "Condition failed");
        }
    }

    public class ContractException : Exception
    {
        public ContractException(string message) : base(message)
        {
        }
    }
}

#endif