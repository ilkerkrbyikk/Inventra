namespace Inventra.Application.Interfaces
{
    /// <summary>
    /// Generic repository interface for data access operations.
    /// All implementations must support soft delete via global query filters.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Retrieves an entity by its ID asynchronously.
        /// </summary>
        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves entities matching a predicate asynchronously.
        /// </summary>
        Task<IEnumerable<TEntity>> GetByPredicateAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities asynchronously.
        /// </summary>
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity asynchronously.
        /// </summary>
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity (soft delete via domain logic).
        /// </summary>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes multiple entities asynchronously.
        /// </summary>
        Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if an entity exists by ID asynchronously.
        /// </summary>
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the count of all entities asynchronously.
        /// </summary>
        Task<int> CountAsync(CancellationToken cancellationToken = default);
    }
}