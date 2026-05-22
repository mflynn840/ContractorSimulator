namespace ContractorSimulator.Interaction
{
    public interface IInteractable
    {
        bool CanInteract { get; }
        string InteractionPrompt { get; }
        void Interact(PlayerInteraction interactor);
    }
}
