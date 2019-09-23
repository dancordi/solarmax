using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMaxClient.Transport
{
    public enum CommunicationResult
    {
        OK,
        KO_Socket_Not_Initialized,
        KO_Socket_Not_Connected,
        KO_Socket_Connection_Timeout,
        KO_Socket_Send_Timout,
        KO_Socket_Receive_Timeout,
        KO_Socket_Receive_Incomplete,
        KO_Socket_Exception,
        KO_Exception
    }
}
