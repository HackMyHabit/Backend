using HackMyHabit.Domain.Commons.Abstractions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HackMyHabit.Infrastructure.Caching;

public class CacheService : ICacheService
{
    private readonly IDatabase database;

    public CacheService(IDatabase database)
    {
        this.database = database;
    }

    public T? GetValue<T>(string key) where T : class
    {
        var value = this.database.StringGet(key);
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        return JsonConvert.DeserializeObject<T>(value);
    }

    public void SaveValue<T>(string key, T value) where T : class
    {
        if (value == null)
        {
            return;
        }
        var serialized = JsonConvert.SerializeObject(value);
        this.database.StringSet(key, serialized);
    }

    public void RemoveValue(string key)
    {
        this.database.KeyDelete(key);
    }
}