namespace Musicfy.API.Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public Image? Image { get; set; }
        public int? ImageId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IList<Song>? Songs { get; set; }
        public string? Description { get; set; }
    }
}
