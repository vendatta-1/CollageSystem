using System.Linq.Expressions;
using api.Helpers;
using api.Services.Implementations;
using CollageSystem.Core.Results;

using Microsoft.Extensions.Logging;


namespace CollageSystem.Application.Services.Interfaces;


/// <summary>
/// Defines a service interface for performing CRUD operations with generic and non-generic types.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TDto">The type of the Data Transfer Object (DTO).</typeparam>
public interface IService<T, TDto>

{

    OperationResult OperationResult { get; }

    #region Create

    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>An <see cref="OperationResult"/> indicating the result of the operation.</returns>
    Task<OperationResult> Create(TDto entity);

    #endregion

    #region Update

    /// <summary>
    /// Updates an existing entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <param name="entity">The updated entity data.</param>
    /// <returns>An <see cref="OperationResult"/> indicating the result of the operation.</returns>
    Task<OperationResult> Update(int id, TDto entity);

    #endregion

    #region Delete

    /// <summary>
    /// Deletes an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to delete.</param>
    /// <returns>An <see cref="OperationResult"/> indicating the result of the operation.</returns>
    Task<OperationResult> Delete(int id);

    #endregion

    #region Get Single

    /// <summary>
    /// Retrieves a single entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>The entity mapped to <see cref="TDto"/>.</returns>
    Task<TDto?> Get(int id);

    /// <summary>
    /// Retrieves a single entity by its identifier with related entities included.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>The entity mapped to <see cref="TDto"/>.</returns>
    Task<TDto?> Get(int id, params Expression<Func<TDto, object>>[] includes);

    /// <summary>
    /// Retrieves a single entity matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <returns>The entity mapped to <see cref="TDto"/>.</returns>
    Task<TDto?> Get(Expression<Func<TDto, bool>> predicate);

    /// <summary>
    /// Retrieves a single entity matching the specified predicate with related entities included.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>The entity mapped to <see cref="TDto"/>.</returns>
    Task<TDto?> Get(Expression<Func<TDto, bool>> predicate, params Expression<Func<TDto, object>>[] includes);

    #endregion

    #region Get All

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>A list of entities mapped to <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>?> GetAll();

    /// <summary>
    /// Retrieves all entities with related entities included.
    /// </summary>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A list of entities mapped to <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>?> GetAll(params Expression<Func<TDto, object>>[] includes);

    /// <summary>
    /// Retrieves all entities matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <returns>A list of entities mapped to <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>?> GetAll(Expression<Func<TDto, bool>> predicate);

