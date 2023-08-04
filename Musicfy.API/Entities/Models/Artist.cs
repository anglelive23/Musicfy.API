namespace Musicfy.API.Entities.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(250)]
        public string Brief { get; set; } = string.Empty;
        public bool IsVerfied { get; set; } = false;
        public Image? Image { get; set; }
        public int? ImageId { get; set; }
        public IList<Album>? Albums { get; set; }
        public IList<Song>? Songs { get; set; }
    }
}
