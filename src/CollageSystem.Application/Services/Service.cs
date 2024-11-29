using System.Linq.Expressions;

using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CollageSystem.Application.Services.Interfaces;
using CollageSystem.Core.Interfaces.Repository;
using CollageSystem.Core.Models;
using CollageSystem.Core.Results;
using CollageSystem.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CollageSystem.Application.Services;

public class Service<T, TDto>(
    ILogger<Service<T, TDto>> logger,
    IBaseRepository<T> repository,
    IMapper mapper)
    : IService<T, TDto>
    where T : BaseEntity, new()
    where TDto : class, new()
{
    private readonly ILogger<Service<T, TDto>> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IBaseRepository<T> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

    private readonly OperationResult _result =
        new OperationResult(OperationStatus.Success, new LoggerFactory().CreateLogger<OperationResult>());


    #region public methods

    public OperationResult OperationResult => _result;

    public virtual async Task<OperationResult> Create(TDto entity)
    {
        try
        {
            var result = await _add(entity);
            if (result)
                return _result.WithStatus(OperationStatus.Success);
            throw new InvalidOperationException();
        }
        catch (Exception e)
        {
            return _result.WithStatus(OperationStatus.Failure)
                .WithErrorCode(ErrorCode.CreateFailed, FailureLevel.Critical).WithException(e);
        }
    }

    public async Task<OperationResult> Update(int id, TDto entity)
    {
        try
        {
            var tEntity = await _repository.Get(id);
            if (tEntity is null)
                return _result.WithStatus(OperationStatus.Failure).WithErrorCode(ErrorCode.NotFound);

            var result = await _repository.Update(_mapper.Map(entity, tEntity));

            if (result.IsSuccess)

                return _result.WithStatus(OperationStatus.Success);

            return _result.WithStatus(OperationStatus.Failure)
                .WithErrorCode(ErrorCode.UpdateFailed);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            _result.WithStatus(OperationStatus.Failure).WithException(e);
            return _result;
        }
    }

    public async Task<OperationResult> Delete(int id)
    {
        try
        {
            await _repository.Delete(id);
            return _result.WithStatus(OperationStatus.Success);
        }
        catch (Exception e)
        {
            return _result.WithStatus(OperationStatus.Failure)
                .WithErrorCode(ErrorCode.DeleteFailed, FailureLevel.Critical).WithException(e);
        }
    }

    public async Task<TDto?> Get(int id)
    {
        var result = await _get<TDto>(id: id);
        return result != null
            ? _handleGetSuccess(result)
            : _handleGetFailure<TDto>(new ArgumentOutOfRangeException(nameof(id)));
    }

    public async Task<TDto?> Get(int id, params Expression<Func<TDto, object>>[] includes)
    {
        var result = await _get(id: id, includes: includes);
        return result != null
            ? _handleGetSuccess(result)
            : _handleGetFailure<TDto>(new ArgumentOutOfRangeException(nameof(id)));
    }

    public async Task<TDto?> Get(Expression<Func<TDto, bool>> predicate)
    {
        var result = await _get(predicate: predicate);
        return result != null ? _handleGetSuccess(result) : _handleGetFailure<TDto>(new InvalidOperationException());
    }

    public async Task<TDto?> Get(Expression<Func<TDto, bool>> predicate,
        params Expression<Func<TDto, object>>[] includes)
    {
        var result = await _get(predicate: predicate, includes);
        return result != null ? _handleGetSuccess(result) : _handleGetFailure<TDto>(new InvalidOperationException());
    }

    public async Task<IEnumerable<TDto>?> GetAll()
    {
        var result = (await _getAll<TDto>()).ToList();
        return result.Count != 0
            ? _handleGetAllSuccess(result)
            : _handleGetAllException<TDto>(new InvalidOperationException(), ErrorCode.NotFound);
    }

    public async Task<IEnumerable<TDto>?> GetAll(params Expression<Func<TDto, object>>[] includes)
    {
        var result = (await _getAll(includes: includes)).ToList();
        return result.Count != 0
            ? _handleGetAllSuccess(result)
            : _handleGetAllException<TDto>(new Exception("there is non data to be retrieve"), ErrorCode.NotFound);
    }

    public async Task<IEnumerable<TDto>?> GetAll(Expression<Func<TDto, bool>> predicate)
    {
        var result = (await _getAll(predicate: predicate)).ToList();
        return result.Count != 0
            ? _handleGetAllSuccess(result)
            : _handleGetAllException<TDto>(new InvalidOperationException(), ErrorCode.NotFound);
    }

    public async Task<IEnumerable<TDto>?> GetAll(Expression<Func<TDto, bool>> predicate,
        params Expression<Func<TDto, object>>[] includes)
    {
        var result = (await _getAll(predicate: predicate, includes: includes)).ToList();
        return result.Count != 0
            ? _handleGetAllSuccess(result)
            : _handleGetAllException<TDto>(new InvalidOperationException(), ErrorCode.NotFound);
    }

    public async Task<IEnumerable<TDto>?> GetAll(Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy)
    {
        var result = (await _getAll(orderBy: orderBy)).ToList();
        return result.Count != 0
            ? _handleGetAllSuccess(result)
            : _handleGetAllException<TDto>(new InvalidOperationException(), ErrorCode.NotFound);
    }

    public async Task<IEnumerable<TDto>?> GetAll(Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy,
        params Expression<Func<TDto, object>>[] includes)
    {
        var result = (await _getAll(orderBy: orderBy, includes: includes)).ToList();
        return result.Count != 0
            ? _handleGetAllSuccess(result)
            : _handleGetAllException<TDto>(new InvalidOperationException(), ErrorCode.NotFound);
    }

    public async Task<IEnumerable<TDto>?> GetAll(Expression<Func<TDto, bool>> predicate,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy)
    {
        var result = (await _getAll(predicate: predicate, orderBy)).ToList();
        return result.Count != 0
            ? _handleGetAllSuccess(result)
            : _handleGetAllException<TDto>(new InvalidOperationException(), ErrorCode.NotFound);
    }

    public async Task<IEnumerable<TDto>?> GetAll(Expression<Func<TDto, bool>> predicate,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy, params Expression<Func<TDto, object>>[] includes)
    {
        var result = (await _getAll(predicate, orderBy, includes)).ToList();
        return result.Count != 0
            ? _handleGetAllSuccess(result)
            : _handleGetAllException<TDto>(new InvalidOperationException(), ErrorCode.NotFound);
    }

    public async Task<PagedResult<TDto>> GetAllPaged(int pageNumber, int pageSize)
    {
        return await _getAllPaged<TDto>(pageNumber: pageNumber, pageSize: pageSize);
    }

    public async Task<PagedResult<TDto>> GetAllPaged(int pageNumber, int pageSize,
        params Expression<Func<TDto, object>>[] includes)
    {
        return await _getAllPaged(pageNumber: pageNumber, pageSize: pageSize, includes: includes);
    }

    public async Task<PagedResult<TDto>> GetAllPaged(Expression<Func<TDto, bool>> predicate, int pageNumber,
        int pageSize)
    {
        return await _getAllPaged(predicate, pageNumber: pageNumber, pageSize: pageSize);
    }

    public async Task<PagedResult<TDto>> GetAllPaged(Expression<Func<TDto, bool>> predicate, int pageNumber,
        int pageSize, params Expression<Func<TDto, object>>[] includes)
    {
        return await _getAllPaged(predicate, pageNumber: pageNumber, pageSize: pageSize, includes: includes);
    }

    public async Task<PagedResult<TDto>> GetAllPaged(int pageNumber, int pageSize,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy)
    {
        return await _getAllPaged(pageNumber: pageNumber, pageSize: pageSize, orderBy: orderBy);
    }

    public async Task<PagedResult<TDto>> GetAllPaged(int pageNumber, int pageSize,
        Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy, params Expression<Func<TDto, object>>[] includes)
    {
        return await _getAllPaged(pageNumber: pageNumber, pageSize: pageSize, orderBy: orderBy, includes: includes);
    }

    public async Task<PagedResult<TDto>> GetAllPaged(Expression<Func<TDto, bool>> predicate, int pageNumber,
        int pageSize, Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy)
    {
        return await _getAllPaged(pageNumber: pageNumber, pageSize: pageSize, predicate: predicate, orderBy: orderBy);
    }

    public async Task<PagedResult<TDto>> GetAllPaged(Expression<Func<TDto, bool>> predicate, int pageNumber,
        int pageSize, Func<IQueryable<TDto>, IOrderedQueryable<TDto>> orderBy,
        params Expression<Func<TDto, object>>[] includes)
    {
        return await _getAllPaged(pageNumber: pageNumber, pageSize: pageSize, predicate: predicate, orderBy: orderBy,
            includes: includes);
    }


    public async Task<TD?> Get<TD>(int id) where TD : TDto
    {
        try
        {
            var data = await _get<TD>(id);
            return data is null
                ? throw new ArgumentNullException
                {
                    HelpLink = null,
                    HResult = 0,
                    Source = null
                }
                : _handleGetSuccess(data);
        }
        catch (Exception e)
        {
            return _handleGetFailure<TD>(e);
        }
    }

    public async Task<TD?> Get<TD>(int id, params Expression<Func<TD, object>>[] includes) where TD : TDto
    {
        try
        {
            _result.WithStatus(OperationStatus.Success);
            return await _get(id, includes: includes);
        }
        catch (Exception e)
        {
            _logger.LogError(string.Join(",\t", _result.Errors.Select(x => x.Message)));
            return _handleGetFailure<TD>(e);
        }
    }

    public async Task<TD?> Get<TD>(Expression<Func<TD, bool>> predicate) where TD : TDto
    {
        try
        {
            return await _get(predicate: predicate);
        }
        catch (Exception e)
        {
            _logger.LogError(string.Join(",\t", _result.Errors.Select(x => x.Message)));
            return _handleGetFailure<TD>(e);
        }
    }

    public async Task<TD?> Get<TD>(Expression<Func<TD, bool>> predicate,
        params Expression<Func<TD, object>>[] includes) where TD : TDto
    {
        try
        {
            return await _get(predicate: predicate, includes: includes);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return _handleGetFailure<TD>(e);
        }
    }

    public async Task<IEnumerable<TD>?> GetAll<TD>() where TD : TDto
    {
        try
        {
            _result.WithStatus(OperationStatus.Success);
            return await _getAll<TD>();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            _result.WithStatus(OperationStatus.Failure).WithException(e);
            return null;
        }
    }


    public async Task<IEnumerable<TD>?> GetAll<TD>(params Expression<Func<TD, object>>[] includes) where TD : TDto
    {
        try
        {
            _result.WithStatus(OperationStatus.Success);
            return _mapper.Map<IEnumerable<TD>>(await _getAll(includes: includes));
        }
        catch (Exception e)
        {
            Console.WriteLine("see Log file there is an exception is happen");

            _logger.LogError($"{e.Message} at: {DateTime.UtcNow} ");

            _result.Errors.Add(new ErrorDetail(ErrorCode.InternalServerError, e.Message, FailureLevel.Critical));

            return null;
        }
    }


    public async Task<IEnumerable<TD>?> GetAll<TD>(Expression<Func<TD, bool>> predicate) where TD : TDto
    {
        try
        {
            _result.WithStatus(OperationStatus.Success);
            return _mapper.Map<IEnumerable<TD>>(await _getAll(predicate: predicate));
        }
        catch (Exception e)
        {
            Console.WriteLine("see Log file there is an exception is happen");

            _logger.LogError($"{e.Message} at: {DateTime.UtcNow} ");

            _result.Errors.Add(new ErrorDetail(ErrorCode.InternalServerError, e.Message, FailureLevel.Critical));

            return null;
        }
    }

    public async Task<IEnumerable<TD>?> GetAll<TD>(Expression<Func<TD, bool>> predicate,
        params Expression<Func<TD, object>>[] includes) where TD : TDto
    {
        try
        {
            _result.WithStatus(OperationStatus.Success);
            return await _getAll(includes: includes, predicate: predicate);
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred while retrieving entities. See the log file for more details.");

            _logger.LogError($"Error: {e.Message} at {DateTime.UtcNow}");


            _result.Errors.Add(new ErrorDetail(ErrorCode.InternalServerError, e.Message, FailureLevel.Critical));

            return null;
        }
    }

    public async Task<IEnumerable<TD>?> GetAll<TD>(Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy)
        where TD : TDto
    {
        try
        {
            return _mapper.Map<IEnumerable<TD>>(await _getAll(orderBy: orderBy));
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e.Message} at {DateTime.UtcNow}");

            _result.Errors.Add(new ErrorDetail(ErrorCode.InternalServerError, e.Message, FailureLevel.Critical));

            return null;
        }
    }


    public async Task<IEnumerable<TD>?> GetAll<TD>(Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy,
        params Expression<Func<TD, object>>[] includes) where TD : TDto
    {
        try
        {
            _result.WithStatus(OperationStatus.Success);
            return _mapper.Map<IEnumerable<TD>>(await _getAll(orderBy: orderBy, includes: includes));
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e.Message} at {DateTime.UtcNow}");
            _result.Errors.Add(new ErrorDetail(ErrorCode.InternalServerError, e.Message, FailureLevel.Critical));
            return null;
        }
    }

    public async Task<IEnumerable<TD>?> GetAll<TD>(Expression<Func<TD, bool>> predicate,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy) where TD : TDto
    {
        try
        {
            _result.WithStatus(OperationStatus.Success);
            return _mapper.Map<IEnumerable<TD>>(await _getAll(predicate: predicate, orderBy: orderBy));
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e.Message} at {DateTime.UtcNow}");

            _result.Errors.Add(new ErrorDetail(ErrorCode.InternalServerError, e.Message, FailureLevel.Critical));

            return null;
        }
    }

    public async Task<IEnumerable<TD>?> GetAll<TD>(Expression<Func<TD, bool>> predicate,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy, params Expression<Func<TD, object>>[] includes)
        where TD : TDto
    {
        try
        {
            _result.WithStatus(OperationStatus.Success);

            return await _getAll(orderBy: orderBy, includes: includes, predicate: predicate);
        }
        catch (Exception e)
        {
            _logger.LogError($"Error: {e.Message} at {DateTime.UtcNow}");
            _result.Errors.Add(new ErrorDetail(ErrorCode.InternalServerError, e.Message, FailureLevel.Important));
            return null;
        }
    }


    public async Task<PagedResult<TD>> GetAllPaged<TD>(int pageNumber, int pageSize) where TD : TDto
    {
        try
        {
            return await _getPage<TD>(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception {e.Message} at {DateTime.UtcNow}");
            return _handlePagedResultException<TD>(e, ErrorCode.ReadFailed);
        }
    }

    public async Task<PagedResult<TD>> GetAllPaged<TD>(int pageNumber, int pageSize,
        params Expression<Func<TD, object>>[] includes) where TD : TDto
    {
        try
        {
            return await _getAllPaged(pageNumber: pageNumber, pageSize: pageSize,
                includes: includes);
        }
        catch (Exception e)
        {
            return _handlePagedResultException<TD>(e, ErrorCode.ReadFailed);
        }
    }

    public async Task<PagedResult<TD>> GetAllPaged<TD>(Expression<Func<TD, bool>> predicate, int pageNumber,
        int pageSize)
        where TD : TDto
    {
        return await _getAllPaged(predicate, pageNumber, pageSize);
    }


    public async Task<PagedResult<TD>> GetAllPaged<TD>(Expression<Func<TD, bool>> predicate, int pageNumber,
        int pageSize, params Expression<Func<TD, object>>[] includes) where TD : TDto
    {
        return await _getAllPaged(predicate: predicate, pageNumber: pageNumber, pageSize: pageSize, includes: includes);
    }

    public async Task<PagedResult<TD>> GetAllPaged<TD>(int pageNumber, int pageSize,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy) where TD : TDto
    {
        return await _getAllPaged(orderBy: orderBy, pageNumber: pageNumber, pageSize: pageSize);
    }

    public async Task<PagedResult<TD>> GetAllPaged<TD>(int pageNumber, int pageSize,
        Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy, params Expression<Func<TD, object>>[] includes)
        where TD : TDto
    {
        return await _getAllPaged(orderBy: orderBy, pageNumber: pageNumber, pageSize: pageSize, includes: includes);
    }


    public async Task<PagedResult<TD>> GetAllPaged<TD>(Expression<Func<TD, bool>> predicate, int pageNumber,
        int pageSize, Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy) where TD : TDto
    {
        return await _getAllPaged(predicate: predicate, pageNumber: pageNumber, pageSize: pageSize, orderBy: orderBy);
    }

    public async Task<PagedResult<TD>> GetAllPaged<TD>(Expression<Func<TD, bool>> predicate, int pageNumber,
        int pageSize, Func<IQueryable<TD>, IOrderedQueryable<TD>> orderBy,
        params Expression<Func<TD, object>>[] includes) where TD : TDto
    {
        return await _getAllPaged(predicate: predicate, pageNumber: pageNumber, pageSize: pageSize, orderBy: orderBy,
            includes: includes);
    }

    public async Task<bool> Exists(int id)
    {
        try
        {
            await _repository.Get(id);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> Exists(Expression<Func<TDto, bool>> predicate)
    {
        return await _repository.Exists(_mapper.MapExpression<Expression<Func<T, bool>>>(predicate));
    }

    public async Task<int> Count()
    {
        return (await _repository.GetQuery().CountAsync());
    }

    public async Task<int> Count(Expression<Func<TDto, bool>> predicate)
    {
        var query = _repository.GetQuery();
        return (await query.Where(_mapper.MapExpression<Expression<Func<T, bool>>>(predicate)).CountAsync());
    }

    #endregion

    #region private implementation

    /// <summary>
    /// Adds one or more entities to the repository.
    /// </summary>
    /// <param name="entities">The entities to be added.</param>
    /// <returns>
    /// True if the entities were successfully added, false otherwise.
    /// </returns>
    /// <exception cref="Exception">Thrown if an error occurs during the addition process.</exception>
    /// <remarks>
    /// <list type="bullet">
    /// <item>
    /// <description>Single Entity Addition: When you want to add a single entity to the repository.</description>
    /// <description>Use Case: This could be used when creating a new record in the database.</description>
    /// <description>Example: If you have a TDto object representing a new customer and want to add it to the customer repository, you would call _add with an array containing that single TDto object.</description>
    /// </item>
    /// <item>
    /// <description>Multiple Entity Addition: When you want to add multiple entities to the repository at once.</description>
    /// <description>Use Case: This could be useful when importing a batch of records from a file or another source.</description>
    /// <description>Example: If you have an array of TDto objects representing new customers and want to add them all to the customer repository, you would call _add with the array of TDto objects.</description>
    /// </item>
    /// <item>
    /// <description>Error Handling: When an error occurs during the addition process.</description>
    /// <description>Use Case: This could happen due to various reasons such as database connection issues, validation errors, or unique constraint violations.</description>
    /// <description>Example: If the database is not accessible or if a unique constraint is violated while trying to add a new customer record, the _add method would catch the exception and return false.</description>
    /// </item>
    /// <item>
    /// <description>No Entities to Add: When there are no entities to add to the repository.</description>
    /// <description>Use Case: This could happen if the method is called with an empty array of TDto objects.</description>
    /// <description>Example: If you call _add with an empty array, the method would immediately return false since there are no entities to add.</description>
    /// </item>
    /// </list>
    /// </remarks>
    private async Task<bool> _add(params TDto[] entities)
    {
        try
        {
            if (entities.Length == 0)
                return false;
            if (entities.Length == 1)
                return (await _repository.Create(_mapper.Map<T>(entities[0]))).IsSuccess;
            return (await _repository.CreateRange(_mapper.Map<IEnumerable<T>>(entities))).IsSuccess;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _result.WithStatus(OperationStatus.Failure).WithErrorCode(ErrorCode.CreateFailed, FailureLevel.Critical)
                .WithException(e);
            return false;
        }
    }

    /// <summary>
    /// this is a complete version of get method that is can be used in any situation but
    /// for follow SOLID Principals ,so we separate it into smaller single responsible versions
    /// </summary>
    /// <param name="id">T key , as a T is a Derived type from baseEntity</param>
    /// <param name="predicate">an Expression to compare based on if exists</param>
    /// <param name="includes">a list of includes</param>
    /// <typeparam name="TD">Generic type</typeparam>
    /// <returns>can be any type of TD that is a Derived type of TDto</returns>
    /// <exception cref="_handleGetFailure">used this method to handle situations of all conditions is failures</exception>
    private async Task<TD?> _get<TD>(
        int id = 0,
        Expression<Func<TD, bool>>? predicate = null,
        params Expression<Func<TD, object>>[]? includes)
    {
        try
        {
            // Case 1: Fetch by ID only
            if (id != 0 && (includes == null || includes.Length == 0) && predicate == null)
            {
                return _mapper.Map<TD>(await _repository.Get(id));
            }

            // Case 2: Fetch by ID with includes
            if (id != 0 && includes is { Length: > 0 })
            {
                var mappedIncludes = _mapIncludes(includes);
                return _mapper.Map<TD>(await _repository.Get(id, mappedIncludes));
            }

            // Case 3: Fetch by predicate only
            if (predicate != null && (includes == null || includes.Length == 0))
            {
                var mappedPredicate = _mapper.MapExpression<Expression<Func<T, bool>>>(predicate);
                return _mapper.Map<TD>(await _repository.Get(mappedPredicate));
            }

            // Case 4: Fetch by predicate with includes
            if (predicate == null || includes == null || includes.Length <= 0) return default;
            {
                var mappedPredicate = _mapper.MapExpression<Expression<Func<T, bool>>>(predicate);

                var mappedIncludes = _mapIncludes(includes);

                return _mapper.Map<TD>(await _repository.Get(mappedPredicate, mappedIncludes));
            }
        }
        catch (Exception e)
        {
            return _handleGetFailure<TD>(e);
        }
    }

    private async Task<TD?> _get<TD>(int id, params Expression<Func<TD, object>>[] includes)
    {
        try
        {
            if (includes.Length == 0)
                return _mapper.Map<TD>(await _repository.Get(id));
            var mappedIncludes = _mapIncludes(includes);
            return _mapper.Map<TD>(await _repository.Get(id, mappedIncludes));
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private async Task<TD?> _get<TD>(Expression<Func<TD, bool>> predicate)
    {
        try
        {
            var mappedPredicate = _mapper.MapExpression<Expression<Func<T, bool>>>(predicate);
            return _mapper.Map<TD>(await _repository.Get(mappedPredicate));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<TD?> _get<TD>(Expression<Func<TD, bool>> predicate, Expression<Func<TD, object>>[] includes)
    {
        var mappedPredicate = _mapper.MapExpression<Expression<Func<T, bool>>>(predicate);
        var mappedIncludes = _mapIncludes(includes);
        return _mapper.Map<TD>(await _repository.Get(mappedPredicate, mappedIncludes));
    }

    private Expression<Func<T, object>>[] _mapIncludes<TD>(params Expression<Func<TD, object>>[] includes)
    {
        // return includes.Select(include => _mapper.MapExpression<Expression<Func<TD, object>>,Expression<Func<T, object>>>(include)).ToArray();
        return includes
            .Select(include => _removeConversion(_mapper.MapExpression<Expression<Func<T, object>>>(include)))
            .ToArray();
    }

    /// <summary>
    /// Removes any conversion expressions from the given expression.
    /// </summary>
    /// <typeparam name="T">The type of the expression.</typeparam>
    /// <param name="expression">The expression to remove conversions from.</param>
    /// <returns>The expression with any conversion expressions removed.</returns>
    private Expression<Func<T, object>> _removeConversion(Expression<Func<T, object>> expression)
    {
        var body = expression.Body;

        // Check if the body is a ConvertExpression
        if (body is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert)
        {
            body = unaryExpression.Operand;
        }

        // Create a new expression with the cleaned body
        var cleanExpression = Expression.Lambda<Func<T, object>>(body, expression.Parameters);
        return cleanExpression;
    }

    private TD _handleGetSuccess<TD>(TD data)
    {
        _result.WithStatus(OperationStatus.Success);
        return data;
    }

    private TD? _handleGetFailure<TD>(Exception e)
    {
        _result.WithStatus(OperationStatus.Failure)
            .WithErrorCode(ErrorCode.NotFound, FailureLevel.Important)
            .WithException(e);

        return default;
    }

    private PagedResult<TD> _handlePagedResultException<TD>(Exception e, ErrorCode errorCode)
    {
        _result.WithStatus(OperationStatus.Failure).WithErrorCode(errorCode, FailureLevel.Important).WithException(e);

        return new PagedResult<TD>
        {
            Items = null,
            PageSize = 0, CurrentPage = 0, TotalCount = 0
        };
    }

    private PagedResult<TD> _handlePageResultSuccess<TD>(PagedResult<T> tItems)
    {
        _result.WithStatus(OperationStatus.Success);
        return new PagedResult<TD>
        {
            Items = _mapper.Map<IEnumerable<TD>>(tItems.Items),
            PageSize = tItems.PageSize,
            CurrentPage = tItems.CurrentPage,
            TotalCount = tItems.TotalCount
        };
    }


    private IEnumerable<TD> _handleGetAllException<TD>(Exception e, ErrorCode errorCode)
    {
        _result.WithStatus(OperationStatus.Failure).WithErrorCode(errorCode, FailureLevel.Important).WithException(e);

        return Enumerable.Empty<TD>();
    }

    private IEnumerable<TD> _handleGetAllSuccess<TD>(IEnumerable<TD> data)
    {
        _result.WithStatus(OperationStatus.Success);
        return data;
    }


    private IQueryable<T> _applyPredicate<TD>(IQueryable<T> query, Expression<Func<TD, bool>>? predicate)
    {
        if (predicate is null) return query;

        var mappedPredicate = _mapper.MapExpression<Expression<Func<T, bool>>>(predicate);

        query = query.Where(mappedPredicate);


        return query;
    }

    private async Task<IEnumerable<TD>> _getAll<TD>(
        Expression<Func<TD, bool>>? predicate = null,
        Func<IQueryable<TD>, IOrderedQueryable<TD>>? orderBy = null,
        params Expression<Func<TD, object>>[]? includes)
    {
        // Start with the base query from the repository
        var query = _repository.GetQuery();

        // Apply the predicate
        query = _applyPredicate(query, predicate);

        // Apply the includes
        query = _applyIncludes(query, includes);

        // Fetch the data
        var result = await query.ToListAsync();

        // Map the result to TDto
        var dItems = _mapper.Map<IEnumerable<TD>>(result).AsQueryable();

        // Apply the order by on the TDto
        dItems = _applyOrderBy(dItems, orderBy);

        return dItems.ToList();
    }

    private IQueryable<T> _applyIncludes<TD>(IQueryable<T> query, Expression<Func<TD, object>>[]? includes)
    {
        if (includes == null || includes.Length <= 0) return query;

        var mappedIncludes = _mapIncludes(includes);

        query = mappedIncludes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }

    private IQueryable<TD> _applyOrderBy<TD>(IQueryable<TD> query, Func<IQueryable<TD>, IOrderedQueryable<TD>>? orderBy)
    {
        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return query;
    }

    private async Task<PagedResult<TD>> _getAllPaged<TD>(Expression<Func<TD, bool>>? predicate = null,
        int pageNumber = 1,
        int pageSize = 10, Func<IQueryable<TD>, IOrderedQueryable<TD>>? orderBy = null,
        params Expression<Func<TD, object>>?[] includes)
    {
        try
        {
            if (_isSimplePage(predicate, orderBy, includes))
            {
                return await _getPage<TD>(pageNumber, pageSize);
            }

            if (!_isSimplePage(predicate, orderBy, includes) && predicate is not null && includes.Length == 0)
            {
                return await _getPage(pageNumber, pageSize, predicate);
            }

            if (!_isSimplePage(predicate, orderBy, includes) && includes.Length > 0 && predicate is null)
            {
                return await _getPage(pageNumber, pageSize, includes);
            }

            if (!_isSimplePage(predicate, orderBy, includes) && includes.Length > 0 && predicate is not null)
            {
                return await _getPage(pageNumber, pageSize, includes, predicate);
            }

            throw new ArgumentException("Invalid combination of parameters");
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception {e.Message} at {DateTime.UtcNow}");
            throw;
        }
    }

    private async Task<PagedResult<TD>> _getPage<TD>(int pageNumber, int pageSize,
        Expression<Func<TD, object>>[] includes, Expression<Func<TD, bool>> predicate)
    {
        try
        {
            var tIncludes = _mapIncludes(includes);

            var tPredicate = _mapper.MapExpression<Expression<Func<T, bool>>>(predicate);

            var tItems = await _repository.GetAllPaged(predicate: tPredicate, pageNumber: pageNumber,
                pageSize: pageSize, includes: tIncludes);
            return tItems.Items.Any()
                ? _handlePageResultSuccess<TD>(tItems)
                : _handlePageResultFailure<TD>(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception {e.Message} at {DateTime.UtcNow}");

            return _handlePagedResultException<TD>(e, ErrorCode.ReadFailed);
        }
    }

    private async Task<PagedResult<TD>> _getPage<TD>(int pageNumber, int pageSize,
        params Expression<Func<TD, object>>[] includes)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(includes);

            var tIncludes = _mapIncludes(includes);

            var tItems = await _repository.GetAllPaged(pageNumber: pageNumber, pageSize: pageSize, includes: tIncludes);

            return tItems.Items.Any()
                ? _handlePageResultSuccess<TD>(tItems)
                : _handlePageResultFailure<TD>(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            _logger.LogError($"Exception {e.Message} at {DateTime.UtcNow}");

            return _handlePagedResultException<TD>(e, ErrorCode.ReadFailed);
        }
    }

    private async Task<PagedResult<TD>> _getPage<TD>(int pageNumber, int pageSize,
        Expression<Func<TD, bool>>? predicate)
    {
        try
        {
            var tPredicate = _mapper.MapExpression<Expression<Func<T, bool>>>(predicate);
            var tPageResult = await _repository.GetAllPaged(tPredicate, pageNumber, pageSize);
            return tPageResult.Items.Any()
                ? _handlePageResultSuccess<TD>(tPageResult)
                : _handlePageResultFailure<TD>(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message + $"\t at time {DateTime.UtcNow} ");
            return _handlePagedResultException<TD>(e, ErrorCode.ReadFailed);
        }
    }

    private async Task<PagedResult<TD>> _getPage<TD>(int pageNumber, int pageSize)
    {
        try
        {
            var pageResult = await _repository.GetAllPaged(pageNumber, pageSize);

            if (pageResult.Items.Any())

                return _handlePageResultSuccess<TD>(pageResult);

            return _handlePageResultFailure<TD>(pageResult.CurrentPage, pageResult.PageSize);
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync(e.Message);
            return _handlePagedResultException<TD>(e, ErrorCode.OperationFailed);
        }
    }

    private PagedResult<TD> _handlePageResultFailure<TD>(int currentPage, int pageSize)
    {
        _result.WithStatus(OperationStatus.Failure).WithErrorCode(ErrorCode.NotFound, FailureLevel.Important);
        return new PagedResult<TD>
        {
            Items = new List<TD>(),
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = 0
        };
    }

    private bool _isSimplePage<TD>(Expression<Func<TD, bool>>? predicate,
        Func<IQueryable<TD>, IOrderedQueryable<TD>>? orderBy, Expression<Func<TD, object>>?[] includes)
        => predicate is null && orderBy is null && includes.Length == 0;

    #endregion
}