    /// <summary>
    /// Retrieves all entities matching the specified predicate with related entities included.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A list of entities mapped to <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>?> GetAll(Expression<Func<TDto, bool>> predicate,
        params Expression<Func<TDto, object>>[] includes);

    /// <summary>
    /// Retrieves all entities with sorting.
    /// </summary>
    /// <param name="orderBy">The sorting expression.</param>
    /// <returns>A sorted list of entities mapped to <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>?> GetAll(Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy);

    /// <summary>
    /// Retrieves all entities with sorting and related entities included.
    /// </summary>
    /// <param name="orderBy">The sorting expression.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A sorted list of entities mapped to <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>?> GetAll(Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy,
        params Expression<Func<TDto, object>>[] includes);

    /// <summary>
    /// Retrieves all entities matching the specified predicate with sorting.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <returns>A sorted list of entities mapped to <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>?> GetAll(Expression<Func<TDto, bool>> predicate,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy);

    /// <summary>
    /// Retrieves all entities matching the specified predicate with sorting and related entities included.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A sorted list of entities mapped to <see cref="TDto"/>.</returns>
    Task<IEnumerable<TDto>?> GetAll(Expression<Func<TDto, bool>> predicate,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy, params Expression<Func<TDto, object>>[] includes);

    #endregion

    #region Get All with Pagination

    /// <summary>
    /// Retrieves a paged list of entities.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged result containing entities mapped to <see cref="TDto"/>.</returns>
    Task<PagedResult<TDto>> GetAllPaged(int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a paged list of entities with related entities included.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result containing entities mapped to <see cref="TDto"/>.</returns>
    Task<PagedResult<TDto>> GetAllPaged(int pageNumber, int pageSize, params Expression<Func<TDto, object>>[] includes);

    /// <summary>
    /// Retrieves a paged list of entities matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged result containing entities mapped to <see cref="TDto"/>.</returns>
    Task<PagedResult<TDto>> GetAllPaged(Expression<Func<TDto, bool>> predicate, int pageNumber, int pageSize);

    /// <summary>
    /// Retrieves a paged list of entities matching the specified predicate with related entities included.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result containing entities mapped to <see cref="TDto"/>.</returns>
    Task<PagedResult<TDto>> GetAllPaged(Expression<Func<TDto, bool>> predicate, int pageNumber, int pageSize,
        params Expression<Func<TDto, object>>[] includes);

    /// <summary>
    /// Retrieves a paged and sorted list of entities.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <returns>A paged result containing sorted entities mapped to <see cref="TDto"/>.</returns>
    Task<PagedResult<TDto>> GetAllPaged(int pageNumber, int pageSize,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy);

    /// <summary>
    /// Retrieves a paged and sorted list of entities with related entities included.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result containing sorted entities mapped to <see cref="TDto"/>.</returns>
    Task<PagedResult<TDto>> GetAllPaged(int pageNumber, int pageSize,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy, params Expression<Func<TDto, object>>[] includes);

    /// <summary>
    /// Retrieves a paged and sorted list of entities matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <returns>A paged result containing sorted entities mapped to <see cref="TDto"/>.</returns>
    Task<PagedResult<TDto>> GetAllPaged(Expression<Func<TDto, bool>> predicate, int pageNumber, int pageSize,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy);

    /// <summary>
    /// Retrieves a paged and sorted list of entities matching the specified predicate with related entities included.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result containing sorted entities mapped to <see cref="TDto"/>.</returns>
    Task<PagedResult<TDto>> GetAllPaged(Expression<Func<TDto, bool>> predicate, int pageNumber, int pageSize,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy, params Expression<Func<TDto, object>>[] includes);

    #endregion


    #region Generic Methods for TD

    /// <summary>
    /// Retrieves a single entity mapped to <see cref="TD"/> by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <returns>The entity mapped to <see cref="TD"/>.</returns>
    Task<TD?> Get<TD>(int id) where TD : TDto;

    /// <summary>
    /// Retrieves a single entity mapped to <see cref="TD"/> by its identifier with related entities included.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>The entity mapped to <see cref="TD"/>.</returns>
    Task<TD?> Get<TD>(int id, params Expression<Func<TD, object>>[] includes) where TD : TDto;

    /// <summary>
    /// Retrieves a single entity mapped to <see cref="TD"/> matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <returns>The entity mapped to <see cref="TD"/>.</returns>
    Task<TD?> Get<TD>(Expression<Func<TD, bool>> predicate) where TD : TDto;

    /// <summary>
    /// Retrieves a single entity mapped to <see cref="TD"/> matching the specified predicate with related entities included.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>The entity mapped to <see cref="TD"/>.</returns>
    Task<TD?> Get<TD>(Expression<Func<TD, bool>> predicate, params Expression<Func<TD, object>>[] includes)
        where TD : TDto;

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="TD"/> from the repository.
    /// </summary>
    /// <typeparam name="TD">The type of the entities to retrieve.</typeparam>
    /// <returns>
    /// An asynchronous task that returns an <see cref="IEnumerable{TD}"/> containing all entities of type <typeparamref name="TD"/>,
    /// or null if an error occurs.
    /// </returns>
    /// <remarks>
    /// This method retrieves all entities of type <typeparamref name="TD"/> from the repository.
    /// It uses the private method in<see cref="api.Services.Implementations.Service{T,TDto}"/>named as _getAll() to perform the actual retrieval.
    /// </remarks>
    public Task<IEnumerable<TD>?> GetAll<TD>() where TD : TDto;


    /// <summary>
    /// Retrieves all entities of type <typeparamref name="TD"/> from the repository.
    /// </summary>
    /// <typeparam name="TD">The type of the entities to retrieve.</typeparam>
    /// <param name="includes">An optional array of expressions representing the properties to include in the query.</param>
    /// <returns>
    /// An asynchronous task that returns an <see cref="IEnumerable{TD}"/> containing all entities of type <typeparamref name="TD"/>,
    /// or null if an error occurs.
    /// </returns>
    /// <remarks>
    /// This method retrieves all entities of type <typeparamref name="TD"/> from the repository, applying any specified includes.
    /// If no includes are specified, the method retrieves all properties of the entities.
    /// </remarks>
    public Task<IEnumerable<TD>?> GetAll<TD>(params Expression<Func<TD, object>>[] includes) where TD : TDto;

    /// <summary>
    /// Retrieves a collection of entities from the repository based on a given predicate.
    /// </summary>
    /// <typeparam name="TD">The type of the entity to retrieve.</typeparam>
    /// <param name="predicate">The condition to filter the entities by.</param>
    /// <returns>
    /// An asynchronous task that returns a collection of entities of type TD, or null if an error occurs.
    /// The returned collection is mapped from the TDto type to the TD type.
    /// </returns>
    /// <exception cref="Exception">Thrown if an error occurs during the retrieval process.</exception>
    public Task<IEnumerable<TD>?> GetAll<TD>(Expression<Func<TD, bool>> predicate) where TD : TDto;


    /// <summary>
    /// Retrieves all entities from the repository that satisfy the given predicate, 
    /// and includes the specified related entities.
    /// </summary>
    /// <typeparam name="TD">The type of the entities to retrieve.</typeparam>
    /// <param name="predicate">
    /// The predicate to filter the entities by. This is an expression that returns a boolean value.
    /// The entities that satisfy this predicate will be retrieved.
    /// </param>
    /// <param name="includes">
    /// The related entities to include in the result. These are specified as expressions that return related entities.
    /// </param>
    /// <returns>
    /// An asynchronous task that returns an enumeration of the retrieved entities, 
    /// or null if an error occurs.
    /// </returns>
    /// <exception cref="Exception">
    /// This method may throw an exception if an error occurs during the retrieval process.
    /// The exception will contain details about the error.
    /// </exception>
    public Task<IEnumerable<TD>?> GetAll<TD>(Expression<Func<TD, bool>> predicate,
        params Expression<Func<TD, object>>[] includes) where TD : TDto;


    /// <summary>
    /// Retrieves all entities from the repository, ordered by the provided expression.
    /// </summary>
    /// <typeparam name="TD">The type of the DTO.</typeparam>
    /// <param name="orderBy">The expression to order the entities by.</param>
    /// <returns>
    /// An asynchronous task that returns an enumeration of the requested entities, or null if an error occurs.
    /// </returns>
    /// <exception cref="Exception">Thrown if an error occurs during the retrieval process.</exception>
    public Task<IEnumerable<TD>?> GetAll<TD>(Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy)
        where TD : TDto;

    /// <summary>
    /// Retrieves all entities from the repository, ordered and including specified related entities.
    /// </summary>
    /// <typeparam name="TD">The type of the entities to retrieve.</typeparam>
    /// <param name="orderBy">A function to order the entities.</param>
    /// <param name="includes">An array of expressions representing the related entities to include.</param>
    /// <returns>
    /// An asynchronous task that returns an enumeration of the retrieved entities, or null if an error occurs.
    /// </returns>
    /// <exception cref="Exception">Thrown if an error occurs during the retrieval process.</exception>
    public Task<IEnumerable<TD>?> GetAll<TD>(Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy,
        params Expression<Func<TD, object>>[] includes) where TD : TDto;

    /// <summary>
    /// Retrieves all entities from the repository that satisfy the given predicate, ordered by the given expression.
    /// </summary>
    /// <typeparam name="TD">The type of the DTO (Data Transfer Object) that the entities belong to.</typeparam>
    /// <param name="predicate">An expression that defines the condition for filtering entities. Only entities that satisfy this condition will be retrieved.</param>
    /// <param name="orderBy">An expression that defines the sorting order for the retrieved entities. The entities will be ordered according to this expression.</param>
    /// <returns>
    /// An asynchronous task that returns an enumeration of the requested entities. If an error occurs during the retrieval process, the task will return null.
    /// </returns>
    /// <exception cref="Exception">Thrown if an error occurs during the retrieval process. The exception will contain details about the error.</exception>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <description>This method is useful when you want to retrieve a specific set of entities from the repository, sorted in a particular order.</description>
    /// </item>
    /// <item>
    /// <description>The <paramref name="predicate"/> parameter is used to filter the entities based on a condition. Only entities that satisfy this condition will be retrieved.</description>
    /// </item>
    /// <item>
    /// <description>The <paramref name="orderBy"/> parameter is used to define the sorting order for the retrieved entities. The entities will be ordered according to this expression.</description>
    /// </item>
    /// <item>
    /// <description>If an error occurs during the retrieval process, the method will catch the exception and log the error details using the <see cref="Logger{T}"/> class. It will then add an error detail to the <see cref="OperationResult"/> object and return null.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public Task<IEnumerable<TD>?> GetAll<TD>(Expression<Func<TD, bool>> predicate,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy) where TD : TDto;

    /// <summary>
    /// Retrieves all entities from the repository that satisfy the given predicate, ordered by the provided expression, and includes the specified related entities.
    /// </summary>
    /// <typeparam name="TD">The type of the DTO (Data Transfer Object).</typeparam>
    /// <param name="predicate">An expression that defines the condition for filtering the entities. This parameter is optional and can be null if no filtering is required.</param>
    /// <param name="orderBy">An expression that defines the sorting order of the entities. This parameter is optional and can be null if no sorting is required.</param>
    /// <param name="includes">An array of expressions that define the related entities to be included in the result. This parameter is optional and can be an empty array if no related entities need to be included.</param>
    /// <returns>
    /// An asynchronous task that returns an enumeration of entities of type TD, or null if an error occurs.
    /// The returned enumeration is wrapped in a nullable type to indicate that it can be null if an error occurs.
    /// </returns>
    /// <exception cref="Exception">
    /// This method catches any exceptions that occur during the retrieval process and logs them using the _logger.
    /// It also adds an error detail to the _result object.
    /// The exception is caught and handled within the method, and does not propagate beyond the method's scope.
    /// </exception>
    public Task<IEnumerable<TD>?> GetAll<TD>(Expression<Func<TD, bool>> predicate,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy, params Expression<Func<TD, object>>[] includes)
        where TD : TDto;


    /// <summary>
    /// Retrieves a paged list of entities mapped to <see cref="TD"/>.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged result containing entities mapped to <see cref="TD"/>.</returns>
    Task<PagedResult<TD>> GetAllPaged<TD>(int pageNumber, int pageSize) where TD : TDto;

    /// <summary>
    /// Retrieves a paged list of entities mapped to <see cref="TD"/> with related entities included.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result containing entities mapped to <see cref="TD"/>.</returns>
    Task<PagedResult<TD>> GetAllPaged<TD>(int pageNumber, int pageSize, params Expression<Func<TD, object>>[] includes)
        where TD : TDto;

    /// <summary>
    /// Retrieves a paged list of entities mapped to <see cref="TD"/> matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paged result containing entities mapped to <see cref="TD"/>.</returns>
    Task<PagedResult<TD>> GetAllPaged<TD>(Expression<Func<TD, bool>> predicate, int pageNumber, int pageSize)
        where TD : TDto;

    /// <summary>
    /// Retrieves a paged list of entities mapped to <see cref="TD"/> matching the specified predicate with related entities included.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result containing entities mapped to <see cref="TD"/>.</returns>
    Task<PagedResult<TD>> GetAllPaged<TD>(Expression<Func<TD, bool>> predicate, int pageNumber, int pageSize,
        params Expression<Func<TD, object>>[] includes) where TD : TDto;

    /// <summary>
    /// Retrieves a paged and sorted list of entities mapped to <see cref="TD"/>.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <returns>A paged result containing sorted entities mapped to <see cref="TD"/>.</returns>
    Task<PagedResult<TD>> GetAllPaged<TD>(int pageNumber, int pageSize,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy) where TD : TDto;

    /// <summary>
    /// Retrieves a paged and sorted list of entities mapped to <see cref="TD"/> with related entities included.
    /// </summary>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result containing sorted entities mapped to <see cref="TD"/>.</returns>
    Task<PagedResult<TD>> GetAllPaged<TD>(int pageNumber, int pageSize,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy, params Expression<Func<TD, object>>[] includes)
        where TD : TDto;

    /// <summary>
    /// Retrieves a paged and sorted list of entities mapped to <see cref="TD"/> matching the specified predicate.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <returns>A paged result containing sorted entities mapped to <see cref="TD"/>.</returns>
    Task<PagedResult<TD>> GetAllPaged<TD>(Expression<Func<TD, bool>> predicate, int pageNumber, int pageSize,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy) where TD : TDto;

    /// <summary>
    /// Retrieves a paged and sorted list of entities mapped to <see cref="TD"/> matching the specified predicate with related entities included.
    /// </summary>
    /// <param name="predicate">The condition to match.</param>
    /// <param name="pageNumber">The page number (starting from 1).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="orderBy">The sorting expression.</param>
    /// <param name="includes">The related entities to include.</param>
    /// <returns>A paged result containing sorted entities mapped to <see cref="TD"/>.</returns>
    Task<PagedResult<TD>> GetAllPaged<TD>(Expression<Func<TD, bool>> predicate, int pageNumber, int pageSize,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy, params Expression<Func<TD, object>>[] includes)
        where TD : TDto;

    #endregion

    #region Additional Methods

    /// <summary>
    /// Gets the count of entities that match is exists in the database of this entity.
    /// </summary>
    /// 
    /// <returns>The count of entities that match the given predicate.</returns>
    Task<int> Count();

    /// <summary>
    /// Gets the count of entities that match the given predicate.
    /// </summary>
    /// <param name="predicate">The predicate to filter the entities by.</param>
    /// <returns>The count of entities that match the given predicate.</returns>
    Task<int> Count(Expression<Func<TDto, bool>> predicate);

    /// <summary>
    /// Checks if an entity exists in the repository based on a given predicate.
    /// </summary>
    /// <param name="predicate">The predicate to use for checking the existence of the entity.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The result is a boolean value indicating whether an entity exists in the repository based on the given predicate.
    /// </returns>
    /// <remarks>
    /// This method maps the given predicate from the TDto type to the T type using the mapper, and then checks for the existence of the entity in the repository based on the mapped predicate.
    /// </remarks>
    Task<bool> Exists(Expression<Func<TDto, bool>> predicate);

    /// <summary>
    /// Counts the total number of entities in the repository.
    /// </summary>
    /// <returns>
    /// An integer representing the total count of entities in the repository.
    /// </returns>
    Task<bool> Exists(int id);

    #endregion
}