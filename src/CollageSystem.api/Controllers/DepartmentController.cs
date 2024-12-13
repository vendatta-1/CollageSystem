using System.Linq.Expressions;
using api.DTOs.Department;
using api.Helpers;
using api.Models.SecurityModels;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AppPolicies.AdminOnly)]
public class DepartmentController : ControllerBase
{
    #region Constructor

    public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    #endregion

    #region Private Fields

    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentController> _logger;

    #endregion

    #region Public Actions

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll(string? query, [FromQuery] params string[] includes)
    {
        try
        {
            var departments = await _getDepartments(query, includes);
            return _generateResponse(departments);
        }
        catch (Exception ex)
        {
            return _handleException(ex, "Error getting departments");
        }
    }

    [HttpGet("[action]/{id:int}")]
    public async Task<IActionResult> Get(int id, [FromQuery] params string[] includes)
    {
        try
        {
            var department = await _getDepartmentById(id, includes);
            return _generateResponse(department);
        }
        catch (Exception ex)
        {
            return _handleException(ex, "Error getting department by ID");
        }
    }

    [HttpGet("[action]Query")]
    public async Task<IActionResult> Get(string query, params string[] includes)
    {
        try
        {
            var department = await _getDepartmentByQuery(query, includes);
            return _generateResponse(department);
        }
        catch (Exception ex)
        {
            return _handleException(ex, "Error getting department by query");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartmentBaseDto department)
    {
        try
        {
            var createdDepartment = await _departmentService.Create(department);
            return _generateResponse(createdDepartment, nameof(Get), department.Name);
        }
        catch (Exception ex)
        {
            return _handleException(ex, "Error creating department");
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] DepartmentUpdateDto department)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedDepartment = await _departmentService.Update(department.Id, department);
            return _generateResponse(updatedDepartment);
        }
        catch (Exception ex)
        {
            return _handleException(ex, "Error updating department");
        }
    }

    #endregion

    #region Private Methods

    private async Task<IEnumerable<DepartmentResponseDto>> _getDepartments(string? query, string[] includes)
    {
        if (string.IsNullOrEmpty(query) && includes.Length == 0)
        {
            var data = await _departmentService.GetAll<DepartmentResponseDto>();
            return data ?? Enumerable.Empty<DepartmentResponseDto>();
        }

        var mappedQuery = string.IsNullOrEmpty(query)
            ? null
            : ExpressionHelper.ParseComplexQuery<DepartmentResponseDto>(query);
        var mappedIncludes = includes.Select(ExpressionHelper.ParseExpression<DepartmentResponseDto>).ToArray();

        var result = await _departmentService.GetAll(predicate: mappedQuery, includes: mappedIncludes);
        return result ?? Enumerable.Empty<DepartmentResponseDto>();
    }

    private async Task<DepartmentBaseDto?> _getDepartmentById(int id, string[] includes)
    {
        if (includes.Length == 0)
        {
            return await _departmentService.Get<DepartmentResponseDto>(id);
        }

        var mappedIncludes = includes.Select(ExpressionHelper.ParseExpression<DepartmentResponseDto>).ToArray();
        return await _departmentService.Get(id, mappedIncludes);
    }

    private async Task<DepartmentBaseDto?> _getDepartmentByQuery(string query, string[] includes)
    {
        var mappedQuery = ExpressionHelper.ParseComplexQuery<DepartmentBaseDto>(query);
        var mappedIncludes = includes.Select(ExpressionHelper.ParseExpression<DepartmentBaseDto>).ToArray();

        return includes.Length == 0
            ? await _departmentService.Get(mappedQuery)
            : await _departmentService.Get(mappedQuery, mappedIncludes);
    }

    private IActionResult _generateResponse<T>(T result)
    {
        if (_departmentService.OperationResult.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(_departmentService.OperationResult.Errors);
    }

    private IActionResult _generateResponse<T>(T result, string actionName, object routeValues)
    {
        if (_departmentService.OperationResult.IsSuccess)
        {
            return CreatedAtAction(actionName, routeValues, result);
        }

        return BadRequest(_departmentService.OperationResult.Errors);
    }

    private IActionResult _handleException(Exception ex, string message)
    {
        _logger.LogError($"{message}: {ex.Message}");
        return StatusCode(500, "An internal server error occurred.");
    }

    #endregion
}