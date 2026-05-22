namespace ContractorSimulator.Interaction
{
    public interface IToolUsable
    {
        bool CanUseTool { get; }
        string ToolPrompt { get; }
        void UseTool(PlayerInteraction interactor);
    }
}
