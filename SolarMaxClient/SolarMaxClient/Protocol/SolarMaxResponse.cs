using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMaxClient.Protocol
{
    internal class SolarMaxResponse
    {
        private string fullResponse;

        public SolarMaxResponse(string response)
        {
            if (response.StartsWith("{"))
            {
                response = response.Remove(0, 1);
            }
            if (response.EndsWith("}"))
            {
                response = response.Remove(response.Length - 1, 1);
            }
            this.fullResponse = response;
        }

        public List<SolarMaxCommand> Parse()
        {
            // example
            //{01;FB;70|64:IDC=407;UL1=A01;TKK=2C;IL1=46D;SYS=4E28,0;TNF=1383;UDC=B7D;PAC=16A6;PRL=2B;KT0=48C;SYS=4E28,0|1A5F}
            List<SolarMaxCommand> solarMaxCommands = new List<SolarMaxCommand>();
            int idx = fullResponse.IndexOf(SolarMaxRequest.MESSAGE_PART_DELIMITER);
            idx += 4;
            int lastIdx = fullResponse.LastIndexOf(SolarMaxRequest.MESSAGE_PART_DELIMITER);
            string messageBody = fullResponse.Substring(idx, lastIdx-idx);

            string[] commandsResponses = messageBody.Split(SolarMaxRequest.MESSAGE_DELIMITER);
            foreach (var commandsResponse in commandsResponses)
            {
                string[] keyValue = commandsResponse.Split("=");
                if (keyValue.Length != 2)
                {
                    //not valid
                    continue;
                }
                SolarMaxCommandEnum s = keyValue[0].GetEnumValueFromDescription<SolarMaxCommandEnum>();
                string valueHex = keyValue[1];
                solarMaxCommands.Add(new SolarMaxCommand(s) { Value = valueHex });
            }
            return solarMaxCommands;
        }
    }
}
