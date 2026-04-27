public interface IInventoryService
{
    void Initialize(int totalInventory);
    object Reserve(int userId);
    object GetSatus();
}