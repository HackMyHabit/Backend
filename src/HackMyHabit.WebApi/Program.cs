using HackMyHabit.WebApi.Consts;
using HackMyHabit.WebApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

var corsName = "CORS_POLICY";

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

//todo configure different policy for prod env
services.AddCors(options =>
options.AddPolicy(corsName, x => x.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

services
    .SetupAuth(builder.Configuration)
    .SetupModules(builder.Configuration)
    .SetupMediatr();

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(corsName);
app.UseHttpsRedirection();
app.UseAuthorization();
app.Use(async (ctx, next) =>
{
    if (ctx.Request.Headers.ContainsKey(AuthConsts.AUTHORIZATION_HEADER))
    {
        ctx.Request.Headers.Remove(AuthConsts.AUTHORIZATION_HEADER);
    }

    if (ctx.Request.Cookies.ContainsKey(AuthConsts.ACCESS_TOKEN_COOKIE))
    {
        var authenticateResult = await ctx.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        if (authenticateResult.Succeeded && authenticateResult.Principal is not null)
        {
            ctx.User = authenticateResult.Principal;
        }
    }

    await next();
});
app.MapControllers();

app.Run();