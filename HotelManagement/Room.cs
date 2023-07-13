using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement
{
    internal class Room
    {
        private int roomNumber;
        private string type;
        private int capacity;
        private decimal pricePerNight;
        private bool occupied;
        private string guestName;

        public Room(int roomNumber, string type, int capacity, decimal pricePerNight, bool occupied, string guestName)
        {
            this.roomNumber = roomNumber;
            this.type = type;
            this.capacity = capacity;
            this.pricePerNight = pricePerNight;
            this.occupied = occupied;
            this.guestName = guestName;
        }

        public int RoomNumber { get => roomNumber; set => roomNumber = value; }
        public string Type { get => type; set => type = value; }
        public int Capacity { get => capacity; set => capacity = value; }
        public decimal PricePerNight { get => pricePerNight; set => pricePerNight = value; }
        public bool Occupied { get => occupied; set => occupied = value; }
        public string GuestName { get => guestName; set => guestName = value; }

    }
}
