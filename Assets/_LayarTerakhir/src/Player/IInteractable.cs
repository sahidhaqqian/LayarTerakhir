namespace LayarTerakhir.Player
{
    public interface IContextInteractionResolver
    {
        bool OwnsInteraction { get; }
        IInteractable ResolveContextInteraction();
    }
    public interface IInteractable
    {
        string InteractableId { get; }
        string GetInteractionPrompt();
        bool CanInteract();
        void Interact();
    }
}
