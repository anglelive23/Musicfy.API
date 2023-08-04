namespace Musicfy.API.Entities.Data
{
    public class MusicfyContext : DbContext
    {
        public MusicfyContext(DbContextOptions<MusicfyContext> options) : base(options) { }
    }
}
