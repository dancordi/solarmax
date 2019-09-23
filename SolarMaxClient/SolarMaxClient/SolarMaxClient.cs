using SolarMaxClient.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMaxClient
{
    public class SolarMaxClient : ISolarMaxClient
    {
        ITransport Transport;

        public SolarMaxClient(ITransport transport)
        {
            this.Transport = transport;
        }

        public bool GetVersion(out string version)
        {
            //connect
            if (this.Transport.Connect() == CommunicationResult.OK)
            {
                this.Transport.Send(null);
                this.Transport.Receive(out byte[] bytes);
                //disconnect
                this.Transport.Disconnect();
                version = null;
                return true;
            }
            version = null;
            return false;
        }
    }
}
