using Microsoft.EntityFrameworkCore;
using WebApp.Abstractions;
using WebApp.Data;
using WebApp.Data.DbModels;

namespace WebApp.Services
{
    public class InsecureTodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _db;
        private readonly LoggedUserProvider _loggedUserProvider;

        public InsecureTodoItemService(ApplicationDbContext db, LoggedUserProvider loggedUserProvider)
        {
            _db = db;
            _loggedUserProvider = loggedUserProvider;
        }

        public async Task AddItem(TodoItem item)
        {
            var user = await _loggedUserProvider.GetLoggedUser();
            item.OwnerId = user.Id;
            item.Done = false;

            _db.TodoItems.Add(item);
            await _db.SaveChangesAsync();
        }

        public async Task<TodoItem> GetItemById(int id)
        {
            return await _db.TodoItems.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<TodoItem>> GetItemsForUser(string userId)
        {
            var query = _db.TodoItems.Where(x => x.OwnerId == userId);
            return await query.ToListAsync();
        }

        public async Task ModifyItem(TodoItem item)
        {
            _db.TodoItems.Update(item);
            await _db.SaveChangesAsync();

        }

        public async Task RemoveItem(int id)
        {
            var model = new TodoItem { Id = id };
            _db.TodoItems.Remove(model);
            await _db.SaveChangesAsync();
        }
    }
}
