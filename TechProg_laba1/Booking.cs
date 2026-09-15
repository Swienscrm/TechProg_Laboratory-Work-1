using System;
using System.Collections.Generic;
using System.Linq;

namespace TechProg_laba1
{
    internal class Booking
    {
        private Room room;
        private Client client;
        private DateTime checkInDate;
        private DateTime checkOutDate;
        private int guestCount;
        private BookingStatus status;
        private decimal totalCost;
        private int earlyBookingDaysTreshold = 30;

        public Room Room
        {
            get { return room; }
            private set { room = value; }
        }

        public Client Client
        {
            get { return client; }
            private set { client = value; }
        }

        public DateTime CheckInDate
        {
            get { return checkInDate; }
            private set
            {
                if (value.Date < DateTime.Today)
                {
                    throw new ArgumentException("Ошибка: дата не может быть указана задним числом");
                }

                checkInDate = value;
            }
        }

        public DateTime CheckOutDate
        {
            get { return checkOutDate; }
            private set
            {
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

        public int GuestCount
        {
            get { return guestCount; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Ошибка: неверное кол-во гостей");
                }

                guestCount = value;
            }
        }

        public BookingStatus Status
        {
            get { return status; }
        }

        public decimal TotalCost
        {
            get { return totalCost; }
        }

        public Booking(Room room, Client client, DateTime checkInDate, DateTime checkOutDate, int guestCount)
        {
            if (room == null)
            {
                throw new ArgumentException("Ошибка: номер не может быть null");
            }

            if (client == null)
            {
                throw new ArgumentException("Ошибка: клиент не может быть null");
            }

            Room = room;
            Client = client;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            GuestCount = guestCount;
            status = BookingStatus.Created;
        }

        public bool TryChangeStatus(BookingStatus newStatus)
        {
            if ((status == BookingStatus.Created) && (newStatus == BookingStatus.CheckedIn))
            {
                status = newStatus;
                return true;
            }

            if ((status == BookingStatus.Created) && (newStatus == BookingStatus.Cancelled))
            {
                status = newStatus;
                return true;
            }

            if ((status == BookingStatus.CheckedIn) && (newStatus == BookingStatus.CheckedOut))
            {
                status = newStatus;
                return true;
            }
            else
            {
                Console.WriteLine($"Ошибка: невозможный переход статуса {status} в {newStatus} бронирования");
                return false;
            }
        }

        public int GetNightCount()
        {
            int nightCount = (CheckOutDate - CheckInDate).Days;
            return nightCount;
        }

        public bool IsEarlyBooking()
        {
            int earlyBookingDays = (CheckInDate - DateTime.Today).Days;
            if (earlyBookingDays > earlyBookingDaysTreshold)
            {
                return true;
            }

            return false;
        }

        public decimal CalculateCost()
        {
            if (Room.Hotel == null)
            {
                throw new ArgumentException("Ошибка: номер еще не привязан к гостинице");
            }

            decimal cost = GetNightCount() * Room.RoomType.PricePerNight;

            if (IsEarlyBooking())
            {
                cost = cost * 0.9m;
            }

            if (Room.Hotel.Star >= 4)
            {
                cost = cost * 1.15m;
            }

            if (Room.Hotel.Seasonal)
            {
                cost = cost * 1.25m;
            }

            int extraGuest = GuestCount - Room.RoomType.Capacity;
            if (extraGuest > 0)
            {
                cost = cost * (1m + 0.2m * extraGuest);
            }

            if (Client.IsLoyaltyMember)
            {
                cost = cost * 0.95m;
            }

            totalCost = cost;
            return totalCost;
        }
        
        
        
    }
}