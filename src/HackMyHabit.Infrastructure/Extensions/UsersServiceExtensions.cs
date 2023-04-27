using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using HackMyHabit.Domain.Users.Commons;
using HackMyHabit.Domain.Users.Entities;
using HackMyHabit.Domain.Users.Repositories;
using HackMyHabit.Domain.Users.Validators;
using HackMyHabit.Infrastructure.Database.Repositories;

namespace HackMyHabit.Domain.Users.Extensions;

public static class UsersServiceExtensions
{
    public static IServiceCollection AddUsersServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IUsersValidator, UsersValidator>()
            .AddSingleton<IUsersRepository, UsersRepository>()
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IAuthManager, AuthManager>()
            .AddSingleton<IEncryptor, Encryptor>()
            .AddSingleton<IHasher, Hasher>()
            .AddSingleton<IRng, Rng>();
    }
}
