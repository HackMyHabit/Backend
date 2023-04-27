
using HackMyHabit.Domain.Users.Entities;

namespace HackMyHabit.Domain.Users.Repositories
{
    public interface IUsersRepository
    {
        Task<User> FindOrThrow(string email);
        Task Add(User user);
        Task<User?> Get(Guid id);
        Task<User?> Get(string email);
    }
}
