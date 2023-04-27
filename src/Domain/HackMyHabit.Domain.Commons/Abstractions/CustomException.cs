using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackMyHabit.Domain.Commons.Abstractions
{
    public abstract class CustomException: Exception
    {
        protected CustomException(string message) : base(message) 
        { 
        }
    }
}
