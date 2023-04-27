using MediatR;
using Microsoft.AspNetCore.Identity;
using HackMyHabit.Domain.Users.Entities;
using HackMyHabit.Domain.Users.Exceptions;
using HackMyHabit.Domain.Users.Repositories;
using HackMyHabit.Domain.Users.Validators;

namespace HackMyHabit.Domain.Users.Actions.Register
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest>
    {
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IUsersRepository usersRepository;
        private readonly IUsersValidator usersValidator;

        public RegisterRequestHandler(IPasswordHasher<User> passwordHasher, IUsersRepository usersRepository, IUsersValidator usersValidator)
        {
            this.passwordHasher = passwordHasher;
            this.usersRepository = usersRepository;
            this.usersValidator = usersValidator;
        }

        public async Task<Unit> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            this.usersValidator.ValidateEmail(request.Email);
            this.usersValidator.ValidatePassword(request.Password);

            var email = request.Email.ToLowerInvariant();
            var user = await this.usersRepository.Get(email);
            if (user is not null)
            {
                throw new UserAlreadyExistException();
            }

            var hashedPassword = this.passwordHasher.HashPassword(default!, request.Password);
            user = User.Create(id: Guid.NewGuid(), email, hashedPassword);
            await this.usersRepository.Add(user);
            return Unit.Value;
        }
    }
}
