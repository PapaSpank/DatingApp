using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;
        public LogUserActivity(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated)
                return;
            
            var userId = resultContext.HttpContext.User.GetUserId();
            var user = await _userRepository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.Now;
            await _userRepository.SaveAllAsync();

            // var username = resultContext.HttpContext.User.GetUsername();
            // var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
            // var user = await repo.GetUserByUsernameAsync(username);
            // user.LastActive = DateTime.Now;
            // await repo.SaveAllAsync();
        }
    }
}