﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    class NotFoundException : Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
