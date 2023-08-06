namespace Musicfy.API.Controllers
{
    [Route("api/odata")]
    public class ArtistsController : ODataController
    {
        #region Fields and Properties
        private readonly IArtistRepository _repo;
        #endregion

        #region Constructors
        public ArtistsController(IArtistRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        #endregion

        #region Get
        [HttpGet("artists")]
        [OutputCache(PolicyName = "Artists")]
        [EnableQuery(MaxExpansionDepth = 3, PageSize = 1000)]
        public IActionResult GetAllArtists()
        {
            try
            {
                Log.Information("Starting controller Artists action GetAllArtists.");
                var artists = _repo
                    .GetArtists(a => a.IsDeleted == false);
                Log.Information("Returning all Artists to the caller.");
                return Ok(artists);
            }
            catch (Exception ex) when (ex is DataFailureException
                                    || ex is Exception)
            {
                Log.Error($"{ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("artists({key})")]
        [OutputCache(PolicyName = "Artist")]
        [EnableQuery(MaxExpansionDepth = 3)]
        public IActionResult GetArtistById(int key)
        {
            try
            {
                Log.Information("Starting controller Artists action GetArtistById.");
                var artist = _repo
                    .GetArtists(a => a.Id == key && a.IsDeleted == false);
                Log.Information("Returning Artist data to the caller.");
                return Ok(SingleResult.Create(artist));
            }
            catch (Exception ex) when (ex is DataFailureException
                                    || ex is InvalidOperationException
                                    || ex is Exception)
            {
                Log.Error($"{ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Post
        [HttpPost("artists")]
        public async Task<IActionResult> AddArtist([FromBody] Artist newArtist, [FromServices] IOutputCacheStore cache, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting controller Artists action AddArtist.");
                var artist = await _repo
                    .AddArtistAsync(newArtist);

                if (artist is null)
                    return BadRequest("Artist already exists with same Name!");

                #region Cache Evict
                await cache.EvictByTagAsync("Artists", cancellationToken);
                #endregion

                Log.Information("Artist has been added.");
                return Created(artist);
            }
            catch (Exception ex) when (ex is DataFailureException
                                    || ex is Exception)
            {
                Log.Error($"{ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Put
        [HttpPut("artists({key})")]
        public async Task<IActionResult> UpdateArtist(int key, [FromBody] Artist artist, [FromServices] IOutputCacheStore cache, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting controller Artists action UpdateArtist.");
                var currentArtist = await _repo
                    .UpdateArtistAsync(key, artist);

                if (currentArtist == null)
                    return NotFound("Artist not found!");

                #region Cache Evict
                await cache.EvictByTagAsync("Artists", cancellationToken);
                #endregion

                Log.Information($"Artist with id: {key} has been updated.");
                return NoContent();
            }
            catch (Exception ex) when (ex is DataFailureException
                                    || ex is Exception)
            {
                Log.Error($"{ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Patch
        [HttpPatch("artists({key})")]
        public async Task<IActionResult> PartUpdateArtist(int key, [FromBody] Delta<Artist> artist, [FromServices] IOutputCacheStore cache, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting controller Artists action PartUpdateArtist.");
                var currentArtist = await _repo
                    .PartUpdateArtistAsync(key, artist);

                if (currentArtist == null)
                    return NotFound("Artist not found!");

                #region Cache Evict
                await cache.EvictByTagAsync("Artists", cancellationToken);
                #endregion

                Log.Information($"Artist with id: {key} has been patched.");
                return NoContent();
            }
            catch (Exception ex) when (ex is DataFailureException
                                    || ex is Exception)
            {
                Log.Error($"{ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Delete
        [HttpDelete("artists({key})")]
        public async Task<IActionResult> RemoveArtist(int key, [FromServices] IOutputCacheStore cache, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting controller Artists action RemoveArtist.");
                var currentArtist = await _repo
                    .RemoveArtistAsync(key);

                if (currentArtist is false)
                    return NotFound("artist not found!");

                #region Cache Evict
                await cache.EvictByTagAsync("Artists", cancellationToken);
                #endregion

                Log.Information($"Artist with id: {key} has been marked as deleted.");
                return NoContent();
            }
            catch (Exception ex) when (ex is DataFailureException
                                    || ex is Exception)
            {
                Log.Error($"{ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
