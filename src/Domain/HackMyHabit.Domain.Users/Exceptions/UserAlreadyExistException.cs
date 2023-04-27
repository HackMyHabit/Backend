namespace HackMyHabit.Domain.Users.Exceptions
{
    public class UserAlreadyExistException : Exception
    {
        public UserAlreadyExistException() : base("User already exist")
        {
        }
    }
}
