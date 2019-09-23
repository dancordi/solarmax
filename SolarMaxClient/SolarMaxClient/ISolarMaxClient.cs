using System;

namespace SolarMaxClient
{
    public interface ISolarMaxClient
    {
        bool GetVersion(out string version);

    }
}
