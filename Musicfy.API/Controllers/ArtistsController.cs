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
                    .GetArtists();
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
                    .GetArtists(a => a.Id == key);
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
        #endregion

        #region Put
        #endregion

        #region Patch
        #endregion

        #region Delete
        #endregion
    }
}
