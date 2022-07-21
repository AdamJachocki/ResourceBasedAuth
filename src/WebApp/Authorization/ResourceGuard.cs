//using Microsoft.AspNetCore.Authorization;
//using WebApp.Data.DbModels;

//namespace WebApp.Authorization
//{
//    //z tego powinieneś zrobić interfejs, jeśli chcesz używać.
//    public class ResourceGuard
//    {
//        private readonly IAuthorizationService _authService;
//        private readonly IHttpContextAccessor _httpCtx;

//        public ResourceGuard(IAuthorizationService authService, IHttpContextAccessor httpCtx)
//        {
//            _authService = authService;
//            _httpCtx = httpCtx;
//        }

//        public async Task<AuthorizationResult> LoggedIsAuthorized<T>(object resource)
//            where T: IAuthorizationRequirement, new()
//        {
//            var requirement = new T();
//            var user = _httpCtx.HttpContext.User;

//            //tu możesz sprawdzić, czy user jest super adminem albo pójść dalej:

//            return await _authService.AuthorizeAsync(user, resource, requirement);
//        }
//    }

//    class Test
//    {
//        private readonly ResourceGuard _guard;

//        public Test(ResourceGuard guard)
//        {
//            _guard = guard;
//        }

//        public async Task DeleteItem(TodoItem item)
//        {
//            var authResult = await _guard.LoggedIsAuthorized<TodoItemOwnerOrSuperAdminRequirement>(item);
//            if (!authResult.Succeeded)
//                return;
//            else
//            {
//                //todo: usuń
//            }
//        }
//    }
//}
