namespace Musicfy.API.Entities.Repos
{
    public interface IArtistRepository
    {
        #region Get
        IQueryable<Artist> GetArtists(Expression<Func<Artist, bool>>? predicate = null);
        Task<Artist?> GetArtistByIdAsync(int id);
        #endregion

        #region Post
        Task<Artist?> AddArtistAsync(Artist artist);
        #endregion

        #region Put
        Task<Artist?> UpdateArtistAsync(int id, Artist artist);
        #endregion

        #region Patch
        Task<Artist?> PartUpdateArtistAsync(int id, Delta<Artist> artist);
        #endregion

        #region Delete
        Task<bool> RemoveArtistAsync(int id);
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
        public async Task<Artist?> AddArtistAsync(Artist artist)
        {
            try
            {
                if (IsExistingArtist(artist.Name))
                    return null;

                _context.Artists.Add(artist);
                await _context.SaveChangesAsync();

                return artist;
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is InvalidOperationException
                                    || ex is DbUpdateException
                                    || ex is SqlException)
            {
                throw new DataFailureException(ex.Message);
            }
        }
        #endregion

        #region Put
        public async Task<Artist?> UpdateArtistAsync(int id, Artist artist)
        {
            try
            {
                var currentArtist = await GetArtistByIdAsync(id);

                if (currentArtist == null)
                    return null;

                artist.Id = currentArtist.Id;
                artist.LastModified = DateTime.UtcNow;
                _context.Entry(currentArtist).CurrentValues.SetValues(artist);
                _context.Update(currentArtist);
                await _context.SaveChangesAsync();

                return currentArtist;
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is InvalidOperationException
                                    || ex is DbUpdateException
                                    || ex is SqlException)
            {
                throw new DataFailureException(ex.Message);
            }
        }
        #endregion

        #region Patch
        public async Task<Artist?> PartUpdateArtistAsync(int id, Delta<Artist> artist)
        {
            try
            {
                var currentArtist = await GetArtistByIdAsync(id);

                if (currentArtist == null)
                    return null;

                artist.Patch(currentArtist);
                currentArtist.LastModified = DateTime.UtcNow;
                _context.Update(currentArtist);
                await _context.SaveChangesAsync();

                return currentArtist;
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is InvalidOperationException
                                    || ex is DbUpdateException
                                    || ex is SqlException)
            {
                throw new DataFailureException(ex.Message);
            }
        }
        #endregion

        #region Delete
        public async Task<bool> RemoveArtistAsync(int id)
        {
            try
            {
                var artist = await GetArtistByIdAsync(id);

                if (artist == null)
                    return false;

                artist.IsDeleted = true;
                artist.LastModified = DateTime.UtcNow;
                _context.Update(artist);

                return _context.SaveChanges() > 0;
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is InvalidOperationException
                                    || ex is DbUpdateException
                                    || ex is SqlException)
            {
                throw new DataFailureException(ex.Message);
            }
        }
        #endregion

        #region Helpers
        private bool IsExistingArtist(string name)
        {
            return _context
                .Artists
                .Any(a => a.Name == name && a.IsDeleted == false);
        }
        #endregion
    }
}
