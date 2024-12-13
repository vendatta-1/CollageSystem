using System.Linq.Expressions;
using CollageSystem.Core.Models;
using CollageSystem.Core.Results;


namespace CollageSystem.Core.Interfaces.Repository
{
    /// <summary>
    /// Defines a generic repository interface for basic CRUD operations and querying.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {
        #region Create

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<OperationResult> Create(T entity);

        /// <summary>
        /// Creates multiple entities in the database.
        /// </summary>
        /// <param name="entities">The collection of entities to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<OperationResult> CreateRange(IEnumerable<T> entities);

        #endregion

        #region Read

        /// <summary>
        /// Retrieves all entities from the database.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
        Task<IEnumerable<T>?> GetAll();

        /// <summary>
        /// Retrieves all entities matching the specified predicate.
        /// </summary>
        /// <param name="predicate">The condition to filter the entities.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
        Task<IEnumerable<T>?> GetAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves all entities with includes if exists 
        /// </summary>
        /// 
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
        Task<IEnumerable<T>?> GetAll(params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// Retrieves all entities matching the specified predicate includes if exists 
        /// </summary>
        /// <param name="predicate">The condition to filter the entities.</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, containing a collection of entities.</returns>
        Task<IEnumerable<T>?> GetAll(Expression<Func<T, bool>>? predicate,
            params Expression<Func<T, object>>[]? includes);


        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing the entity.</returns>
        Task<T?> Get(int id);

        /// <summary>
        /// Retrieves a single entity matching the specified predicate.
        /// </summary>
        /// <param name="predicate">The condition to filter the entity.</param>
        /// <returns>A task representing the asynchronous operation, containing the entity.</returns>
        Task<T?> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves a single entity by its ID, including related entities specified by the include paths.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, containing the entity.</returns>
        Task<T?> Get(int id, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// this method is used to retrieve a single entity based on predicate and include if needed to add more flexibility
        /// </summary>
        /// <param name="predicate">The predicate is will used to be matched to search in db</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, containing the entity.</returns>
        Task<T?> Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        /// <summary>
        /// return a query if needed in service operations take care when use it 
        /// </summary>
        /// <returns></returns>
        [Obsolete("Take care when using this service ")]
        IQueryable<T> GetQuery();

        /// <summary>
        /// Checks if any entity exists matching the specified predicate.
        /// </summary>
        /// <param name="predicate">The condition to check.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean value.</returns>
        Task<bool> Exists(Expression<Func<T, bool>> predicate);

        #endregion

        #region Update

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<OperationResult> Update(T entity);

        /// <summary>
        /// Updates multiple entities in the database.
        /// </summary>
        /// <param name="entities">The collection of entities to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<OperationResult> UpdateRange(IEnumerable<T> entities);

        #endregion

        #region Delete

        /// <summary>
        /// Deletes an entity from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean value indicating success.</returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Deletes a specified entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean value indicating success.</returns>
        Task<bool> Delete(T entity);

        /// <summary>
        /// Deletes multiple entities from the database.
        /// </summary>
        /// <param name="entities">The collection of entities to delete.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean value indicating success.</returns>
        Task<bool> DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes entities matching the specified predicate.
        /// </summary>
        /// <param name="predicate">The condition to filter the entities to delete.</param>
        /// <returns>A task representing the asynchronous operation, containing a boolean value indicating success.</returns>
        Task<bool> Delete(Expression<Func<T, bool>> predicate);

        #endregion

        #region Paging

        /// <summary>
        /// Retrieves a paged list of entities from the database.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of entities per page.</param>
        /// <returns>A task representing the asynchronous operation, containing a paged result of entities.</returns>
        Task<PagedResult<T>> GetAllPaged(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves a paged list of entities from the database, including related entities specified by the include paths.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of entities per page.</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, containing a paged result of entities.</returns>
        Task<PagedResult<T>> GetAllPaged(int pageNumber, int pageSize, params Expression<Func<T, object>>[]? includes);

        /// <summary>
        /// Retrieves a paged list of entities matching the specified predicate from the database.
        /// </summary>
        /// <param name="predicate">The condition to filter the entities.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of entities per page.</param>
        /// <returns>A task representing the asynchronous operation, containing a paged result of entities.</returns>
        Task<PagedResult<T>> GetAllPaged(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves a paged list of entities matching the specified predicate from the database, including related entities specified by the include paths.
        /// </summary>
        /// <param name="predicate">The condition to filter the entities.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of entities per page.</param>
        /// <param name="includes">The related entities to include in the query.</param>
        /// <returns>A task representing the asynchronous operation, containing a paged result of entities.</returns>
        Task<PagedResult<T>> GetAllPaged(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize,
            params Expression<Func<T, object>>[]? includes);

        #endregion
    }
}