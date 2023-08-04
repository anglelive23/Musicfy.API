namespace Musicfy.API.Controllers
{
    [Route("api/odata")]
    public class CategoriesController : ODataController
    {
        #region Fields and Properties
        private readonly ICategoryRepository _repo;
        #endregion

        #region Constructors
        public CategoriesController(ICategoryRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        #endregion

        #region Get
        [HttpGet("categories")]
        [OutputCache(PolicyName = "Categories")]
        [EnableQuery(MaxExpansionDepth = 3, PageSize = 1000)]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categories = _repo
                    .GetCategories(a => a.IsDeleted == false);

                return Ok(categories);
            }
            catch (Exception ex) when (ex is DataFailureException
                                    || ex is Exception)
            {
                Log.Error($"{ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("categories({key})")]
        [OutputCache(PolicyName = "Category")]
        [EnableQuery(MaxExpansionDepth = 3)]
        public IActionResult GetCategoryById(int key)
        {
            try
            {
                var category = _repo
                    .GetCategories(a => a.Id == key && a.IsDeleted == false);

                return Ok(SingleResult.Create(category));
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
    }
}
