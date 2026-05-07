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
    public List<TicketBuilderCheckoutCustomerViewModel> Customers { get; set; } = new();
}

public class TicketBuilderCheckoutCustomerViewModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
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

    [Required]
    [Display(Name = "Kupac")]
    public int CustomerId { get; set; }
}
