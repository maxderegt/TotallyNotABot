﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TotallyNotABot.src.commands
{
    class BaseCommand
    {
        public string name;

        protected BaseCommand(string name) { this.name = name; }

        public virtual string Help()
        {
            return "No help function implemented yet";
        }

    }
}
