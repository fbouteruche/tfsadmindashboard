﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsAdminDashboardConsole.Commands
{
    public interface iCommand
    {
        void Execute(string outFormat);
    }
}
