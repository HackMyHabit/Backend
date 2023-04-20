using HackMyHabit.Domain.Commons.Abstractions;

namespace HackMyHabit.WebApi.Commons
{
    public record UserSession(Guid UserId, string Token, DateTime CreatedAt, string IpAddress, string UserAgent);

    public interface IUsersSessionsStorage
    {
        void Add(UserSession session);
        UserSession? Get(string token);
        void RemoveByToken(string token);
    }

    public class UsersSessionsStorage : IUsersSessionsStorage
    {
        private readonly ICacheService cacheService;

        public UsersSessionsStorage(ICacheService cacheService)
        {
            this.cacheService = cacheService;
        }

        public void Add(UserSession session) => cacheService.SaveValue(session.Token, session);
        public UserSession? Get(string token) => this.cacheService.GetValue<UserSession>(token);
        public void RemoveByToken(string token)
        {
            cacheService.RemoveValue(token);
        }
    }
}
