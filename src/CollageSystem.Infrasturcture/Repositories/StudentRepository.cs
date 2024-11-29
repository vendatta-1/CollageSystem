using CollageSystem.Core.Interfaces.Repository;
using CollageSystem.Core.Models;
using CollageSystem.Core.Models.RelationshipEntities;
using CollageSystem.Core.Models.SecurityModels;
using CollageSystem.Core.Results;
using CollageSystem.Core.Validation;
using CollageSystem.Data.Contexts;
using CollageSystem.Infrastructure.Repositories;
using CollageSystem.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CollageSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository class to handle operations related to students.
    /// </summary>
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Student> _logger;
        private readonly ILogger<OperationResult> _operationLogger;
        private readonly StudentHelper _studentHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="logger">Logger instance for student-related logs.</param>
        /// <param name="operationLogger">Logger instance for operation result logs.</param>
        /// <param name="studentHelper">Helper class for student-related operations.</param>
        public StudentRepository(ApplicationDbContext context,
            ILogger<Student> logger,
            ILogger<OperationResult> operationLogger,
            StudentHelper studentHelper)
            : base(context, logger, operationLogger)
        {
            _studentHelper = studentHelper;
            _context = context;
            _logger = logger;
            _operationLogger = operationLogger;
        }

        /// <summary>
        /// Creates a new student entity and performs additional related operations.
        /// </summary>
        /// <param name="entity">The student entity to be created.</param>
        /// <returns>An <see cref="OperationResult"/> indicating success or failure.</returns>
        public override async Task<OperationResult> Create(Student entity)
        {
            try
            {
                var code = await GenerateUniqueStudentCode(entity);

                // Set the unique student code
                entity.StudentCode = code;

                // Create the student in the repository
                var result = await base.Create(entity);

                // Check if creation failed
                if (result.IsFailure)
                    return result;

                // Create additional student info
                result = await CreateStudentInfo(entity);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred during student creation.");
                return new OperationResult(OperationStatus.Failure, _operationLogger).WithException(e);
            }
        }

        /// <summary>
        /// Creates additional information related to the student.
        /// </summary>
        /// <param name="student">The student entity for which to create additional information.</param>
        /// <returns>An <see cref="OperationResult"/> indicating success or failure.</returns>
        public async Task<OperationResult> CreateStudentInfo(Student student)
        {
            try
            {
                // Generate additional student info
                var studentInfo = await _studentHelper.GenerateStudentInfo(student.StudentCode);

                // Assign the student ID to the generated student info
                studentInfo.StudentId = student.Id;

                // Add student info to the context
                var result = await _context.Set<StudentCrucialInformation>().AddAsync(studentInfo);

                if (result.State == EntityState.Added)
                    await _context.SaveChangesAsync();
                else
                {
                    return new OperationResult(OperationStatus.Failure, _operationLogger)
                        .WithErrorCode(ErrorCode.CreateFailed, "Failed to create student info", FailureLevel.Important);
                }

                // Create student user
                var userCreationResult = await _studentHelper.CreateStudentUser(student, studentInfo);
                if (userCreationResult.IsFailure)
                {
                    _operationLogger.LogError($"Error creating user for student {student.StudentCode}: {userCreationResult.Errors[0]}");
                    return userCreationResult.WithStatus(OperationStatus.Failure)
                        .WithErrorCode(ErrorCode.CreateFailed, $"Error creating user for student {student.StudentCode}", FailureLevel.Important);
                }

                return userCreationResult;
            }
            catch (Exception ex)
            {
                _operationLogger.LogError($"Error in CreateStudentInfo: {ex.Message}");
                return new OperationResult(OperationStatus.Failure, _operationLogger)
                    .WithException(ex);
            }
        }

        /// <summary>
        /// Generates a unique student code by checking against existing codes.
        /// </summary>
        /// <param name="entity">The student entity to generate the code for.</param>
        /// <returns>A unique student code.</returns>
        private async Task<string> GenerateUniqueStudentCode(Student entity)
        {
            var studentSet = _context.Set<Student>();
            string code;

            do
            {
                code = await _studentHelper.GenerateStudentCode(entity);
            } while (await studentSet.AnyAsync(x => x.StudentCode == code));

            return code;
        }

        /// <summary>
        /// Assigns courses to the student based on their department and academic year.
        /// </summary>
        /// <param name="student">The student entity to assign courses to.</param>
        /// <returns>An <see cref="OperationResult"/> indicating success or failure.</returns>
        private async Task<OperationResult> AssignStudentCourses(Student student)
        {
            try
            {
                // Get the courses based on department and academic year
                var courses = await _context.Courses
                    .Where(x => x.DepartmentId == student.DepartmentId && x.Year == student.AcademicYear)
                    .ToListAsync();

                // Assign each course to the student
                foreach (var course in courses)
                {
                    var studentCourse = new StudentCourse
                    {
                        CourseId = course.Id,
                        StudentId = student.Id
                    };
                    await _context.StudentCourses.AddAsync(studentCourse);
                }

                await _context.SaveChangesAsync();
                return new OperationResult(OperationStatus.Success, _operationLogger);
            }
            catch (Exception ex)
            {
                _operationLogger.LogError($"Error assigning courses to student {student.StudentCode}: {ex.Message}");
                return new OperationResult(OperationStatus.Failure, _operationLogger)
                    .WithException(ex);
            }
        }
    }
}
