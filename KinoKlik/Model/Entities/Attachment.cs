namespace KinoKlik.Model.Entities;

public class Attachment
{

    public int Id { get; set; }

    public int MovieId { get; set; }
    public virtual Movie Movie { get; set; } = null!;

    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long FileSize { get; set; }

    public DateTime CreatedAt { get; set; }
}
