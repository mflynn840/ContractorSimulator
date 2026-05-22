using UnityEngine;

namespace ContractorSimulator.Interaction
{
    [DisallowMultipleComponent]
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactionPrompt = "Press [E] to interact";
        [SerializeField] private bool canInteract = true;

        public bool CanInteract => canInteract;
        public string InteractionPrompt => interactionPrompt;

        public virtual void Interact(PlayerInteraction interactor)
        {
            Debug.Log($"InteractableBase: {name} was interacted with by {interactor.name}.");
        }
    }
}
