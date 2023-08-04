using Musicfy.API.Entities.Models;

namespace Musicfy.API.Entities.Data
{
    public class MusicfyContext : DbContext
    {
        public MusicfyContext(DbContextOptions<MusicfyContext> options) : base(options) { }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
    }
}
