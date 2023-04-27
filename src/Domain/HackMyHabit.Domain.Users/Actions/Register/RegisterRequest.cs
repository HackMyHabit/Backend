using MediatR;

namespace HackMyHabit.Domain.Users.Actions.Register
{
    public record RegisterRequest(string Email, string Password) : IRequest;
}
