using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Abstractions;
using WebApp.Data.DbModels;

namespace WebApp.Pages
{
    [Authorize]
    public class TodoItemAddPageModel : PageModel
    {
        [BindProperty] public TodoItem Item { get; set; } = new();

        private readonly ITodoItemService _itemService;
        public TodoItemAddPageModel(ITodoItemService itemService)
        {
            _itemService = itemService;
        }

        public void OnGet()
        {
            Item.Id = 0;
        }
        public async Task OnGetEdit(int id)
        {
            Item = await _itemService.GetItemById(id);
        }

        public async Task<IActionResult> OnPost()
        {
            if (Item.Id > 0)
                await _itemService.ModifyItem(Item);
            else
                await _itemService.AddItem(Item);

            return RedirectToPage("TodoItemsPage");
        }
    }
}
