using UnityEngine;

namespace ContractorSimulator.Managers
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        public bool IsSessionActive { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            IsSessionActive = false;
        }

        public void StartSession()
        {
            IsSessionActive = true;
        }

        public void EndSession()
        {
            IsSessionActive = false;
        }
    }
}
