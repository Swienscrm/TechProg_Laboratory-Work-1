using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechProg_laba1
{
    internal class Room
    {
        private string roomNumber;
        private Hotel hotel;
        private RoomType roomType;
        private bool status;

        public string RoomNumber
        {
            get { return roomNumber; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ошибка: номер комнаты не может быть пустым");
                }
                roomNumber = value;
            }
        }

        public Hotel Hotel
        {
            get { return hotel; }
            private set
            {

                hotel = value;
            }
        }

        public RoomType RoomType
        {
            get { return roomType; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Ошибка: объект RoomType - не может быть null");
                }
                roomType = value;
            }
        }

        public bool IsOccupied
        {
            get { return status; }
            set { status = value; }
        }

        public Room(string roomNumber, RoomType roomType)
        {
            RoomNumber = roomNumber;
            RoomType = roomType;
            IsOccupied = false;
        }

        public void AssignHotel(Hotel hotel)
        {
            if (hotel == null)
            {
                throw new ArgumentException("Гостиница не может быть null");

            }
            if (this.hotel != null)
            {
                throw new ArgumentException("Гостиница уже назначена");
            }
            Hotel = hotel;
        }
    }
}
