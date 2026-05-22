using Unity.Netcode;
using UnityEngine;

namespace ContractorSimulator.Networking
{
    /// <summary>
    /// Base for networked gameplay state that is owned and written by the server.
    /// </summary>
    public abstract class ServerAuthoritativeNetworkBehaviour : NetworkBehaviour, IServerAuthoritative
    {
        public abstract NetworkStateDomain StateDomain { get; }

        protected bool CanWriteState()
        {
            return IsServer && NetworkAuthorityRules.CanClientWrite(StateDomain, IsServer);
        }

        protected bool ValidateServerWrite()
        {
            if (IsServer)
            {
                return true;
            }

            Debug.LogWarning(
                $"[{GetType().Name}] Rejected non-server write for {StateDomain}. " +
                "MVP uses server-authoritative state.");
            return false;
        }
    }
}
