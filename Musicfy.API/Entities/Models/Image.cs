namespace Musicfy.API.Entities.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Path { get; set; } = null!;
        [Required, MaxLength(255)]
        public string Description { get; set; } = null!;
    }
}
