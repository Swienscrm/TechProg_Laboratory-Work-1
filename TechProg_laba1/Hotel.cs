using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechProg_laba1
{
    internal class Hotel
    {
        private string name;
        private int star;
        private bool seasonal;
        private List<Room> rooms = new List<Room>();

        public string Name
        {
            get { return name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Название гостиницы не может быть пустым");
                }
                name = value;
            }
        }

        public int Star
        {
            get { return star; }
            set
            {
                if (value < 1 || value > 5) { throw new ArgumentException("Категория гостиницы должна быть от 1 до 5 звезд");}
                star = value;
            }

        }

        public bool Seasonal
        {
            get { return seasonal; }
            set { seasonal = value; }
        }

        public List<Room> Rooms
        {
            get { return rooms; }
        }

        public void AddRoom(Room roomName)
        {
            if (rooms.Any(existingRoom => existingRoom.RoomNumber == roomName.RoomNumber)) { Console.WriteLine("Ошибка: данная комната уже добавлена"); return; }
            try
            {
                roomName.AssignHotel(this);
            }

            catch(ArgumentException ex)
            {
                Console.WriteLine("Ошибка" + ex.Message);
                return;
            }

            rooms.Add(roomName);
        }



    }
}
