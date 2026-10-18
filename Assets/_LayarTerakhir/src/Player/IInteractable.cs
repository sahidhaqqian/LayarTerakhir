namespace LayarTerakhir.Player
{
    public interface IInteractable
    {
        string InteractableId { get; }
        string GetInteractionPrompt();
        bool CanInteract();
        void Interact();
    }
}
