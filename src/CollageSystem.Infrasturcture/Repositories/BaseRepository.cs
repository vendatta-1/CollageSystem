
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CollageSystem.Core.Interfaces.Repository;
using CollageSystem.Core.Models;
using CollageSystem.Core.Validation;
using CollageSystem.Data.Contexts;
using Microsoft.Extensions.Logging;
using CollageSystem.Core.Results;

namespace CollageSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Provides a base implementation for repository operations, including create, read, update, and delete (CRUD) operations.
    /// </summary>
    /// <typeparam name="T">The type of the entity, which must be a subclass of <see cref="BaseEntity"/>.</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity, new()
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        private readonly ILogger<T> _logger;
        private readonly OperationResult _result;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The <see cref="ApplicationDbContext"/> instance.</param>
        /// <param name="logger">The <see>
        ///         <cref>ILogger{BaseRepository{T}}</cref>
        ///     </see>
        ///     instance for logging.</param>
        /// <param name="operationLogger">The <see cref="ILogger{OperationResult}"/> instance for logging operation results.</param>
        public BaseRepository(ApplicationDbContext context, ILogger<T> logger, ILogger<OperationResult> operationLogger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbSet = _context.Set<T>();
            _result = new OperationResult(OperationStatus.Pending, operationLogger);
        }

        #region Create Methods

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the result of the operation.</returns>
        public virtual async Task<OperationResult> Create(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                return await AddEntityAsync(entity);
            }
            catch (Exception ex)
            {
                return HandleException(ErrorCode.CreateFailed, ex);
            }
        }

        /// <summary>
        /// Creates a range of new entities in the database.
        /// </summary>
        /// <param name="entities">The entities to create.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the result of the operation.</returns>
        public virtual async Task<OperationResult> CreateRange(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                return await AddEntitiesAsync(entities);
            }
            catch (Exception ex)
            {
                return HandleException(ErrorCode.CreateFailed, ex);
            }
        }

        #endregion

        #region Update Methods

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the result of the operation.</returns>
        public async Task<OperationResult> Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                return await UpdateEntityAsync(entity);
            }
            catch (Exception ex)
            {
                return HandleException(ErrorCode.UpdateFailed, ex);
            }
        }

        /// <summary>
        /// Updates a range of entities in the database.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the result of the operation.</returns>
        public async Task<OperationResult> UpdateRange(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                return await UpdateEntitiesAsync(entities);
            }
            catch (Exception ex)
            {
                return HandleException(ErrorCode.UpdateFailed, ex);
            }
        }

        #endregion

        #region Delete Methods

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A <see cref="Task{Boolean}"/> indicating whether the entity was deleted.</returns>
        public async Task<bool> Delete(int id)
        {
            try
            {
                return await DeleteByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity by ID.");
                throw;
            }
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A <see cref="Task{Boolean}"/> indicating whether the entity was deleted.</returns>
        public async Task<bool> Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                return await DeleteEntityAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity.");
                throw;
            }
        }

        /// <summary>
        /// Deletes a range of entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <returns>an <see cref="Task{Boolean}"/> indicating whether the entities were deleted.</returns>
        public async Task<bool> DeleteRange(IEnumerable<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            try
            {
                return await DeleteEntitiesAsync(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entities.");
                return false;
            }
        }

        /// <summary>
        /// Deletes entities that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>an <see cref="Task{Boolean}"/> indicating whether any entities were deleted.</returns>
        public async Task<bool> Delete(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            try
            {
                return await DeleteByPredicateAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entities by predicate.");
                return false;
            }
        }

        #endregion

        #region Read Methods

        /// <summary>
        /// Retrieves an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A <see cref="Task{T}"/> containing the entity.</returns>
        public async Task<T?> Get(int id)
        {
            try
            {
                return await _getById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entity by ID.");
                return null;
            }
        }

        /// <summary>
        /// Retrieves an entity that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A <see cref="Task{T}"/> containing the entity.</returns>
        public async Task<T?> Get(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            try
            {
                return await _getByPredicate(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entity by predicate.");
                return null;
            }
        }

        /// <summary>
        /// Retrieves an entity by its ID with included related entities.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <param name="includes">The related entities to include.</param>
        /// <returns>A <see cref="Task{T}"/> containing the entity.</returns>
        public async Task<T?> Get(int id, params Expression<Func<T, object>>[] includes)
        {
            if (id <= 0) throw new ArgumentException("Invalid ID", nameof(id));

            try
            {
                var result = await _getByIdWithIncludes(id, includes);
                if (result == null) throw new ArgumentException("Invalid ID", nameof(id));
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entity by ID with includes.");
                return null;
            }
        }

        public async Task<T?> Get(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes)
        {
            var query = _dbSet.AsQueryable();
            if (includes != null && includes.Any())
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// returns a query  of type T to use it in some operations in service if needed 
        /// </summary>
        /// <returns>Dbset As Query</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IQueryable<T> GetQuery()
        {
            try
            {
                return _dbSet.AsQueryable();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Checks whether an entity exists that matches the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>an <see cref="Task{Boolean}"/> indicating whether the entity exists.</returns>
        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return await _isEntityExists(predicate);
        }

        /// <summary>
        /// Retrieves all entities.
        /// </summary>
        /// <returns>A <see>
        ///         <cref>Task{IEnumerable{T}}</cref>
        ///     </see>
        ///     containing all entities.</returns>
        public async Task<IEnumerable<T>?> GetAll()
        {
            try
            {
                return await _getAllEntities();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all entities.");
                return Enumerable.Empty<T>();
            }
        }

        public async Task<IEnumerable<T>?> GetAll(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[]? includes)
        {
            try
            {
                return await _getAllEntities(predicate, includes);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting all entities with predicate.");
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Retrieves all entities with included related entities.
        /// </summary>
        /// <param name="includes">The related entities to include.</param>
        /// <returns>A <see>
        ///         <cref>Task{IEnumerable{T}}</cref>
        ///     </see>
        ///     containing all entities.</returns>
        public async Task<IEnumerable<T>?> GetAll(params Expression<Func<T, object>>[] includes)
        {
            try
            {
                return await _getAllEntities(includes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all entities with includes.");
                return Enumerable.Empty<T>();
            }
        }

        public async Task<IEnumerable<T>?> GetAll(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await ExecuteQueryWithPredicate(predicate).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting all entities with predicate.");
                return Enumerable.Empty<T>();
            }
        }

        public async Task<PagedResult<T>> GetAllPaged(int pageNumber, int pageSize)
        {
            return await GetPagedResult(pageNumber, pageSize);
        }

        public async Task<PagedResult<T>> GetAllPaged(int pageNumber, int pageSize,
            params Expression<Func<T, object>>[]? includes)
        {
            return await GetPagedResult(pageNumber, pageSize, includes);
        }

        public async Task<PagedResult<T>> GetAllPaged(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize)
        {
            return await GetPagedResult(pageNumber, pageSize, predicate: predicate);
        }

        public async Task<PagedResult<T>> GetAllPaged(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize,
            params Expression<Func<T, object
            >>[]? includes)
        {
            return await GetPagedResult(pageNumber, pageSize, includes, predicate);
        }

        #endregion

        #region Private Helper Methods

        private IQueryable<T> ExecuteQueryWithPredicate(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        private async Task<PagedResult<T>> GetPagedResult(int pageNumber, int pageSize,
            Expression<Func<T, object>>[]? includes = null, Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            var totalItems = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalItems,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        private async Task<OperationResult> AddEntityAsync(T entity)
        {
            await _dbSet.AddAsync(entity);

            await _context.SaveChangesAsync();

            return _result.WithStatus(OperationStatus.Success);
        }

        private async Task<OperationResult> AddEntitiesAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            await _context.SaveChangesAsync();
            return _result.WithStatus(OperationStatus.Success);
        }

        private async Task<OperationResult> UpdateEntityAsync(T entity)
        {
            try
            {
                var local = _dbSet.Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                }

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return _result.WithStatus(OperationStatus.Success);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return _result.WithStatus(OperationStatus.Failure).WithException(e);
            }
        }

        private async Task<OperationResult> UpdateEntitiesAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync();
            return _result.WithStatus(OperationStatus.Success);
        }

        private async Task<bool> DeleteByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> DeleteEntityAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> DeleteEntitiesAsync(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> DeleteByPredicateAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await _dbSet.Where(predicate).ToListAsync();
            if (!entities.Any()) return false;

            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<T> _getById(int id)
        {
            _dbSet.AsNoTracking();
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id) ??
                   throw new Exception($"Could not find {id} at time {DateTime.Now}");
        }

        private async Task<T> _getByPredicate(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate) ??
                   throw new Exception($"there is non matching with predicate: {predicate}");
        }

        private async Task<T?> _getByIdWithIncludes(int id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        private async Task<bool> _isEntityExists(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        private async Task<IEnumerable<T>> _getAllEntities()
        {
            return await _dbSet.ToListAsync();
        }

        private async Task<IEnumerable<T>> _getAllEntities(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        private async Task<IEnumerable<T>> _getAllEntities(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[]? includes)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                return await query.Where(predicate).ToListAsync();
            }
            catch (Exception e)
            {
                HandleException(ErrorCode.OperationFailed, e);
                throw;
            }
        }

        private OperationResult HandleException(ErrorCode errorCode, Exception ex)
        {
            _logger.LogError(ex, $"Operation failed with error code: {errorCode}");
            return _result.WithStatus(OperationStatus.Failure).WithErrorCode(errorCode, FailureLevel.Critical)
                .WithException(ex);
        }

        #endregion
    }
}