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
        private DateTime checkInDate;
        private DateTime checkOutDate;
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
            get { return client; }
            set { client = value; }
        }

        public DateTime CheckInDate
        {
            get { return checkInDate; }
            set
            {
                if (value.Date == default || value.Date == DateTime.MinValue)
                {
                    throw new ArgumentException("Ошибка: указан неверный формат даты");
                }
                if (value.Date < DateTime.Today)
                {
                    throw new ArgumentException("Ошибка: дата не может быть указана задним числом");
                }
                if (value.Date >= checkOutDate)
                {
                    throw new ArgumentException("Ошибка: дата выезда не может быть раньше чем дата заезда");
                }
                checkInDate = value;
            }
        }

        public DateTime CheckOutDate
        {
            get { return checkOutDate; }
            set
            {
                if (value.Date == default || value.Date == DateTime.MinValue)
                {
                    throw new ArgumentException("Ошибка: указан неверный формат даты");
                }
                if (value.Date < DateTime.Today)
                {
                    throw new ArgumentException("Ошибка: дата не может быть указана задним числом");
                }
                if (value.Date <= checkInDate)
                {
                    throw new ArgumentException("Ошибка: дата выезда не может быть раньше чем дата заезда");
                }
                checkOutDate = value;
            }
        }


    }
}
