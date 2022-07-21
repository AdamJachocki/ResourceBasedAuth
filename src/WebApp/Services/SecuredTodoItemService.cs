using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApp.Abstractions;
using WebApp.Authorization;
using WebApp.Data;
using WebApp.Data.DbModels;

namespace WebApp.Services
{
    public class SecuredTodoItemService : ITodoItemService
    {
        private readonly IAuthorizationService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoggedUserProvider _loggedUserProvider;
        private readonly ApplicationDbContext _db;

        public SecuredTodoItemService(IAuthorizationService authService, IHttpContextAccessor httpContextAccessor, 
            ApplicationDbContext db, LoggedUserProvider loggedUserProvider)
        {
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _db = db;
            _loggedUserProvider = loggedUserProvider;
        }

        public async Task AddItem(TodoItem item)
        {
            var user = await _loggedUserProvider.GetLoggedUser();
            item.OwnerId = user.Id;
            item.Done = false;
            var authResult = await _authService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, item,
                new TodoItemOwnerOrSuperAdminRequirement());

            if(authResult.Succeeded)
            {           
                _db.TodoItems.Add(item);
                await _db.SaveChangesAsync();
            }//else return 403
        }

        public async Task<TodoItem> GetItemById(int id)
        {
            var result = await _db.TodoItems.SingleOrDefaultAsync(x => x.Id == id);
            if (result == null)
                return null;

            var authResult = await _authService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, result, 
                new TodoItemOwnerOrSuperAdminRequirement());
            if (authResult.Succeeded)
                return result;
            else
                return null; //403
        }

        public async Task<IEnumerable<TodoItem>> GetItemsForUser(string userId)
        {
            var query = _db.TodoItems.Where(x => x.OwnerId == userId);
            var items = await query.ToListAsync();
            if (items.Count == 0)
                return null; //404

            var authResult = await _authService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, items[0], 
                new TodoItemOwnerOrSuperAdminRequirement());
            if (authResult.Succeeded)
                return items;
            else
                return null; //403
        }

        public async Task ModifyItem(TodoItem item)
        {
            var authResult = await _authService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, item, 
                new TodoItemOwnerOrSuperAdminRequirement());
            if(authResult.Succeeded)
            {
                _db.TodoItems.Update(item);
                await _db.SaveChangesAsync();
            }//else return 403
        }

        public async Task RemoveItem(int id)
        {
            var item = await _db.TodoItems.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return; //404

            var authResult = await _authService.AuthorizeAsync(_httpContextAccessor.HttpContext.User, item,
                new TodoItemOwnerOrSuperAdminRequirement());
                
            if(authResult.Succeeded)
            {
                _db.TodoItems.Remove(item);
                await _db.SaveChangesAsync();
            } //else return 403
        }
    }
}
