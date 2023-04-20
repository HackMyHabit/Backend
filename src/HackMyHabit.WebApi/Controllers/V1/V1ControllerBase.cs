using HackMyHabit.WebApi.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace HackMyHabit.WebApi.Controllers.V1
{
    [EnableCors("CORS_POLICY")]
    [ApiController]
    [Authorize(Policy = AuthConsts.SESSION_CHECK_POLICY)]
    [Route("/public/api/v1.0/[controller]/[action]")]
    public class V1ControllerBase : ControllerBase
    {
    }
}
