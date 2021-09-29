using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Neac.BusinessLogic.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        //UserAuthorizeAttribute : IAsyncActionFilter
        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    //var cacheService = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
        //    var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        //    var responseUser = await userService.GetUserByUserName(context.HttpContext.User.Identity.Name);
        //    var roles = responseUser.Data.UserRoles.Select(n => n.Role.RoleCode);

        //    string controllerName = context.ActionDescriptor.RouteValues["controller"].ToString();
        //    string actionName = context.ActionDescriptor.RouteValues["action"].ToString();

        //    //var identityRoles = context.HttpContext.User.Claims.Select(n => n.Value);
        //    if(!roles.Contains(controllerName + "-" + actionName))
        //    {
        //        context.Result = new ContentResult()
        //        {
        //            StatusCode = 401,
        //            ContentType = "application/json",
        //            Content = "bạn không có quyền vào trang này !"
        //        };
        //    }
        //    await next();
        //}

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //var cacheService = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var responseUser = await userService.GetUserByUserName(context.HttpContext.User.Identity.Name);
            var roles = responseUser.Data.UserRoles.Select(n => n.Role.RoleCode);

            string controllerName = context.ActionDescriptor.RouteValues["controller"].ToString();
            string actionName = context.ActionDescriptor.RouteValues["action"].ToString();

            //var identityRoles = context.HttpContext.User.Claims.Select(n => n.Value);
            if (!roles.Contains(controllerName + "-" + actionName))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    ContentType = "application/json",
                    Content = "bạn không có quyền vào trang này !"
                };
            }
        }
    }
}
