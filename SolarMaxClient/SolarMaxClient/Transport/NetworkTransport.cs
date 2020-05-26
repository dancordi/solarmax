using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SolarMaxClient.Transport
{
    public class NetworkTransport : ITransport
    {
        private Socket _socket;
        public string IpAddess { get; set; }
        public int TcpPort { get; set; }
        public TimeSpan ConnectTimeout { get; set; }
        public TimeSpan SendTimeout { get; set; }
        public TimeSpan ReceiveTimeout { get; set; }
        public Encoding Encoding { get; set; }

        public NetworkTransport(string ipAddess, int tcpPort,
                               TimeSpan connectTimeout,
                               TimeSpan sendTimeout,
                               TimeSpan receiveTimeout,
                               Encoding encoding)
        {
            this.IpAddess = ipAddess;
            this.TcpPort = tcpPort;
            this.ConnectTimeout = connectTimeout;
            this.SendTimeout = sendTimeout;
            this.ReceiveTimeout = receiveTimeout;
            this.Encoding = encoding;
        }

        public void Dispose()
        {
            try
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket.Dispose();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                _socket = null;
            }
        }

        public CommunicationResult Connect()
        {
            try
            {
                // Establish the remote endpoint for the socket.            
                IPEndPoint remoteEP = new IPEndPoint(System.Net.IPAddress.Parse(this.IpAddess), this.TcpPort);

                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    //
                    // Connect the socket to the remote endpoint with timeout
                    //
                    var result = _socket.BeginConnect(remoteEP, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne((int)this.ConnectTimeout.TotalMilliseconds, true);
                    if (_socket.Connected)
                    {
                        _socket.EndConnect(result);
                        return CommunicationResult.OK;
                    }
                    else
                    {
                        return CommunicationResult.KO_Socket_Connection_Timeout;
                    }
                }
                catch (SocketException se)
                {
                    return CommunicationResult.KO_Socket_Exception;
                }
                catch (Exception e)
                {
                    return CommunicationResult.KO_Exception;
                }
            }
            catch (Exception e)
            {
                return CommunicationResult.KO_Exception;
            }
        }

        public CommunicationResult Disconnect()
        {
            if (_socket == null)
            {
                return CommunicationResult.OK;
            }
            if (_socket.Connected)
            {
                // Release the socket.
                _socket.Shutdown(SocketShutdown.Both);
            }
            Dispose();
            return CommunicationResult.OK;
        }

        public CommunicationResult Send(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentException("message cannot be null");
            }

            //Send the bytes
            try
            {
                if (_socket == null)
                {
                    return CommunicationResult.KO_Socket_Not_Initialized;
                }
                if (!_socket.Connected)
                {
                    return CommunicationResult.KO_Socket_Not_Connected;
                }

                //
                // SEND THE MESSAGE
                //
                var resultSend = _socket.BeginSend(bytes, 0, bytes.Length, 0, null, null);
                bool successSend = resultSend.AsyncWaitHandle.WaitOne((int)this.SendTimeout.TotalMilliseconds, true);
                if (successSend)
                {
                    int bytesSent = _socket.EndSend(resultSend);
                    return CommunicationResult.OK;
                }
                else
                {
                    return CommunicationResult.KO_Socket_Send_Timout;
                }
            }
            catch (SocketException se)
            {
                return CommunicationResult.KO_Socket_Exception;
            }
            catch (Exception e)
            {
                return CommunicationResult.KO_Exception;
            }
        }

        public CommunicationResult Receive(out byte[] bytes)
        {
            bytes = null;
            try
            {
                if (_socket == null)
                {
                    return CommunicationResult.KO_Socket_Not_Initialized;
                }
                if (!_socket.Connected)
                {
                    return CommunicationResult.KO_Socket_Not_Connected;
                }
                //
                // RECEIVE THE RESPONSE
                //
                byte[] responseBuffer = new byte[_socket.ReceiveBufferSize];
                var resultReceive = _socket.BeginReceive(responseBuffer, 0, responseBuffer.Length, 0, null, null);
                bool successReceive = resultReceive.AsyncWaitHandle.WaitOne((int)this.ReceiveTimeout.TotalMilliseconds, true);

                if (successReceive)
                {
                    int bytesRec = _socket.EndReceive(resultReceive);
                    if (bytesRec > 0)
                    {
                        //copy the response
                        bytes = new byte[bytesRec];
                        Array.Copy(responseBuffer, 0, bytes, 0, bytesRec);
                        return CommunicationResult.OK;
                    }
                    else
                    {
                        //timeout
                        return CommunicationResult.KO_Socket_Receive_Timeout;
                    }
                }
                else
                {
                    return CommunicationResult.KO_Socket_Receive_Timeout;
                }
            }
            catch (SocketException se)
            {
                return CommunicationResult.KO_Socket_Exception;
            }
            catch (Exception e)
            {
                return CommunicationResult.KO_Exception;
            }
        }

        public CommunicationResult Send(string message)
        {
            byte[] messageBytes = Encoding.GetBytes(message);
            return Send(messageBytes);
        }

        public CommunicationResult Receive(out string message)
        {
            message = null;
            var res = Receive(out byte[] rec);
            if (res == CommunicationResult.OK)
            {
                message = Encoding.GetString(rec);
            }
            return res;
        }
    }
}
