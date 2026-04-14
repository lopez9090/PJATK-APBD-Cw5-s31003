using Microsoft.AspNetCore.Mvc;
using PJATK_APBD_Cw5_s31003.Data;
using PJATK_APBD_Cw5_s31003.Models;

namespace PJATK_APBD_Cw5_s31003.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> GetReservations([FromQuery] DateOnly? date, [FromQuery] string? status, [FromQuery] int? roomId)
    {
        var query = DataStore.Reservations.AsQueryable();

        if (date.HasValue) 
            query = query.Where(r => r.Date == date.Value);
        
        if (!string.IsNullOrEmpty(status)) 
            query = query.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        
        if (roomId.HasValue) 
            query = query.Where(r => r.RoomId == roomId.Value);

        return Ok(query.ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetReservation(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        
        if (reservation is null)
        {
            return NotFound();
        }
        
        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult<Reservation> CreateReservation(Reservation newReservation)
    {
        if (!ValidateBusinessRules(newReservation, out string errorMessage))
        {
            return Conflict(new { message = errorMessage });
        }

        newReservation.Id = DataStore.Reservations.Any() ? DataStore.Reservations.Max(r => r.Id) + 1 : 1;
        DataStore.Reservations.Add(newReservation);

        return CreatedAtAction(nameof(GetReservation), new { id = newReservation.Id }, newReservation);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateReservation(int id, Reservation updatedReservation)
    {
        var existingReservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        
        if (existingReservation is null)
        {
            return NotFound();
        }

        updatedReservation.Id = id;

        if (!ValidateBusinessRules(updatedReservation, out string errorMessage))
        {
            return Conflict(new { message = errorMessage });
        }

        existingReservation.RoomId = updatedReservation.RoomId;
        existingReservation.OrganizerName = updatedReservation.OrganizerName;
        existingReservation.Topic = updatedReservation.Topic;
        existingReservation.Date = updatedReservation.Date;
        existingReservation.StartTime = updatedReservation.StartTime;
        existingReservation.EndTime = updatedReservation.EndTime;
        existingReservation.Status = updatedReservation.Status;

        return Ok(existingReservation);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteReservation(int id)
    {
        var reservation = DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        
        if (reservation is null)
        {
            return NotFound();
        }

        DataStore.Reservations.Remove(reservation);
        
        return NoContent();
    }

    private bool ValidateBusinessRules(Reservation reservation, out string errorMessage)
    {
        errorMessage = string.Empty;

        var room = DataStore.Rooms.FirstOrDefault(r => r.Id == reservation.RoomId);
        
        if (room is null)
        {
            errorMessage = "Nie ma takiej sali.";
            return false;
        }
        
        if (!room.IsActive)
        {
            errorMessage = "Wskazana sala jest obecnie niedostępna.";
            return false;
        }

        bool isConflict = DataStore.Reservations.Any(r =>
            r.RoomId == reservation.RoomId &&
            r.Date == reservation.Date &&
            r.Status != "cancelled" &&
            r.Id != reservation.Id &&
            reservation.StartTime < r.EndTime &&
            reservation.EndTime > r.StartTime
        );

        if (isConflict)
        {
            errorMessage = "Termin rezerwacji pokrywa się z inna rezerwacja dla tej sali.";
            return false;
        }

        return true;
    }
}