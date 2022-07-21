using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Abstractions;
using WebApp.Data.DbModels;
using WebApp.Services;

namespace WebApp.Pages
{
    [Authorize]
    public class TodoItemsPageModel : PageModel
    {
        public IEnumerable<TodoItem> Items { get; set; } = new List<TodoItem>();

        private readonly ITodoItemService _itemService;
        private readonly LoggedUserProvider _loggedUserProvider;

        public TodoItemsPageModel(ITodoItemService itemService, LoggedUserProvider loggedUserProvider)
        {
            _itemService = itemService;
            _loggedUserProvider = loggedUserProvider;
        }

        public async Task OnGet()
        {
            var logged = await _loggedUserProvider.GetLoggedUser();
            Items = await _itemService.GetItemsForUser(logged.Id);
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            await _itemService.RemoveItem(id);
            return RedirectToPage();
        }
    }
}
