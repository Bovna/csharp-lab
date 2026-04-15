using Vjezba.Model.Models.Entities;

namespace Vjezba.Model.Data;

public class MovieMockRepository
{
    public List<Movie> GetAll()
    {
        return Movies.ToList();
    }

    public Movie? GetById(int id)
    {
        return Movies.FirstOrDefault(m => m.Id == id);
    }

    private static readonly List<Movie> Movies =
    [
        new Movie { Id = 1, Title = "Galactic Run", Description = "Sci-fi akcija o bijegu kroz galaksiju.", DurationMinutes = 155, ReleaseDate = new DateTime(2025, 12, 12), Genre = MovieGenre.SciFi, Language = "EN", AgeRating = "12+" },
        new Movie { Id = 2, Title = "Tiha Ulica", Description = "Kriminalisticka drama smjestena u Zagrebu.", DurationMinutes = 110, ReleaseDate = new DateTime(2025, 10, 1), Genre = MovieGenre.Crime, Language = "HR", AgeRating = "15+" },
        new Movie { Id = 3, Title = "Mali Izumitelj", Description = "Animirana avantura za cijelu obitelj.", DurationMinutes = 90, ReleaseDate = new DateTime(2026, 2, 20), Genre = MovieGenre.Animation, Language = "HR", AgeRating = "U" },
        new Movie { Id = 4, Title = "Planina Straha", Description = "Horror triler o ekspediciji koja krene po zlu.", DurationMinutes = 130, ReleaseDate = new DateTime(2025, 9, 15), Genre = MovieGenre.Horror, Language = "EN", AgeRating = "16+" },
        new Movie { Id = 5, Title = "Brzina Noci", Description = "Akcijski film o ilegalnim utrkama.", DurationMinutes = 125, ReleaseDate = new DateTime(2026, 1, 10), Genre = MovieGenre.Action, Language = "EN", AgeRating = "12+" },
        new Movie { Id = 6, Title = "Ljeto u Puli", Description = "Romanticna drama snimljena na obali.", DurationMinutes = 100, ReleaseDate = new DateTime(2025, 6, 25), Genre = MovieGenre.Romance, Language = "HR", AgeRating = "12+" },
        new Movie { Id = 7, Title = "Sjene Istine", Description = "Napeti triler o novinarskoj istrazi.", DurationMinutes = 105, ReleaseDate = new DateTime(2025, 11, 30), Genre = MovieGenre.Thriller, Language = "EN", AgeRating = "15+" },
        new Movie { Id = 8, Title = "Kraljevstvo Vjetra", Description = "Fantasy pustolovina kroz tri svijeta.", DurationMinutes = 125, ReleaseDate = new DateTime(2026, 3, 8), Genre = MovieGenre.Fantasy, Language = "EN", AgeRating = "10+" },
        new Movie { Id = 9, Title = "Smijeh do Suza", Description = "Komedija o tri prijatelja i jednoj svadbi.", DurationMinutes = 100, ReleaseDate = new DateTime(2025, 8, 18), Genre = MovieGenre.Comedy, Language = "HR", AgeRating = "U" },
        new Movie { Id = 10, Title = "Ledena Dubina", Description = "Dokumentarac o zivotu ispod antarktickog leda.", DurationMinutes = 90, ReleaseDate = new DateTime(2024, 12, 5), Genre = MovieGenre.Documentary, Language = "EN", AgeRating = "U" },
        new Movie { Id = 11, Title = "Zvuk Tisine", Description = "Drama o glazbenici koja se vraca na pozornicu.", DurationMinutes = 112, ReleaseDate = new DateTime(2026, 4, 5), Genre = MovieGenre.Drama, Language = "HR", AgeRating = "12+" },
        new Movie { Id = 12, Title = "Put Bez Mape", Description = "Avanturisticki road trip kroz Europu.", DurationMinutes = 118, ReleaseDate = new DateTime(2026, 2, 2), Genre = MovieGenre.Adventure, Language = "EN", AgeRating = "10+" },
        new Movie { Id = 13, Title = "Nocturna", Description = "Krimi misterij u nocnom gradu.", DurationMinutes = 123, ReleaseDate = new DateTime(2025, 12, 20), Genre = MovieGenre.Crime, Language = "EN", AgeRating = "15+" },
        new Movie { Id = 14, Title = "Signal 404", Description = "Napeta sci-fi misterija o nestalom signalu.", DurationMinutes = 108, ReleaseDate = new DateTime(2026, 4, 10), Genre = MovieGenre.SciFi, Language = "EN", AgeRating = "12+" },
        new Movie { Id = 15, Title = "Dnevnik Sjevera", Description = "Dokumentarni film o arktickim ekspedicijama.", DurationMinutes = 96, ReleaseDate = new DateTime(2026, 1, 22), Genre = MovieGenre.Documentary, Language = "HR", AgeRating = "U" },
        new Movie { Id = 16, Title = "Juzni Vjetar", Description = "Drama o povratku kuci nakon dugog putovanja.", DurationMinutes = 114, ReleaseDate = new DateTime(2026, 3, 2), Genre = MovieGenre.Drama, Language = "HR", AgeRating = "12+" }
    ];
}
