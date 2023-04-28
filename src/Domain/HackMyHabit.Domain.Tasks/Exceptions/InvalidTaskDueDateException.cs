using HackMyHabit.Domain.Commons.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackMyHabit.Domain.Tasks.Exceptions
{
    internal class InvalidTaskDueDateException : CustomException
    {
        public InvalidTaskDueDateException(DateTime dueDate, DateTime now) : 
            base($"It is not possible to set a due date {dueDate.ToShortDateString()} earlier than the current date {now.ToShortDateString}")
        {
        }
    }
}
