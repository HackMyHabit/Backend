namespace HackMyHabit.Domain.Commons.Abstractions;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}
