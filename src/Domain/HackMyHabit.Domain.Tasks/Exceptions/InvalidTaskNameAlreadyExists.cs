using HackMyHabit.Domain.Commons.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackMyHabit.Domain.Tasks.Exceptions
{
    internal class InvalidTaskNameAlreadyExists : CustomException
    {
        public InvalidTaskNameAlreadyExists(string taskName) : 
            base($"The task with the given name({taskName}) already exists")
        {
        }
    }
}
