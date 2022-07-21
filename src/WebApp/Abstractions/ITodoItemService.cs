using WebApp.Data.DbModels;

namespace WebApp.Abstractions
{
    public interface ITodoItemService
    {
        Task AddItem(TodoItem item);
        Task ModifyItem(TodoItem item);
        Task RemoveItem(int id);
        Task<IEnumerable<TodoItem>> GetItemsForUser(string userId);
        Task<TodoItem> GetItemById(int id);
    }
}
