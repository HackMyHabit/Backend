namespace HackMyHabit.Domain.Tasks.Entities
{
    public class Task
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        public bool IsCompleted { get; private set; }
        //TODO: to replace by User class as creator
        public Guid UserId { get; private set; }

        public Task(Guid id, string name, string description, DateTime dueDate, Guid userId, DateTime creationDate)
        {
            Id = id;
            Name = name;
            Description = description;
            DueDate = dueDate;
            IsCompleted = false;
            UserId = userId;
            CreationDate = creationDate;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }

        public void MarkAsIncomplete()
        {
            IsCompleted = false;
        }
    }
}
