using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechProg_laba1
{
    internal class RoomType
    {
        private string name;
        private decimal pricePerNight;
        private int capacity;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ошибка: тип комнаты не может быть пустым");
                }
                name = value;
            }
        }

        public decimal PricePerNight
        {
            get { return pricePerNight; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Ошибка: неверный формат цены");
                }
                pricePerNight = value;
            }
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Ошибка: неверный формат вместимости номера");
                }
                capacity = value;
            }
        }
    }
}
