﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackMyHabit.Domain.Users.Actions.Login
{
    public sealed record LoginRequest(string Email, string Password) : IRequest<LoginResponse>;
}
