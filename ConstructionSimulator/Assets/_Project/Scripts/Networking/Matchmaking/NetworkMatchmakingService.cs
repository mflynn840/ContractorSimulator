namespace ContractorSimulator.Networking.Matchmaking
{
    /// <summary>
    /// Placeholder for future matchmaking (MVP uses direct host/client connect).
    /// </summary>
    public sealed class NetworkMatchmakingService
    {
        public string LastConnectionAddress { get; private set; } = "127.0.0.1";
        public ushort LastConnectionPort { get; private set; } = 7777;

        public void SetLastEndpoint(string address, ushort port)
        {
            LastConnectionAddress = address;
            LastConnectionPort = port;
        }
    }
}
