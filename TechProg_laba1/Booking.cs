using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TechProg_laba1
{
    internal class Booking
    {
        private Room room;
        private Client client;
        private DateTime checkIn;
        private bool earlyBooking;
        private BookingStatus status;
        public decimal totalCost;

        public Room Room
        {
            get { return room; }
            set
            {
                room = value;
            }
        }

        public Client Client
        {
            get { return  client; }
            set { client = value; }
        }

        private DateTime CheckIn
        {
            get { return  checkIn; }
            set
            {
                if 
            }
        }

    }
}
