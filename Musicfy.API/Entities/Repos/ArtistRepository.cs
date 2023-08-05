namespace Musicfy.API.Entities.Repos
{
    public interface IArtistRepository
    {
        #region Get
        IQueryable<Artist> GetArtists(Expression<Func<Artist, bool>>? predicate = null);
        Task<Artist?> GetArtistByIdAsync(int id);
        #endregion

        #region Post
        #endregion

        #region Put
        #endregion

        #region Patch
        #endregion

        #region Delete
        #endregion
    }
    public class ArtistRepository : IArtistRepository
    {
        #region Fields and Properties
        private readonly MusicfyContext _context;
        #endregion

        #region Constructors
        public ArtistRepository(MusicfyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Get
        public IQueryable<Artist> GetArtists(Expression<Func<Artist, bool>>? predicate = null)
        {
            try
            {
                IQueryable<Artist>? artists = _context
                      .Artists;

                if (predicate is not null)
                    artists = artists.Where(predicate);

                return artists;
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is InvalidOperationException
                                    || ex is SqlException)
            {
                throw new DataFailureException(ex.Message);
            }
        }

        public async Task<Artist?> GetArtistByIdAsync(int id)
        {
            try
            {
                var artist = await _context
                    .Artists
                    .FirstOrDefaultAsync(a => a.Id == id);

                return artist;
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is InvalidOperationException
                                    || ex is SqlException)
            {
                throw new DataFailureException(ex.Message);
            }
        }
        #endregion

        #region Post
        #endregion

        #region Put
        #endregion

        #region Patch
        #endregion

        #region Delete
        #endregion
    }
}
