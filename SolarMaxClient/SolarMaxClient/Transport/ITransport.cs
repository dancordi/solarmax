using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMaxClient.Transport
{
    public interface ITransport : IDisposable
    {
        CommunicationResult Connect();
        CommunicationResult Disconnect();
        CommunicationResult Send(byte[] bytes);
        CommunicationResult Receive(out byte[] bytes);
    }
}
