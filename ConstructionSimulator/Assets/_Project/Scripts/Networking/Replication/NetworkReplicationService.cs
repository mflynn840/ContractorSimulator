namespace ContractorSimulator.Networking.Replication
{
    /// <summary>
    /// MVP replication guidelines: prefer NetworkVariables for state, ServerRpc only for discrete player actions.
    /// </summary>
    public static class NetworkReplicationService
    {
        public const bool PreferServerRpcForGameplayState = true;
        public const float MinSecondsBetweenRepairRpc = 0.15f;
        public const float MinSecondsBetweenInteractRpc = 0.25f;

        public static bool ShouldAllowAction(float lastActionTime, float minIntervalSeconds)
        {
            return UnityEngine.Time.time - lastActionTime >= minIntervalSeconds;
        }
    }
}
