namespace ContractorSimulator.Networking
{
    /// <summary>
    /// MVP authority model: listen-server host with server-owned gameplay state.
    /// </summary>
    public static class NetworkAuthorityRules
    {
        public const int MvpMaxPlayers = 2;

        /// <summary>Host runs as server in MVP listen-server sessions.</summary>
        public const bool UseListenServerHost = true;

        /// <summary>Repair progress, tools, interactables, contracts, and economy are server-owned.</summary>
        public const bool RepairStateIsServerAuthoritative = true;

        /// <summary>World interactables and contract state must not be client-authoritative.</summary>
        public const bool WorldStateIsServerAuthoritative = true;

        public static bool IsServerAuthorityRequired(NetworkStateDomain domain)
        {
            return domain switch
            {
                NetworkStateDomain.RepairProgress => RepairStateIsServerAuthoritative,
                NetworkStateDomain.ToolUsage => true,
                NetworkStateDomain.InteractableState => WorldStateIsServerAuthoritative,
                NetworkStateDomain.ContractProgress => true,
                NetworkStateDomain.Economy => true,
                NetworkStateDomain.PlayerMovement => false,
                _ => true
            };
        }

        public static bool CanClientWrite(NetworkStateDomain domain, bool isServer)
        {
            if (isServer)
            {
                return true;
            }

            return domain == NetworkStateDomain.PlayerMovement;
        }
    }

    public enum NetworkStateDomain
    {
        PlayerMovement,
        RepairProgress,
        ToolUsage,
        InteractableState,
        ContractProgress,
        Economy
    }

    public enum NetworkOwnershipKind
    {
        Server,
        ClientPlayer,
        None
    }
}
