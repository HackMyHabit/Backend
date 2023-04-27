public class JsonWebToken
{
    public string AccessToken { get; init; }
    public long Expiry { get; init; }
    public Guid UserId { get; init; }
    public string Role { get; init; }
    public string Email { get; init; }
    public IDictionary<string, IEnumerable<string>> Claims { get; init; }
}