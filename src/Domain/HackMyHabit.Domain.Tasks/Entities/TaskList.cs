using HackMyHabit.Domain.Commons.Abstractions;
using HackMyHabit.Domain.Tasks.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackMyHabit.Domain.Tasks.Entities
{
    public class TaskList: BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        //TODO: to replace by User class as owner
        public Guid UserId { get; private set; }
        public IEnumerable<Task> Tasks => _tasks;

        private readonly HashSet<Task> _tasks = new HashSet<Task>();

        private TaskList() { }

        private TaskList(Guid id, string name, Guid userId)
        {
            Id= id;
            Name = name;
            UserId = userId;
        }

        public static TaskList Create(string name, Guid userId) => new(Guid.NewGuid(), name, userId);

        public void AddTask(Task task) 
        { 
            var currentDate = DateTime.Now;
            var isInvalidDateTime = task.DueDate <= currentDate;
            if (isInvalidDateTime)
            {
                throw new InvalidTaskDueDateException(task.DueDate, currentDate);
            }
            if(_tasks.Any(x => x.Name== task.Name))
            {
                throw new InvalidTaskNameAlreadyExists(task.Name);
            }

            _tasks.Add(task);

        }

        public void RemoveTask(Guid taskId) 
        { 
            _tasks.RemoveWhere(x => x.Id == taskId);
        }

        public void MarkTaskAsCompleted(Guid taskId) 
        { 
            _tasks.Where(x => x.Id == taskId).Single().MarkAsCompleted();
        }

        public void MarkTaskAsIncompleted(Guid taskId)
        {
            _tasks.Where(x => x.Id == taskId).Single().MarkAsIncomplete();
        }

        public void MarkTasksCompleted(IEnumerable<Guid> taskIds)
        {
            foreach (var task in _tasks.Where(x => taskIds.Contains(x.Id))) 
            {
                task.MarkAsCompleted();
            }
        }
    }
}
