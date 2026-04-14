using PJATK_APBD_Cw5_s31003.Models;

namespace PJATK_APBD_Cw5_s31003.Data;

public static class DataStore
{
    public static List<Room> Rooms { get; set; } = new List<Room>()
    {
        new Room { Id = 1, Name = "Lab 101", BuildingCode = "A", Floor = 1, Capacity = 15, HasProjector = true, IsActive = true },
        new Room { Id = 2, Name = "Aula A1", BuildingCode = "A", Floor = 0, Capacity = 200, HasProjector = true, IsActive = true },
        new Room { Id = 3, Name = "Lab 003", BuildingCode = "B", Floor = -1, Capacity = 16, HasProjector = false, IsActive = true },
        new Room { Id = 4, Name = "Lab 204", BuildingCode = "B", Floor = 2, Capacity = 24, HasProjector = true, IsActive = false },
        new Room { Id = 5, Name = "Aula C1", BuildingCode = "C", Floor = 1, Capacity = 100, HasProjector = true, IsActive = true }
    };

    public static List<Reservation> Reservations { get; set; } = new List<Reservation>()
    {
        new Reservation { Id = 1, RoomId = 1, OrganizerName = "Jan Nowak", Topic = "Wstep do C#", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(11, 0), Status = "confirmed" },
        new Reservation { Id = 2, RoomId = 2, OrganizerName = "Anna Kowalska", Topic = "Architektura REST", Date = new DateOnly(2026, 5, 10), StartTime = new TimeOnly(12, 0), EndTime = new TimeOnly(14, 0), Status = "planned" },
        new Reservation { Id = 3, RoomId = 3, OrganizerName = "Kolo PJSEC", Topic = "Daily", Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(9, 30), EndTime = new TimeOnly(10, 0), Status = "confirmed" },
        new Reservation { Id = 4, RoomId = 1, OrganizerName = "Jan Nowak", Topic = "C#", Date = new DateOnly(2026, 5, 11), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(13, 0), Status = "cancelled" },
        new Reservation { Id = 5, RoomId = 5, OrganizerName = "Rektor", Topic = "Inauguracja", Date = new DateOnly(2026, 10, 1), StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(12, 0), Status = "planned" }
    };
}