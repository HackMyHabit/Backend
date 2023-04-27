namespace HackMyHabit.Domain.Users.Actions.Login
{
    public sealed record LoginResponse(Guid UserId, string Token);
}
