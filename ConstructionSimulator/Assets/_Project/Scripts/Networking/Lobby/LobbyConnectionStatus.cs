namespace ContractorSimulator.Networking.Lobby
{
    public enum LobbyConnectionStatus
    {
        Offline,
        StartingHost,
        Hosting,
        Connecting,
        Connected,
        Disconnecting,
        Failed
    }
}
