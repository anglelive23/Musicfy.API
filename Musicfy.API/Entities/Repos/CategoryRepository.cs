namespace Musicfy.API.Entities.Repos
{
    public interface ICategoryRepository
    {
        #region Get
        IQueryable<Category> GetCategories(Expression<Func<Category, bool>> predicate);
        Task<Category?> GetCategoryByIdAsync(int id);
        #endregion

        #region Post
        Task<Category?> AddCategoryAsync(Category category);
        #endregion

        #region Put
        Task<Category?> UpdateCategoryAsync(int id, Category category);
        #endregion

        #region Patch
        Task<Category?> PartUpdateCategoryAsync(int id, Delta<Category> category);
        #endregion

        #region Delete
        Task<bool> RemoveCategoryAsync(int id);
        #endregion
    }
    public class CategoryRepository : ICategoryRepository
    {
        #region Fields and properties
        private readonly MusicfyContext _context;
        #endregion

        #region Constructors
        public CategoryRepository(MusicfyContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        #endregion

        #region Get
        public IQueryable<Category> GetCategories(Expression<Func<Category, bool>> predicate)
        {
            try
            {
                var categories = _context
                    .Categories
                    .Where(predicate);

                return categories;
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is InvalidOperationException
                                    || ex is SqlException)
            {
                throw new DataFailureException(ex.Message);
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await _context
                    .Categories
                    .FirstOrDefaultAsync(c => c.Id == id);

                return category;
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
        public async Task<Category?> AddCategoryAsync(Category category)
        {
            try
            {
                if (IsExistingCategory(category.Name))
                    return null;

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return category;
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
        public async Task<Category?> UpdateCategoryAsync(int id, Category category)
        {
            try
            {
                var currentCategory = await GetCategoryByIdAsync(id);

                if (currentCategory == null)
                    return null;

                category.Id = currentCategory.Id;
                _context.Entry(currentCategory).CurrentValues.SetValues(category);
                _context.Update(currentCategory);
                await _context.SaveChangesAsync();

                return currentCategory;
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
        public async Task<Category?> PartUpdateCategoryAsync(int id, Delta<Category> category)
        {
            try
            {
                var currentCategory = await GetCategoryByIdAsync(id);

                if (currentCategory == null)
                    return null;

                category.Patch(currentCategory);
                _context.Update(currentCategory);
                await _context.SaveChangesAsync();

                return currentCategory;
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
        public async Task<bool> RemoveCategoryAsync(int id)
        {
            try
            {
                var category = await GetCategoryByIdAsync(id);

                if (category == null)
                    return false;

                category.IsDeleted = true;
                _context.Update(category);

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
        private bool IsExistingCategory(string name)
        {
            return _context
                .Categories
                .Any(a => a.Name == name && a.IsDeleted == false);
        }
        #endregion
    }
}
