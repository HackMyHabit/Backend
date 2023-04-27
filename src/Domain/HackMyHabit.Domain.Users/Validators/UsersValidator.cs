using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HackMyHabit.Domain.Users.Validators
{
    public interface IUsersValidator
    {
        void ValidateEmail(string email);
        void ValidatePassword(string password);
    }

    public class UsersValidator : IUsersValidator
    {
        private Regex exEmail = new Regex(@"\S+\@\S+\.\S+");

        public void ValidateEmail(string email)
        {
            if (!this.exEmail.IsMatch(email))
            {
                throw new ArgumentException("Email is invalid");
            }
        }

        public void ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                throw new ArgumentException("Password is shorter than 8");
            }
            if (password.All(x => !char.IsDigit(x)))
            {
                throw new ArgumentException("Password has no numbers");
            }
            if (password.All(x => !char.IsLetter(x)))
            {
                throw new ArgumentException("Password has no letters");
            }
        }
    }
}
