namespace Musicfy.API.Entities.Models
{
    public class Album
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(255)]
        public string? Description { get; set; } = string.Empty;
        public Artist? Artist { get; set; }
        [Required]
        public int ArtistId { get; set; }
        public DateTime? DateAdded { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public IList<Song>? Songs { get; set; }

    }
}
