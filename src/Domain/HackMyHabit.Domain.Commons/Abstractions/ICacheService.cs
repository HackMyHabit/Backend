namespace HackMyHabit.Domain.Commons.Abstractions;

public interface ICacheService
{
    T? GetValue<T>(string key) where T : class;
    void SaveValue<T>(string key, T value) where T : class;
    void RemoveValue(string key);
}