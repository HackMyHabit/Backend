using HackMyHabit.Domain.Commons.Abstractions;

namespace HackMyHabit.Domain.Tasks.Entities
{
    public class Task: Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime CreationDate { get; private set; }
        public bool IsCompleted { get; private set; }
        public Guid TaskListId { get; private set; }

        public Task(Guid id, string name, string description, DateTime dueDate, Guid userId, DateTime creationDate, Guid taskListId)
        {
            Id = id;
            Name = name;
            Description = description;
            DueDate = dueDate;
            IsCompleted = false;
            CreationDate = creationDate;
            TaskListId = taskListId;
        }

        public void Done()
        {
            IsCompleted = true;
        }

        public void Undone()
        {
            IsCompleted = false;
        }
    }
}
