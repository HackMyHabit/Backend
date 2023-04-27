using MediatR;
using HackMyHabit.Domain.Users.Commons;
using Microsoft.AspNetCore.Identity;
using HackMyHabit.Domain.Users.Entities;
using HackMyHabit.Domain.Users.Exceptions;
using HackMyHabit.Domain.Users.Repositories;
using HackMyHabit.Domain.Users.Validators;

namespace HackMyHabit.Domain.Users.Actions.Login
{
    public sealed class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IAuthManager authManager;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IUsersRepository usersRepository;
        private readonly IUsersValidator usersValidator;

        public LoginRequestHandler(IAuthManager authManager, IPasswordHasher<User> passwordHasher, IUsersRepository usersRepository, IUsersValidator usersValidator)
        {
            this.authManager = authManager;
            this.passwordHasher = passwordHasher;
            this.usersRepository = usersRepository;
            this.usersValidator = usersValidator;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            this.usersValidator.ValidateEmail(request.Email);
            this.usersValidator.ValidatePassword(request.Password);

            var user = await this.usersRepository.Get(request.Email.ToLowerInvariant());
            if(user is null)
            {
                throw new UserNotFoundException();
            }

            if (this.passwordHasher.VerifyHashedPassword(default!, user.HashedPassword, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialsException();
            }

            var jwt = this.authManager.CreateToken(user, "user");

            return new(user.Id, jwt.AccessToken);
        }
    }
}
