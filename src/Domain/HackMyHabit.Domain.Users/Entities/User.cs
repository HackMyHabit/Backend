namespace HackMyHabit.Domain.Users.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string HashedPassword { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User(Guid id, string email, string hashedPassword, DateTime createdAt)
    {
        this.Id = id;
        this.Email = email;
        this.HashedPassword = hashedPassword;
        this.CreatedAt = createdAt;
    }

    public static User Create(Guid id, string email, string hashedPassword)
    {
        return new User(id, email, hashedPassword, DateTime.Now);
    }
}
