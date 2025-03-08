using System;
using System.Collections.Generic;

namespace SolarMaxUpdater.Settings
{
    public class AppSettings
    {
        public AzureFunctionsSettings AzureFunctions { get; set; }
        public List<InverterSettings> Inverters { get; set; }
    }
}
