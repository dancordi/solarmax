using System;
using System.Collections.Generic;
using System.Text;

namespace SolarMaxRESTApiClient.Models
{
    public class SolarPacItem
    {
        public string id { get; set; }


        public int inverterId { get; set; }

        public DateTime timeStamp { get; set; }

        public double pac { get; set; }

        public SolarPacItem()
        {

        }
    }
}
