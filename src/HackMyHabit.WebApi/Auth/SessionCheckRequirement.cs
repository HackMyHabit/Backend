using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Permissions;

namespace HackMyHabit.WebApi.Auth
{
    public class SessionCheckRequirement : IAuthorizationRequirement
    {

    }
}
