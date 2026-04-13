using System.ComponentModel.DataAnnotations;

namespace PJATK_APBD_Cw5_s31003.Models;

public class Room
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa sali jest wymagana.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kod budynku jest wymagany.")]
    public string BuildingCode { get; set; } = string.Empty;

    public int Floor { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Pojemnosc musi byc wieksza od zera.")]
    public int Capacity { get; set; }

    public bool HasProjector { get; set; }
    
    public bool IsActive { get; set; }
}