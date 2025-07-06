﻿using DelitaTrade.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class UserViewModel : IExceptionName
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public string? UserName { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
