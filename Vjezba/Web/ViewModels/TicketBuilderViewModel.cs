namespace Vjezba.Web.ViewModels;

using System.ComponentModel.DataAnnotations;

public class TicketBuilderCinemaListViewModel
{
    public List<Vjezba.Model.Entities.Cinema> Cinemas { get; set; } = new();
}

public class TicketBuilderMovieListViewModel
{
    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = string.Empty;
    public List<TicketBuilderMovieCardViewModel> Movies { get; set; } = new();
}

public class TicketBuilderMovieCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public string Language { get; set; } = string.Empty;
    public string AgeRating { get; set; } = string.Empty;
}

public class TicketBuilderScreeningListViewModel
{
    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public List<TicketBuilderScreeningCardViewModel> Screenings { get; set; } = new();
}

public class TicketBuilderScreeningCardViewModel
{
    public int Id { get; set; }
    public int HallId { get; set; }
    public string HallName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Is3D { get; set; }
}

public class TicketBuilderSeatPageViewModel
{
    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public int ScreeningId { get; set; }
    public string HallName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool Is3D { get; set; }
    public List<TicketBuilderSeatViewModel> Seats { get; set; } = new();
    public Dictionary<int, string> SeatStatuses { get; set; } = new();
}

public class TicketBuilderSeatViewModel
{
    public int Id { get; set; }
    public string RowLabel { get; set; } = string.Empty;
    public int SeatNumber { get; set; }
    public string SeatType { get; set; } = string.Empty;
}

public class TicketBuilderCheckoutViewModel
{
    public int CinemaId { get; set; }
    public string CinemaName { get; set; } = string.Empty;
    public int MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public int ScreeningId { get; set; }
    public DateTime StartTime { get; set; }
    public string HallName { get; set; } = string.Empty;
    public bool Is3D { get; set; }
    public int SeatId { get; set; }
    public string SeatLabel { get; set; } = string.Empty;
    public string SeatType { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAuthenticated { get; set; }
    public string KnownEmail { get; set; } = string.Empty;
    public TicketBuilderCheckoutInputModel Input { get; set; } = new();
}

public class TicketBuilderCheckoutInputModel
{
    [Required]
    public int CinemaId { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Required]
    public int ScreeningId { get; set; }

    [Required]
    public int SeatId { get; set; }

    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(60, ErrorMessage = "Ime ne smije biti duze od {1} znakova.")]
    [Display(Name = "Ime")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(60, ErrorMessage = "Prezime ne smije biti duze od {1} znakova.")]
    [Display(Name = "Prezime")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Grad je obavezan.")]
    [StringLength(80, ErrorMessage = "Grad ne smije biti duzi od {1} znakova.")]
    [Display(Name = "Grad")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ulica je obavezna.")]
    [StringLength(120, ErrorMessage = "Ulica ne smije biti duza od {1} znakova.")]
    [Display(Name = "Ulica")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kucni broj je obavezan.")]
    [StringLength(20, ErrorMessage = "Kucni broj ne smije biti duzi od {1} znakova.")]
    [Display(Name = "Kucni broj")]
    public string HouseNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postanski broj je obavezan.")]
    [StringLength(12, MinimumLength = 4, ErrorMessage = "Postanski broj mora imati izmedu {2} i {1} znakova.")]
    [Display(Name = "Postanski broj")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta je obavezna.")]
    [EmailAddress(ErrorMessage = "Unesite ispravnu e-postu.")]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Telefon je obavezan.")]
    [Phone(ErrorMessage = "Unesite ispravan telefonski broj.")]
    [StringLength(30, ErrorMessage = "Telefon ne smije biti duzi od {1} znakova.")]
    [Display(Name = "Telefon")]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "Zelim biti clan programa vjernosti")]
    public bool IsLoyaltyMember { get; set; }
}

public class TicketBuilderPurchaseSuccessViewModel
{
    public int TicketId { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CinemaName { get; set; } = string.Empty;
    public string MovieTitle { get; set; } = string.Empty;
    public string HallName { get; set; } = string.Empty;
    public string SeatLabel { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public decimal Price { get; set; }
}
