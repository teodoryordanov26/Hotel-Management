using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement
{
    internal class HotelManagement
    {

        private const string FilePath = "rooms.txt";
        private List<Room> rooms;

        public HotelManagement()
        {
            rooms = LoadRoomsFromFile();
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("1. Резервиране на стая");
                Console.WriteLine("2. Освобождаване на стая");
                Console.WriteLine("3. Проверка на наличността и цените на стаите");
                Console.WriteLine("4. Справка за заетите стаи и гостите");
                Console.WriteLine("5. Изход");
                Console.Write("Изберете опция: ");
                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        ReserveRoom();
                        break;
                    case "2":
                        ReleaseRoom();
                        break;
                    case "3":
                        DisplayAvailableRooms();
                        break;
                    case "4":
                        DisplayOccupiedRooms();
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Невалиден избор. Моля, опитайте отново.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНатиснете въпросния клавиш, за да продължите...");
                    Console.ReadKey();
                }
            }
        }

        private List<Room> LoadRoomsFromFile()
        {
            List<Room> loadedRooms = new List<Room>();

            if (!File.Exists(FilePath))
                return loadedRooms;

            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length == 6)
                    {
                        int roomNumber;
                        int.TryParse(parts[0], out roomNumber);

                        string type = parts[1];

                        int capacity;
                        int.TryParse(parts[2], out capacity);

                        decimal pricePerNight;
                        decimal.TryParse(parts[3], out pricePerNight);

                        bool occupied;
                        bool.TryParse(parts[4], out occupied);

                        string guestName = parts[5];

                        Room room = new Room(roomNumber, type, capacity, pricePerNight, occupied, guestName);
                        loadedRooms.Add(room);
                    }
                }
            }

            return loadedRooms;
        }

        private void SaveRoomsToFile()
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                foreach (Room room in rooms)
                {
                    string line = $"{room.RoomNumber};{room.Type};{room.Capacity};{room.PricePerNight};{room.Occupied};{room.GuestName}";
                    writer.WriteLine(line);
                }
            }
        }

        private void ReserveRoom()
        {
            Console.WriteLine("Резервиране на стая:");
            Console.Write("Въведете номер на стаята: ");
            string roomNumberInput = Console.ReadLine();

            int roomNumber;
            if (!int.TryParse(roomNumberInput, out roomNumber))
            {
                Console.WriteLine("Невалиден номер на стая. Операцията е прекратена.");
                return;
            }

            Room room = rooms.Find(r => r.RoomNumber == roomNumber);
            if (room == null)
            {
                Console.WriteLine("Стая с такъв номер не съществува. Операцията е прекратена.");
                return;
            }

            if (room.Occupied)
            {
                Console.WriteLine("Стаята е вече заета от гост. Операцията е прекратена.");
                return;
            }

            Console.Write("Въведете тип на стаята: ");
            string type = Console.ReadLine();

            Console.Write("Въведете капацитет на стаята: ");
            string capacityInput = Console.ReadLine();

            int capacity;
            if (!int.TryParse(capacityInput, out capacity))
            {
                Console.WriteLine("Невалиден капацитет на стаята. Операцията е прекратена.");
                return;
            }

            Console.Write("Въведете цена за наемане на стаята за една нощ: ");
            string priceInput = Console.ReadLine();

            decimal pricePerNight;
            if (!decimal.TryParse(priceInput, out pricePerNight))
            {
                Console.WriteLine("Невалидна цена за наемане на стаята. Операцията е прекратена.");
                return;
            }

            Console.Write("Въведете име на госта: ");
            string guestName = Console.ReadLine();

            room.Type = type;
            room.Capacity = capacity;
            room.PricePerNight = pricePerNight;
            room.Occupied = true;
            room.GuestName = guestName;

            SaveRoomsToFile();
            Console.WriteLine("Стаята е успешно резервирана.");
        }

        private void ReleaseRoom()
        {
            Console.WriteLine("Освобождаване на стая:");
            Console.Write("Въведете номер на стаята: ");
            string roomNumberInput = Console.ReadLine();

            int roomNumber;
            if (!int.TryParse(roomNumberInput, out roomNumber))
            {
                Console.WriteLine("Невалиден номер на стая. Операцията е прекратена.");
                return;
            }

            Room room = rooms.Find(r => r.RoomNumber == roomNumber);
            if (room == null)
            {
                Console.WriteLine("Стая с такъв номер не съществува. Операцията е прекратена.");
                return;
            }

            if (!room.Occupied)
            {
                Console.WriteLine("Стаята е вече свободна. Операцията е прекратена.");
                return;
            }

            room.Occupied = false;
            room.GuestName = "";

            SaveRoomsToFile();
            Console.WriteLine("Стаята е успешно освободена.");
        }

        private void DisplayAvailableRooms()
        {
            Console.WriteLine("Списък с налични стаи и цени за наемане:");
            foreach (Room room in rooms)
            {
                if (!room.Occupied)
                {
                    Console.WriteLine($"Номер на стая: {room.RoomNumber}");
                    Console.WriteLine($"Тип: {room.Type}");
                    Console.WriteLine($"Капацитет: {room.Capacity}");
                    Console.WriteLine($"Цена за наемане на стаята за една нощ: {room.PricePerNight}");
                    Console.WriteLine();
                }
            }
        }

        private void DisplayOccupiedRooms()
        {
            Console.WriteLine("Списък със заетите стаи:");
            bool occupiedRoomsExist = false;
            foreach (Room room in rooms)
            {
                if (room.Occupied)
                {
                    Console.WriteLine($"Номер на стая: {room.RoomNumber}");
                    Console.WriteLine($"Тип: {room.Type}");
                    Console.WriteLine($"Капацитет: {room.Capacity}");
                    Console.WriteLine($"Цена за наемане на стаята за една нощ: {room.PricePerNight}");
                    Console.WriteLine($"Гост: {room.GuestName}");
                    Console.WriteLine();
                    occupiedRoomsExist = true;
                }
            }

            if (!occupiedRoomsExist)
            {
                Console.WriteLine("Няма заети стаи в момента.");
            }
        }
    }
}
