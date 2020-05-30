using SolarMaxClient.Protocol;
using SolarMaxClient.Result;
using SolarMaxClient.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public GetEnergyReportResult GetEnergyReport()
        {
            GetEnergyReportResult energyReport = null;
            //connect
            if (this.Transport.Connect() == CommunicationResult.OK)
            {
                energyReport = new GetEnergyReportResult();
                var solarMaxRequest = new SolarMaxRequest(new SolarMaxCommand[] { new SolarMaxCommand(SolarMaxCommandEnum.P_AC) }.ToList());
                var resSend = this.Transport.Send(solarMaxRequest.ToString());
                if (resSend == CommunicationResult.OK)
                {
                    //receive
                    var resRec = this.Transport.Receive(out string rec);
                    if (resRec == CommunicationResult.OK)
                    {
                        SolarMaxResponse solarMaxResponse = new SolarMaxResponse(rec);
                        var listSolarMaxCommand = solarMaxResponse.Parse();
                        if (listSolarMaxCommand.Count > 0)
                        {
                            //
                            energyReport.PAC = int.Parse(listSolarMaxCommand[0].Value, System.Globalization.NumberStyles.HexNumber) / 2;
                            energyReport.Result = true;
                        }
                    }
                    else
                    {
                        energyReport.Result = false;
                        energyReport.ErrorDescription = "Unable to receive the response";
                    }
                }
                else
                {
                    energyReport.Result = false;
                    energyReport.ErrorDescription = "Unable to send the request";
                }
                //disconnect
                this.Transport.Disconnect();
                return energyReport;
            }
            return new GetEnergyReportResult() { Result = false, ErrorDescription = "Unable to connect" };
        }

        public GetStatusResult GetStatus()
        {
            GetStatusResult status = null;
            //connect
            if (this.Transport.Connect() == CommunicationResult.OK)
            {
                status = new GetStatusResult();
                var solarMaxRequest = new SolarMaxRequest(new SolarMaxCommand[] { new SolarMaxCommand(SolarMaxCommandEnum.DEVICE_STATUS) }.ToList());
                var resSend = this.Transport.Send(solarMaxRequest.ToString());
                if (resSend == CommunicationResult.OK)
                {
                    //receive
                    var resRec = this.Transport.Receive(out string rec);
                    if (resRec == CommunicationResult.OK)
                    {
                        SolarMaxResponse solarMaxResponse = new SolarMaxResponse(rec);
                        var listSolarMaxCommand = solarMaxResponse.Parse();
                        if (listSolarMaxCommand.Count > 0)
                        {
                            status.Status = listSolarMaxCommand[0].Value;
                            status.Result = true;
                        }
                    }
                    else
                    {
                        status.Result = false;
                        status.ErrorDescription = "Unable to receive the response";
                    }
                }
                else
                {
                    status.Result = false;
                    status.ErrorDescription = "Unable to send the request";
                }
                //disconnect
                this.Transport.Disconnect();
                return status;
            }
            return new GetStatusResult() { Result = false, ErrorDescription = "Unable to connect" };
        }

        bool Test()
        {
            string test = "{FB;01;3E|64:IDC;UL1;TKK;IL1;SYS;TNF;UDC;PAC;PRL;KT0;SYS|0F66}";
            byte[] send_bytes = Encoding.UTF8.GetBytes(test);
            //connect
            if (this.Transport.Connect() == CommunicationResult.OK)
            {
                var resSend = this.Transport.Send(send_bytes);
                if (resSend == CommunicationResult.OK)
                {
                    var resRec = this.Transport.Receive(out byte[] rec_bytes);
                    if (resRec == CommunicationResult.OK)
                    {
                        string rec_string = Encoding.UTF8.GetString(rec_bytes);

                    }
                }
                //disconnect
                this.Transport.Disconnect();
                return true;
            }
            return false;
        }
    }
}
