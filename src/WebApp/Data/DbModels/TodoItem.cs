using Microsoft.AspNetCore.Identity;

namespace WebApp.Data.DbModels
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public IdentityUser Owner { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Done { get; set; }
    }
}
