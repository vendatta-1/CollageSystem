using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Controllers;
using api.DTOs.Student;
using api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace api.Tests.Controllers
{
    public class StudentControllerTests
    {
        private readonly Mock<IStudentService> _mockStudentService;
        private readonly Mock<ILogger<StudentController>> _mockLogger;
        private readonly StudentController _controller;

        public StudentControllerTests()
        {
            _mockStudentService = new Mock<IStudentService>();
            _mockLogger = new Mock<ILogger<StudentController>>();
            _controller = new StudentController(_mockStudentService.Object, _mockLogger.Object);
        }
    }
}
