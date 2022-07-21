using Microsoft.AspNetCore.Identity;

namespace WebApp.Services
{
    public class LoggedUserProvider
    {
        private readonly IHttpContextAccessor _httpCtx;
        private readonly UserManager<IdentityUser> _userMan;
        public LoggedUserProvider(IHttpContextAccessor httpCtx, UserManager<IdentityUser> userMan)
        {
            _httpCtx = httpCtx;
            _userMan = userMan;
        }

        public async Task<IdentityUser> GetLoggedUser()
        {
            var user = _httpCtx.HttpContext.User;
            if (user.Identity == null || !user.Identity.IsAuthenticated)
                return null;

            return await _userMan.GetUserAsync(user);
        }
    }
}
