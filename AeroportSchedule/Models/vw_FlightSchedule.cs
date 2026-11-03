using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroportSchedule.Models
{
    public class vw_FlightSchedule
    {
        public int FlightID { get; set; }
        public string FlightNumber { get; set; }
        public string AirlineName { get; set; }
        public string DepartureCode { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCode { get; set; }
        public string ArrivalCity { get; set; }
        public System.DateTime DepartureTime { get; set; }
        public System.DateTime ArrivalTime { get; set; }
        public int DurationMin { get; set; }
        public string AircraftReg { get; set; }
        public string AircraftModel { get; set; }
        public string Status { get; set; }
        public string GateNumber { get; set; }
    }
}
