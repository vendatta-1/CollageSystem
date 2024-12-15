using System.Linq.Dynamic.Core;
using api.DTOs.Student;
using api.Helpers;
using api.Services.Interfaces;
using CollageSystem.Application.DTOs.Student;
using CollageSystem.Application.Services.Interfaces;
using CollageSystem.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AppPolicies.AdminOnly)]
    public class StudentController : ControllerBase
    {
        #region Constructor

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        #endregion

        #region Private Fields

        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;
        private const int PAGE_SIZE = 10;
        private static int _currentPageNumber = 1;

        #endregion

        #region Public Actions

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll(string? query, [FromQuery] params string[] includes)
        {
            try
            {
                var students = await _getStudents(query, includes);

                return _generateResponse(students);
            }
            catch (Exception ex)
            {
                return _handleException(ex, "Error getting students");
            }
        }

        [HttpGet("[action]/{id:int}")]
        public async Task<IActionResult> Get(int id, [FromQuery] params string[] includes)
        {
            try
            {
                var student = await _getStudentById(id, includes);
                return _generateResponse(student);
            }
            catch (Exception ex)
            {
                return _handleException(ex, "Error getting student by ID");
            }
        }

        [HttpGet("[action]Query")]
        public async Task<IActionResult> Get(string query, [FromQuery] params string[] includes)
        {
            try
            {
                var student = await _getStudentByQuery(query, includes);
                return _generateResponse(student);
            }
            catch (Exception ex)
            {
                return _handleException(ex, "Error getting student by query");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPages(string? query, [FromQuery] params string[] includes)
        {
            try
            {
                var response = await _getPagedResult<StudentResponseDto>(_currentPageNumber, PAGE_SIZE);
                if (response.HasNext)
                    _currentPageNumber++;
                else
                    _currentPageNumber = 1;
                return _generateResponse(response);
            }
            catch (Exception e)
            {
                return _handleException(e, e.Message);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StudentCreateDto studentDto)
        {
            try
            {
                var createdStudentResult = await _studentService.Create(studentDto);
                return _generateResponse(createdStudentResult, nameof(Get), new { createdStudentResult.IsSuccess });
            }
            catch (Exception ex)
            {
                return _handleException(ex, "Error creating student");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentUpdateDto studentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedStudent = await _studentService.Update(studentDto.StudentCode, studentDto);
                return _generateResponse(updatedStudent);
            }
            catch (Exception ex)
            {
                return _handleException(ex, "Error updating student");
            }
        }

        #endregion

        #region Private Methods

        private async Task<Helpers.PagedResult<TD>> _getPagedResult<TD>(int pageNumber, int pageSize)
            where TD : StudentBaseDto
        {
            var page = await _studentService.GetAllPaged<TD>(pageNumber: pageNumber, pageSize: pageSize);
            return page;
        }

        private async Task<IEnumerable<StudentResponseDto>> _getStudents(string? query, string[] includes)
        {
            if (string.IsNullOrEmpty(query) && includes.Length == 0)
            {
                var data = await _studentService.GetAll<StudentResponseDto>();
                return data ?? Enumerable.Empty<StudentResponseDto>();
            }

            var mappedQuery = string.IsNullOrEmpty(query)
                ? null
                : ExpressionHelper.ParseComplexQuery<StudentResponseDto>(query);
            var mappedIncludes = includes.Select(ExpressionHelper.ParseExpression<StudentResponseDto>).ToArray();

            return await _studentService.GetAll(predicate: mappedQuery, includes: mappedIncludes);
        }

        private async Task<StudentResponseDto?> _getStudentById(int id, string[] includes)
        {
            if (includes.Length == 0)
            {
                return await _studentService.Get<StudentResponseDto>(id);
            }

            var mappedIncludes = includes.Select(ExpressionHelper.ParseExpression<StudentResponseDto>).ToArray();
            return await _studentService.Get(id, mappedIncludes);
        }

        private async Task<StudentResponseDto?> _getStudentByQuery(string query, string[] includes)
        {
            var mappedQuery = ExpressionHelper.ParseComplexQuery<StudentResponseDto>(query);
            var mappedIncludes = includes.Select(ExpressionHelper.ParseExpression<StudentResponseDto>).ToArray();

            return includes.Length == 0
                ? await _studentService.Get(mappedQuery)
                : await _studentService.Get(mappedQuery, mappedIncludes);
        }

        private IActionResult _generateResponse<T>(T result)
        {
            if (_studentService.OperationResult.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(_studentService.OperationResult.Errors);
        }

        private IActionResult _generateResponse<T>(T result, string actionName, object routeValues)
        {
            if (_studentService.OperationResult.IsSuccess)
            {
                return CreatedAtAction(actionName, routeValues, result);
            }

            return BadRequest(_studentService.OperationResult.Errors);
        }

        private IActionResult _handleException(Exception ex, string message)
        {
            _logger.LogError($"{message}: {ex.Message}");
            return StatusCode(500,
                new { ExceptionType = ex.GetType(), ex.Message, _studentService.OperationResult.Errors });
        }

        #endregion
    }
}