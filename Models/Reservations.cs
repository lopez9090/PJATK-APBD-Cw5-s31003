using System.ComponentModel.DataAnnotations;

namespace PJATK_APBD_Cw5_s31003.Models;

public class Reservation : IValidatableObject
{
    public int Id { get; set; }
    public int RoomId { get; set; }

    [Required(ErrorMessage = "Imie organizatora jest wymagane.")]
    public string OrganizerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Temat jest wymagany.")]
    public string Topic { get; set; } = string.Empty;

    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = "planned";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
        {
            yield return new ValidationResult(
                "Czas zakonczenia (EndTime) musi być pozniejszy niż czas rozpoczecia (StartTime).",
                new[] { nameof(EndTime), nameof(StartTime) });
        }
    }
}