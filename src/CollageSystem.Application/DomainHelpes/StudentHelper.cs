using System.Text;
using CollageSystem.Application.Services.Interfaces;

using CollageSystem.Core.Models;
using CollageSystem.Core.Models.SecurityModels;
using CollageSystem.Core.Results;
using CollageSystem.Core.Validation;
using Microsoft.Extensions.Logging;
using static CollageSystem.Core.Validation.ErrorCode;
namespace CollageSystem.Utilities.Helpers;

public class StudentHelper
{
    private readonly ILogger<OperationResult> _operationLogger =
        new LoggerFactory().CreateLogger<OperationResult>();

    private readonly OperationResult _result;
    private readonly IUserService _userService;

    public StudentHelper(IUserService userService)
    {
        _userService = userService;
        _result = new OperationResult(OperationStatus.Pending, new LoggerFactory().CreateLogger<OperationResult>());
    }


    public async Task<string> GenerateStudentCode(Student student)
    {
        var rand = new Random();
        var codeBuilder = new StringBuilder();
        codeBuilder.Append("ST");
        codeBuilder.Append((DateTime.Now.Year % 100).ToString());
        codeBuilder.Append(student.Age.ToString());
        codeBuilder.Append(student.AcademicYear.HasValue ? (int)student.AcademicYear : "0");
        codeBuilder.Append(rand.Next(10101, 99119));

        return await Task.FromResult(codeBuilder.ToString());
    }

    public async Task<Dictionary<string, string>> GenerateStudentEmailAndPassword(string code)
    {
        var email = $"{code}.fci.s-mu.edu.au";
        var password = await GeneratePassword(code);
        return await Task.FromResult(new Dictionary<string, string> { { "Email", email }, { "Password", password } });
    }

    public async Task<StudentCrucialInformation> GenerateStudentInfo(string studentCode)
    {
        try
        {
            var studentInfo = new StudentCrucialInformation
            {
                StudentCode = studentCode
            };

            var emailAndPassword = await GenerateStudentEmailAndPassword(studentCode);

            studentInfo.UniversityEmail = emailAndPassword["Email"];

            studentInfo.Password = emailAndPassword["Password"];

            if (string.IsNullOrEmpty(studentInfo.UniversityEmail) || string.IsNullOrEmpty(studentInfo.Password))
                throw new ApplicationException(
                    "can not create the info for unknown reasons ensure studentCode is not empty string ");

            return await Task.FromResult(studentInfo);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _operationLogger.LogError("see error message for Student helper: " + e.Message);
            throw new ApplicationException(
                "can not create the info for unknown reasons ensure studentCode is not empty string ");
        }
    }

    public async Task<OperationResult> CreateStudentUser(Student student, StudentCrucialInformation studentInfo)
    {
        try
        {
            var sName = student.Name.Split(" ");
            
            var user = new AppUser
            {
                Email = studentInfo.UniversityEmail,
                UserName = studentInfo.UniversityEmail.Split(".")[0],
                SecurityStamp = Guid.NewGuid().ToString(),
                FirstName = sName[0],
                LastName = sName.Length > 1 ? sName[1] : string.Empty,
                PhoneNumber = student.PhoneNumber
            };
            
            var result = await _userService.CreateStudent(user, studentInfo.Password);

            if (!result.IsSuccess)
                return result.WithErrorCode(CreateFailed);

            return await Task.FromResult(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _operationLogger.LogError("see error message for Student helper: ");
            return _result.WithErrorCode(CreateFailed).WithException(e);
        }
    }

    private async Task<string> GeneratePassword(string code)
    {
        // Define character sets
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        const string characterSet = upperCase + lowerCase + digits + specialChars;

        var rand = new Random();
        var passwordBuilder = new StringBuilder();


        passwordBuilder.Append(upperCase[rand.Next(upperCase.Length)]);
        passwordBuilder.Append(lowerCase[rand.Next(lowerCase.Length)]);
        passwordBuilder.Append(digits[rand.Next(digits.Length)]);
        passwordBuilder.Append(specialChars[rand.Next(specialChars.Length)]);

        var piece = code.Substring(rand.Next(0, code.Length - 2));
        passwordBuilder.Append(piece);

        while (passwordBuilder.Length < 14)
        {
            passwordBuilder.Append(characterSet[rand.Next(characterSet.Length)]);
        }

        var finalPassword = ShuffleString(passwordBuilder.ToString());

        return await Task.FromResult(finalPassword);
    }


    private static string ShuffleString(string input)
    {
        var array = input.ToCharArray();
        var rand = new Random();
        for (var i = array.Length - 1; i > 0; i--)
        {
            var j = rand.Next(i + 1);

            (array[i], array[j]) = (array[j], array[i]);
        }

        return new string(array);
    }
}