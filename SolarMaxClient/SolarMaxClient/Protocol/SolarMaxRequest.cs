using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMaxClient.Protocol
{
    internal class SolarMaxRequest
    {
        public static int PREFIX_LENGTH = 13;
        public static int SUFFIX_LENGTH = 6;
        public static string MESSAGE_DELIMITER = ";";
        public static string MESSAGE_PART_DELIMITER = "|";
        public static string SOURCE = "FB";
        public static int DEFAULT_DESTINATION = 0;

        private static string ENCODING = "64:";

        private string _Request;

        public SolarMaxRequest(List<SolarMaxCommand> commands) : this(DEFAULT_DESTINATION, commands)
        {
        }

        public SolarMaxRequest(int destination, List<SolarMaxCommand> commands)
        {
            this._Request = CreateRequestMessage(destination, commands);
        }

        private string CreateRequestMessage(int destination, List<SolarMaxCommand> commands)
        {
            //{FB;01;3E|64:IDC;UL1;TKK;IL1;SYS;TNF;UDC;PAC;PRL;KT0;SYS|0F66}
            //get the body of the request
            StringBuilder sb = new StringBuilder();
            foreach (var command in commands)
            {
                sb.Append($"{command.Command.GetValueAsString()}{MESSAGE_DELIMITER}");
            }
            var body = sb.ToString();
            sb = null;

            //get the message
            int length = PREFIX_LENGTH + body.Length + SUFFIX_LENGTH;
            var sbMessage = new StringBuilder(length);
            // {(1) + Src(2) + ;(1) + dest(2) + ;(1) + len(2) + |(1) + enconding(3) + body + |(1) + checksum(4) + }(1)

            sbMessage.Append(SOURCE).Append(MESSAGE_DELIMITER)
                    .Append(destination.ToString().PadLeft(2, '0')).Append(MESSAGE_DELIMITER)
                    .Append(length.ToHexString().PadLeft(2, '0'))
                    .Append(MESSAGE_PART_DELIMITER)
                    .Append(ENCODING).Append(body)
                    .Append(MESSAGE_PART_DELIMITER);
            string message = sbMessage.ToString();
            sbMessage = null;

            //calculate the checksum of the message
            int checkSum = Helper.CalculateChecksum(message);

            //the request
            return "{" + message + checkSum.ToHexString().PadLeft(4, '0') + '}';
        }

        public override string ToString()
        {
            return this._Request;
        }

    }
}
