﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Exceptions
{
    public class AlreadyRunning : Exception
    {
        public AlreadyRunning(string message) : base(message)
        { }
    }
}