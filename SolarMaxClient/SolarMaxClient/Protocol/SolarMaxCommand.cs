using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMaxClient.Protocol
{
    internal class SolarMaxCommand
    {
        public string Value { get; set; }
        public SolarMaxCommandEnum Command { get; private set; }
        public SolarMaxCommand(SolarMaxCommandEnum commandEnum)
        {
            this.Command = commandEnum;
        }

    }
}
