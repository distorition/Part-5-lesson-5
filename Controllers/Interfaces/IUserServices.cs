﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Part_3_Lesson_4.Controllers.Interfaces
{
 public    interface IUserServices
    {
            string Authentication(string user, string password);
    }
}
