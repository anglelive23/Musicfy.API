namespace Musicfy.API.Entities.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        public string? Lyrics { get; set; }

        public Artist? Artist { get; set; }
        public int? ArtistId { get; set; }

        public DateTime? DateAdded { get; set; } = DateTime.UtcNow;
        public DateTime? DateReleased { get; set; }

        public Genre? Genre { get; set; }
        [Required]
        public int GenreId { get; set; }

        public Album? Album { get; set; }
        [Required]
        public int AlbumId { get; set; }

        public Category? Category { get; set; }
        [Required]
        public int CategoryId { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsTrend { get; set; } = false;

        [Required, Precision(18, 2)]
        public decimal Rate { get; set; }

        public Image? Image { get; set; }
        public int? ImageId { get; set; }

        public IList<Playlist>? Playlists { get; set; }
    }
}
