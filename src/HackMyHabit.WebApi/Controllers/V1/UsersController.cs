using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HackMyHabit.Domain.Users.Actions.Login;
using HackMyHabit.Domain.Users.Actions.Register;
using HackMyHabit.WebApi.Commons;
using HackMyHabit.WebApi.Consts;

namespace HackMyHabit.WebApi.Controllers.V1
{
    public sealed class UsersController : V1ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IUsersSessionsStorage usersSessionsStorage;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CookieOptions cookieOptions;

        public UsersController(IMediator mediator, IOptions<CookieOptions> cookieOptions, IUsersSessionsStorage usersSessionsStorage, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.usersSessionsStorage = usersSessionsStorage;
            this.httpContextAccessor = httpContextAccessor;
            this.cookieOptions = cookieOptions.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Login(LoginRequest request)
        {
            var response = await this.mediator.Send(request);
            this.AddCookie(AuthConsts.ACCESS_TOKEN_COOKIE, response.Token);
            var ip = this.Request.HttpContext.Connection.RemoteIpAddress!.ToString();
            var userAgent = this.Request.Headers[AuthConsts.USER_AGENT_HEADER].ToString();
            var now = DateTime.UtcNow;
            var session = new UserSession(response.UserId, response.Token, now, ip, userAgent);
            usersSessionsStorage.Add(session);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Register(RegisterRequest request)
        {
            await this.mediator.Send(request);
        }

        [HttpPost]
        [AllowAnonymous]
        public Task Logout()
        {
            var token = this.GetCookieValue(AuthConsts.ACCESS_TOKEN_COOKIE);
            if (!string.IsNullOrWhiteSpace(token))
            {
                usersSessionsStorage.RemoveByToken(token);
            }
            this.DeleteCookie(AuthConsts.ACCESS_TOKEN_COOKIE);
            return Task.CompletedTask;
        }

        private void AddCookie(string key, string value) => this.Response.Cookies.Append(key, value, this.cookieOptions);
        private void DeleteCookie(string key) => this.Response.Cookies.Delete(key, this.cookieOptions);
        private string GetCookieValue(string key) => this.httpContextAccessor.HttpContext!.Request.Cookies[key]!;
    }
}
