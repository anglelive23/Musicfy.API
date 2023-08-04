namespace Musicfy.API.Entities.Repos
{
    public interface ICategoryRepository
    {
        #region Get
        IQueryable<Category> GetCategories(Expression<Func<Category, bool>> predicate);
        Task<Category?> GetCategoryByIdAsync(int id);
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
                var accounts = _context
                    .Categories
                    .Where(predicate);

                return accounts;
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
                var account = _context
                    .Categories
                    .FirstOrDefault(a => a.Id == id);

                return await Task.FromResult(account);
            }
            catch (Exception ex) when (ex is ArgumentNullException
                                    || ex is InvalidOperationException
                                    || ex is SqlException)
            {
                throw new DataFailureException(ex.Message);
            }
        }
        #endregion
    }
}
