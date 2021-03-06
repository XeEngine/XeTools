﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Tools.Modules
{
    public class ModuleInit
    {
        public Type Type { get; set; }

        public string FileName { get; set; }

        public string OutputFileName { get; set; }

        public string InputPath { get; set; }

        public string OutputPath { get; set; }

        public KeyValuePair<string, string>[] Parameters { get; set; }
    }
}
