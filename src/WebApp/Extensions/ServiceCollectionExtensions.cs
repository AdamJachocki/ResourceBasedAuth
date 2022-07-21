using Microsoft.AspNetCore.Authorization;
using WebApp.Abstractions;
using WebApp.Authorization;
using WebApp.Services;

namespace WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInsecureServices(this IServiceCollection services)
        {
            services.AddScoped<ITodoItemService, InsecureTodoItemService>();

            return services;
        }

        public static IServiceCollection AddSecureServices(this IServiceCollection services)
        {
            services.AddScoped<ITodoItemService, SecuredTodoItemService>();
            services.AddScoped<IAuthorizationHandler, TodoItemAuthHandler>();

            return services;
        }
    }
}
