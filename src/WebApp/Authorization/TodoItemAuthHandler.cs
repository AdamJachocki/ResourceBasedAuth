using Microsoft.AspNetCore.Authorization;
using WebApp.Data.DbModels;
using WebApp.Services;

namespace WebApp.Authorization
{
    public class TodoItemAuthHandler : AuthorizationHandler<TodoItemOwnerOrSuperAdminRequirement, TodoItem>
    {
        private readonly LoggedUserProvider _loggedUserProvider;

        public TodoItemAuthHandler(LoggedUserProvider loggedUserProvider)
        {
            _loggedUserProvider = loggedUserProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            TodoItemOwnerOrSuperAdminRequirement requirement, 
            TodoItem resource)
        {
            var loggedUser = await _loggedUserProvider.GetLoggedUser();
            if(resource.OwnerId == loggedUser.Id)
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }

    public class TodoItemOwnerOrSuperAdminRequirement: IAuthorizationRequirement
    {

    }
}
