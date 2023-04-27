using HackMyHabit.Domain.Users.Entities;
using HackMyHabit.Domain.Users.Repositories;

namespace HackMyHabit.Infrastructure.Database.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private List<User> _users = new List<User>();

        public async Task Add(User user)
        {
            await Task.CompletedTask;
            _users.Add(user);
        }

        public async Task<User> FindOrThrow(string email)
        {
            await Task.CompletedTask;
            return _users.FirstOrDefault(x => x.Email == email) ?? throw new ArgumentException();
        }

        public async Task<User?> Get(Guid id)
        {
            await Task.CompletedTask;
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<User?> Get(string email)
        {
            await Task.CompletedTask;
            return _users.FirstOrDefault(x => x.Email == email);
        }
    }
}
