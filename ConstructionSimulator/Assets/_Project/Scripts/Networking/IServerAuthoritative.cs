namespace ContractorSimulator.Networking
{
    /// <summary>
    /// Marks gameplay state that must be written on the server/host only.
    /// </summary>
    public interface IServerAuthoritative
    {
        NetworkStateDomain StateDomain { get; }
    }
}
