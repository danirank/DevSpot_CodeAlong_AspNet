namespace DevSpot_CodeAlong.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);

        //Notes: Generic Repository Pattern - 
        //A design pattern that abstracts data access logic,
        //allowing for a more flexible and reusable way to interact with different data models.
    }
}
