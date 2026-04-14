using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw5_s31003.Data;
using PJATK_APBD_Cw5_s31003.Models;

namespace PJATK_APBD_Cw5_s31003.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetRooms([FromQuery] int? minCapacity, [FromQuery] bool? hasProjector, [FromQuery] bool? activeOnly)
    {
        var query = DataStore.Rooms.AsQueryable();

        if (minCapacity.HasValue)
            query = query.Where(r => r.Capacity >= minCapacity.Value);
        
        if (hasProjector.HasValue)
            query = query.Where(r => r.HasProjector == hasProjector.Value);

        if (activeOnly.HasValue && activeOnly.Value)
            query = query.Where(r => r.IsActive);

        return Ok(query.ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Room> GetRoom(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        
        if (room is null)
        {
            return NotFound();
        }
        
        return Ok(room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<IEnumerable<Room>> GetRoomsByBuilding(string buildingCode)
    {
        var rooms = DataStore.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();
            
        return Ok(rooms);
    }

    [HttpPost]
    public ActionResult<Room> CreateRoom(Room newRoom)
    {
        newRoom.Id = DataStore.Rooms.Any() ? DataStore.Rooms.Max(r => r.Id) + 1 : 1;
        DataStore.Rooms.Add(newRoom);

        return CreatedAtAction(nameof(GetRoom), new { id = newRoom.Id }, newRoom);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateRoom(int id, Room updatedRoom)
    {
        var existingRoom = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        
        if (existingRoom is null)
        {
            return NotFound();
        }

        existingRoom.Name = updatedRoom.Name;
        existingRoom.BuildingCode = updatedRoom.BuildingCode;
        existingRoom.Floor = updatedRoom.Floor;
        existingRoom.Capacity = updatedRoom.Capacity;
        existingRoom.HasProjector = updatedRoom.HasProjector;
        existingRoom.IsActive = updatedRoom.IsActive;

        return Ok(existingRoom);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteRoom(int id)
    {
        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        
        if (room is null)
        {
            return NotFound();
        }

        var hasReservations = DataStore.Reservations.Any(res => res.RoomId == id);
        
        if (hasReservations)
        {
            return Conflict(new { message = "Nie mozna usunac sali, poniewaz posiada przypisane rezerwacje." });
        }

        DataStore.Rooms.Remove(room);
        
        return NoContent();
    }
}