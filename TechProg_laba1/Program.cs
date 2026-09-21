using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace TechProg_laba1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Ошибка: неверное количество входных элементов");
                Console.WriteLine("Использование: <файл гостиницы> <файл типов номеров> <файл номеров> <файл журнала бронирований>");
                return;
            }

            string hotelFilePath = args[0];
            string roomTypesFilePath = args[1];
            string roomsFilePath = args[2];
            string bookingLogFilePath = args[3];

            Hotel hotel;
            try
            {
                hotel = ReadHotel(hotelFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла гостиницы: {ex.Message}");
                return;
            }
            List<RoomType> roomTypes;
            try
            {
                roomTypes = ReadRoomTypes(roomTypesFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла типов номеров: {ex.Message}");
                return;
            }
            List<Room> rooms;
            try
            {
                rooms = ReadRoom(roomsFilePath, roomTypes, hotel);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Ошибка при чтении файла номеров: {ex.Message}");
                return;
            }

            List<Booking> bookings = new List<Booking>();
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Оформить бронирование");
                Console.WriteLine("2. Добавить номер в гостиницу");
                Console.WriteLine("3. Изменить статус бронирования");
                Console.WriteLine("4. Показать список бронирований");
                Console.WriteLine("5. Выход");
                Console.Write("Выберите пункт: ");

                string choice = ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            CreateBooking(hotel, bookings, bookingLogFilePath);
                            break;
                        case "2":
                            AddRoomToHotel(hotel, roomTypes, bookingLogFilePath);
                            break;
                        case "3":
                            ChangeBookingStatus(bookings, bookingLogFilePath);
                            break;
                        case "4":
                            ShowBookings(bookings);
                            break;
                        case "5":
                            return;
                        default:
                            Console.WriteLine("Ошибка: неверный пункт меню");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
                    Console.WriteLine("Действие не выполнено, вы возвращены в главное меню.");
                }
            }
        }
        static Hotel ReadHotel(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
            {
                throw new ArgumentException("Ошибка: файл с гостиницей пуст");
            }

            string[] parts = lines[0].Split(',');

            if (parts.Length < 3)
            {
                throw new ArgumentException("Ошибка: файл с гостиницой - неверное кол-во элементов");
            }

            Hotel hotel = new Hotel();
            hotel.Name = parts[0].Trim();
            hotel.Star = int.Parse(parts[1]);
            hotel.Seasonal = bool.Parse(parts[2]);

            return hotel;
        }

        static List<RoomType> ReadRoomTypes(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            List<RoomType> roomTypes = new List<RoomType>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    string[] parts = line.Split(',');

                    if (parts.Length < 3)
                    {
                        throw new ArgumentException("неверное количество полей в строке");
                    }

                    RoomType roomType = new RoomType();
                    roomType.Name = parts[0].Trim();
                    roomType.PricePerNight = decimal.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
                    roomType.Capacity = int.Parse(parts[2]);

                    roomTypes.Add(roomType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: строка '{line}': {ex.Message}");
                }
            }

            return roomTypes;
        }

        static List<Room> ReadRoom(string filePath, List<RoomType> roomTypes, Hotel hotel)
        {
            string[] lines = File.ReadAllLines(filePath);
            List<Room> rooms = new List<Room>();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    string[] parts = line.Split(",");
                    if (parts.Length < 2)
                    {
                        throw new ArgumentException("Ошибка: неверное колличество полей в строке");
                    }
                    RoomType foundType = null;
                    foreach (RoomType rt in roomTypes)
                    {
                        if (rt.Name == parts[1].Trim())
                        {
                            foundType = rt;
                            break;
                        }
                    }
                    if (foundType == null)
                    {
                        throw new ArgumentException("Ошибка: тип номера неизвестен для программы");
                    }

                    Room room = new Room(parts[0].Trim(), foundType);
                    hotel.AddRoom(room);

                    if (hotel.Rooms.Contains(room))
                    {
                        rooms.Add(room);
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Ошибка: строка '{line}': {ex.Message}");
                }
            }
            return rooms;
        }


        static void CreateBooking(Hotel hotel, List<Booking> bookings, string bookingLogFilePath)
        {
            bool hasAvalibleRooms = false;
            Console.WriteLine($"Свободные номера гостиницы «{hotel.Name}»:");
            foreach (Room room in hotel.Rooms)
            {
                if (!room.IsOccupied)
                {
                    Console.WriteLine($"Номер: {room.RoomNumber}, тип: {room.RoomType.Name}, цена за сутки: {room.RoomType.PricePerNight:F2}, вместимость: {room.RoomType.Capacity}");
                    hasAvalibleRooms = true;
                }
            }
            if (hasAvalibleRooms == false)
            {
                Console.WriteLine("Нет свободных номеров");
                return;
            }

            Room selectedRoom = null;
            while (selectedRoom == null)
            {
                Console.Write("Выберите номер комнаты для бронирования (пустая строка - отмена): ");
                string roomNumberInput = ReadLine();

                if (roomNumberInput == "")
                {
                    Console.WriteLine("Бронирование отменено");
                    return;
                }

                selectedRoom = FindRoom(hotel, roomNumberInput);
                if (selectedRoom == null)
                {
                    Console.WriteLine("Ошибка: комнаты с таким номером нет в списке");
                }
                else if (selectedRoom.IsOccupied)
                {
                    Console.WriteLine("Ошибка: данный номер уже занят");
                    selectedRoom = null;
                }
            }

            Client client = ReadClient();
            if (client == null)
            {
                Console.WriteLine("Бронирование отменено");
                return;
            }

            Booking booking = ReadBooking(selectedRoom, client);
            if (booking == null)
            {
                Console.WriteLine("Бронирование отменено");
                return;
            }

            try
            {
                booking.CalculateCost();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка при расчёте стоимости: {ex.Message}");
                Console.WriteLine("Бронирование не оформлено");
                return;
            }

            selectedRoom.IsOccupied = true;
            bookings.Add(booking);

            string confirmation = "=== ПОДТВЕРЖДЕНИЕ БРОНИРОВАНИЯ ===" + Environment.NewLine
                                  + BuildBookingText(booking, bookings.Count);
            Console.WriteLine();
            Console.WriteLine(confirmation);
            WriteToLog(bookingLogFilePath, confirmation);
        }

        static Client ReadClient()
        {
            while (true)
            {
                Console.Write("Введите ваше ФИО: ");
                string fullName = ReadLine();

                Console.Write("Введите номер телефона: ");
                string contactInfo = ReadLine();

                try
                {
                    Client client = new Client(fullName, contactInfo, false);
                    client.IsLoyaltyMember = ReadYesNo("Вы участвуете в нашей программе лояльности?");
                    return client;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    if (!ReadYesNo("Повторить ввод данных клиента?"))
                    {
                        return null;
                    }
                }
            }
        }

        static Booking ReadBooking(Room room, Client client)
        {
            while (true)
            {
                DateTime checkInDate = ReadDate("Введите дату заезда (гггг-мм-дд): ");
                DateTime checkOutDate = ReadDate("Введите дату выезда (гггг-мм-дд): ");
                int guestCount = ReadInt("Введите количество гостей: ");

                try
                {

                    return new Booking(room, client, checkInDate, checkOutDate, guestCount);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                    if (!ReadYesNo("Повторить ввод дат и количества гостей?"))
                    {
                        return null;
                    }
                }
            }
        }


        static void AddRoomToHotel(Hotel hotel, List<RoomType> roomTypes, string bookingLogFilePath)
        {
            if (roomTypes.Count == 0)
            {
                Console.WriteLine("Ошибка: не загружено ни одного типа номеров, добавить номер невозможно");
                return;
            }

            Console.Write("Введите номер новой комнаты: ");
            string roomNumber = ReadLine();

            Console.WriteLine("Типы номеров:");
            for (int i = 0; i < roomTypes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {roomTypes[i].Name}, цена за сутки: {roomTypes[i].PricePerNight:F2}, вместимость: {roomTypes[i].Capacity}");
            }

            int typeNumber = ReadInt("Выберите тип номера: ");
            if (typeNumber < 1 || typeNumber > roomTypes.Count)
            {
                Console.WriteLine($"Ошибка: нет типа номера с номером {typeNumber} (доступно: 1-{roomTypes.Count})");
                return;
            }

            Room room;
            try
            {
                room = new Room(roomNumber, roomTypes[typeNumber - 1]);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }


            hotel.AddRoom(room);
            if (!hotel.Rooms.Contains(room))
            {
                Console.WriteLine("Номер не добавлен");
                return;
            }

            string message = $"Номер {room.RoomNumber} (тип «{room.RoomType.Name}») добавлен в гостиницу «{hotel.Name}»";
            Console.WriteLine(message);
            WriteToLog(bookingLogFilePath, message);
        }


        static void ChangeBookingStatus(List<Booking> bookings, string bookingLogFilePath)
        {
            if (bookings.Count == 0)
            {
                Console.WriteLine("Бронирований пока нет");
                return;
            }

            ShowBookings(bookings);

            int number = ReadInt("Введите номер бронирования: ");
            if (number < 1 || number > bookings.Count)
            {
                Console.WriteLine($"Ошибка: бронирования с номером {number} нет (доступно: 1-{bookings.Count})");
                return;
            }

            Booking booking = bookings[number - 1];
            BookingStatus oldStatus = booking.Status;

            Console.WriteLine($"Текущий статус: {StatusToText(oldStatus)}");
            Console.WriteLine("Новый статус:");
            Console.WriteLine("1. Заселён");
            Console.WriteLine("2. Выселен");
            Console.WriteLine("3. Отменено");
            Console.Write("Выберите пункт: ");

            BookingStatus newStatus;
            switch (ReadLine())
            {
                case "1":
                    newStatus = BookingStatus.CheckedIn;
                    break;
                case "2":
                    newStatus = BookingStatus.CheckedOut;
                    break;
                case "3":
                    newStatus = BookingStatus.Cancelled;
                    break;
                default:
                    Console.WriteLine("Ошибка: неверный пункт, статус не изменён");
                    return;
            }

            if (!booking.TryChangeStatus(newStatus))
            {
                Console.WriteLine("Статус не изменён");
                return;
            }


            if (newStatus == BookingStatus.CheckedOut || newStatus == BookingStatus.Cancelled)
            {
                booking.Room.IsOccupied = false;
            }

            string message = $"Бронирование №{number}: статус изменён «{StatusToText(oldStatus)}» -> «{StatusToText(newStatus)}»";
            if (!booking.Room.IsOccupied)
            {
                message += $". Номер {booking.Room.RoomNumber} освобождён";
            }
            Console.WriteLine(message);
            WriteToLog(bookingLogFilePath, message);
        }


        static void ShowBookings(List<Booking> bookings)
        {
            if (bookings.Count == 0)
            {
                Console.WriteLine("Бронирований пока нет");
                return;
            }

            for (int i = 0; i < bookings.Count; i++)
            {
                Console.WriteLine(BuildBookingText(bookings[i], i + 1));
                Console.WriteLine();
            }
        }

        static Room FindRoom(Hotel hotel, string roomNumber)
        {
            foreach (Room room in hotel.Rooms)
            {
                if (string.Equals(room.RoomNumber, roomNumber, StringComparison.OrdinalIgnoreCase))
                {
                    return room;
                }
            }
            return null;
        }

        static string BuildBookingText(Booking booking, int number)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine($"Бронирование №{number}");
            text.AppendLine($"  Гостиница: {booking.Room.Hotel.Name} ({booking.Room.Hotel.Star} зв.)");
            text.AppendLine($"  Номер:     {booking.Room.RoomNumber}, тип «{booking.Room.RoomType.Name}»");
            text.AppendLine($"  Гость:     {booking.Client.FullName}, {booking.Client.ContactInfo}");
            text.AppendLine($"  Заезд:     {booking.CheckInDate:dd.MM.yyyy}");
            text.AppendLine($"  Выезд:     {booking.CheckOutDate:dd.MM.yyyy} (ночей: {booking.GetNightCount()})");
            text.AppendLine($"  Гостей:    {booking.GuestCount}");
            text.AppendLine($"  Статус:    {StatusToText(booking.Status)}");
            text.Append($"  Стоимость: {booking.TotalCost:F2} руб.");
            return text.ToString();
        }

        static string StatusToText(BookingStatus status)
        {
            switch (status)
            {
                case BookingStatus.Created:
                    return "Создано";
                case BookingStatus.CheckedIn:
                    return "Заселён";
                case BookingStatus.CheckedOut:
                    return "Выселен";
                case BookingStatus.Cancelled:
                    return "Отменено";
                default:
                    return status.ToString();
            }
        }

        static void WriteToLog(string filePath, string text)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                    writer.WriteLine(text);
                    writer.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Предупреждение: не удалось записать в файл журнала: {ex.Message}");
            }
        }

        static string ReadLine()
        {
            string line = Console.ReadLine();
            if (line == null)
            {
                Console.WriteLine();
                Console.WriteLine("Ввод завершён. Работа программы остановлена.");
                Environment.Exit(0);
            }
            return line.Trim();
        }

        static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                DateTime date;
                if (DateTime.TryParseExact(ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    return date;
                }
                Console.WriteLine($"Ошибка: дата должна быть в формате гггг-мм-дд, например {DateTime.Today:yyyy-MM-dd}");
            }
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                int value;
                if (int.TryParse(ReadLine(), out value))
                {
                    return value;
                }
                Console.WriteLine("Ошибка: нужно ввести целое число");
            }
        }

        static bool ReadYesNo(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (да/нет): ");
                string answer = ReadLine().ToLower();
                if (answer == "да" || answer == "д")
                {
                    return true;
                }
                if (answer == "нет" || answer == "н")
                {
                    return false;
                }
                Console.WriteLine("Ошибка: ответьте «да» или «нет»");
            }
        }
    }
}