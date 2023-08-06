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
                Log.Information("Starting controller Categories action GetAllCategories.");
                var categories = _repo
                    .GetCategories(a => a.IsDeleted == false);
                Log.Information("Returning all Categories to the caller.");
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
                Log.Information("Starting controller Categories action GetCategoryById.");
                var category = _repo
                    .GetCategories(a => a.Id == key && a.IsDeleted == false);
                Log.Information("Returning Category data to the caller.");
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

        #region Post
        [HttpPost("categories")]
        public async Task<IActionResult> AddCategory([FromBody] Category newCategory, [FromServices] IOutputCacheStore cache, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting controller Categories action AddCategory.");

                var category = await _repo
                    .AddCategoryAsync(newCategory);

                if (category is null)
                    return BadRequest("Category already exists with same Name!");

                #region Cache Evict
                await cache.EvictByTagAsync("Categories", cancellationToken);
                #endregion

                Log.Information("Category has been added.");
                return Created(category);
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
        [HttpPut("categories({key})")]
        public async Task<IActionResult> UpdateCategory(int key, [FromBody] Category category, [FromServices] IOutputCacheStore cache, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting controller Categories action UpdateCategory.");

                var currentCategory = await _repo
                    .UpdateCategoryAsync(key, category);

                if (currentCategory == null)
                    return NotFound("Category not found!");

                #region Cache Evict
                await cache.EvictByTagAsync("Categories", cancellationToken);
                #endregion

                Log.Information($"Category with id: {key} has been updated.");
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
        [HttpPatch("categories({key})")]
        public async Task<IActionResult> PartUpdateCategory(int key, [FromBody] Delta<Category> category, [FromServices] IOutputCacheStore cache, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting controller Categories action PartUpdateCategory.");

                var currentCategory = await _repo
                    .PartUpdateCategoryAsync(key, category);

                if (currentCategory == null)
                    return NotFound("Category not found!");

                #region Cache Evict
                await cache.EvictByTagAsync("Categories", cancellationToken);
                #endregion

                Log.Information($"Category with id: {key} has been patched.");
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
        [HttpDelete("categories({key})")]
        public async Task<IActionResult> RemoveCategory(int key, [FromServices] IOutputCacheStore cache, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Starting controller Categories action RemoveCategory.");

                var currentCategory = await _repo
                    .RemoveCategoryAsync(key);

                if (currentCategory is false)
                    return NotFound("Category not found!");

                #region Cache Evict
                await cache.EvictByTagAsync("Categories", cancellationToken);
                #endregion

                Log.Information($"Category with id: {key} has been marked as deleted.");
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
