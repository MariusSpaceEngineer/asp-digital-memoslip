using AspDigitalMemoSlip.Api.Controllers;
using AspDigitalMemoSlip.Application.CQRS.Authentication;
using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Utils;
using DTOClassLibrary.DTO.Authentication;
using DTOClassLibrary.DTO.Consigner;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using System.Text;

namespace AspDigitalMemoSlip.Test.Authentication
{
    [TestClass]
    public class AuthenticationControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<ILogger<AuthenticationController>> _loggerMock;
        private AuthenticationController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<AuthenticationController>>();
            _controller = new AuthenticationController(_mediatorMock.Object, _loggerMock.Object);
        }

        #region Login

        [TestMethod]
        public async Task Login_SetsAuthorizationHeader_WhenModelStateIsValid()
        {
            // Arrange
            var model = new LoginDTO
            {
                Username = "TestUser",
                Password = "TestPassword",
                OTCode = "123456"
            };

            var context = new DefaultHttpContext();
            context.Request.Headers["Origin"] = "http://localhost"; // Set the Origin header

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context,
            };

            // Mock the Send method to return a result with a Token
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new AuthResult(201, "Login Succesfull", "TestToken"));

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.AreEqual("Bearer TestToken", _controller.Response.Headers["Authorization"].ToString());
        }

        [TestMethod]
        public async Task Login_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new LoginDTO
            {
                // Provide an invalid model
                Username = "",
                Password = "",
                OTCode = ""
            };

            var context = new DefaultHttpContext();
            context.Request.Headers["Origin"] = "http://localhost"; // Set a valid Origin header

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context,
            };

            _controller.ModelState.AddModelError("Username", "Required");
            _controller.ModelState.AddModelError("Password", "Required");
            _controller.ModelState.AddModelError("OTCode", "Required");

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Login_ThrowsException_WhenOriginUrlIsIncorrect()
        {
            // Arrange
            var model = new LoginDTO
            {
                Username = "TestUser",
                Password = "TestPassword",
                OTCode = "123456"
            };

            var context = new DefaultHttpContext();
            context.Request.Headers["Origin"] = "http://incorrecturl"; // Set an incorrect Origin header

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context,
            };

            // Setup the mediator to throw an exception when Send is called
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Unauthorized"));

            // Act
            var exception = await Assert.ThrowsExceptionAsync<Exception>(() => _controller.Login(model));

            // Assert
            Assert.AreEqual("Unauthorized", exception.Message);
        }

        [TestMethod]
        public async Task Login_ReturnsOkResult_WhenOriginUrlIsValid()
        {
            // Arrange
            var model = new LoginDTO
            {
                Username = "TestUser",
                Password = "TestPassword",
                OTCode = "123456"
            };

            var context = new DefaultHttpContext();
            context.Request.Headers["Origin"] = "http://localhost"; // Set a valid Origin header

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context,
            };

            // Setup the mediator to return a result with a Token when Send is called
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new AuthResult(201, "Login Successful", "TestToken"));

            // Act
            var result = await _controller.Login(model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.AreEqual("Bearer TestToken", _controller.Response.Headers["Authorization"].ToString());
        }

        [TestMethod]
        public async Task Login_ThrowsException_WhenLoginIsUnsuccessful()
        {
            // Arrange
            var model = new LoginDTO
            {
                Username = "TestUser",
                Password = "IncorrectPassword",
                OTCode = "123456"
            };

            var context = new DefaultHttpContext();
            context.Request.Headers["Origin"] = "http://localhost"; // Set a valid Origin header

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = context,
            };

            // Setup the mediator to throw an exception when Send is called
            _mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                         .ThrowsAsync(new Exception("Unauthorized"));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _controller.Login(model));
        }

        #endregion

        #region Register
        [TestMethod]
        public async Task Register_ReturnsCreatedAtAction_WhenModelStateIsValid()
        {
            // Arrange
            var model = new RegisterDTO
            {
                Username = "TestUser",
                Email = "testuser@example.com",
                Password = "TestPassword",
                PhoneNumber = "1234567890",
                Name = "Test User",
                VATNumber = "BE0123456789", // Replace with a valid BTW number
                InsuranceNumber = "123456", // Replace with a valid insurance number
                InsuranceCoverage = "Test Coverage",
                NationalRegistryNumber = "123456", // Replace with a valid national registry number
                NationalRegistryExpirationDate = "2023-12-31", // Replace with a valid expiration date
                ConsignerId = "TestConsignerId",
                Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt"), // Replace with a valid IFormFile
                IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt") // Replace with a valid IFormFile
            };


            var contextMock = new Mock<HttpContext>();
            var responseMock = new Mock<HttpResponse>();
            responseMock.SetupGet(r => r.Headers).Returns(new HeaderDictionary());

            contextMock.SetupGet(x => x.Response).Returns(responseMock.Object);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object,
            };

            var expectedToken = "TestToken";
            var expectedStatusCode = 201;
            var expectedMessage = "Registration Successful";

            var authResult = new AuthResult(expectedStatusCode, expectedMessage, expectedToken);

            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Register("TestConsignerUserName", model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdAtResult = result as CreatedAtActionResult;

            // Assert that the correct status code and message were returned
            Assert.AreEqual(expectedStatusCode, authResult.StatusCode);
            Assert.AreEqual(expectedMessage, authResult.Message);
        
        }

        [TestMethod]
        public async Task Register_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new RegisterDTO(); // Provide an invalid model

            _controller.ModelState.AddModelError("Error", "Invalid model state"); // Add a model state error

            // Act
            var result = await _controller.Register("TestConsignerUserName", model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task Register_ReturnsCreatedAtAction_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var model = new RegisterDTO
            {
                Username = "TestUser",
                Email = "testuser@example.com",
                Password = "TestPassword",
                PhoneNumber = "1234567890",
                Name = "Test User",
                VATNumber = "BE0123456789", // Replace with a valid BTW number
                InsuranceNumber = "123456", // Replace with a valid insurance number
                InsuranceCoverage = "Test Coverage",
                NationalRegistryNumber = "123456", // Replace with a valid national registry number
                NationalRegistryExpirationDate = "2023-12-31", // Replace with a valid expiration date
                ConsignerId = "TestConsignerId",
                Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt"), // Replace with a valid IFormFile
                IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt") // Replace with a valid IFormFile
            };

            var contextMock = new Mock<HttpContext>();
            var responseMock = new Mock<HttpResponse>();
            responseMock.SetupGet(r => r.Headers).Returns(new HeaderDictionary());

            contextMock.SetupGet(x => x.Response).Returns(responseMock.Object);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = contextMock.Object,
            };

            var expectedToken = "TestToken";
            var authResult = new AuthResult(201, "Registration Successful", expectedToken);

            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(authResult);

            // Act
            var result = await _controller.Register("TestConsignerUserName", model);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }

        [TestMethod]
        public async Task Register_ThrowsUserAlreadyExistsException_WhenUserExists()
        {
            // Arrange
            var model = new RegisterDTO
            {
                Username = "TestUser",
                Email = "testuser@example.com",
                Password = "TestPassword",
                PhoneNumber = "1234567890",
                Name = "Test User",
                VATNumber = "BE0123456789", // Replace with a valid BTW number
                InsuranceNumber = "123456", // Replace with a valid insurance number
                InsuranceCoverage = "Test Coverage",
                NationalRegistryNumber = "123456", // Replace with a valid national registry number
                NationalRegistryExpirationDate = "2023-12-31", // Replace with a valid expiration date
                ConsignerId = "TestConsignerId",
                Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt"), // Replace with a valid IFormFile
                IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt") // Replace with a valid IFormFile
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                         .Throws(new UserAlreadyExistsException("User already exists"));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<UserAlreadyExistsException>(() => _controller.Register("TestConsignerUserName", model));
        }

        [TestMethod]
        public async Task Register_ThrowsInvalidVatNumberException_WhenVatNumberIsInvalid()
        {
            // Arrange
            var model = new RegisterDTO
            {
                Username = "TestUser",
                Email = "testuser@example.com",
                Password = "TestPassword",
                PhoneNumber = "1234567890",
                Name = "Test User",
                VATNumber = "BE0123456789", // Replace with a valid BTW number
                InsuranceNumber = "123456", // Replace with a valid insurance number
                InsuranceCoverage = "Test Coverage",
                NationalRegistryNumber = "123456", // Replace with a valid national registry number
                NationalRegistryExpirationDate = "2023-12-31", // Replace with a valid expiration date
                ConsignerId = "TestConsignerId",
                Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt"), // Replace with a valid IFormFile
                IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt") // Replace with a valid IFormFile
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                         .Throws(new InvalidVatNumberException("Invalid VAT number"));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidVatNumberException>(() => _controller.Register("TestConsignerUserName", model));
        }
        #endregion

        #region Get User Images
        [TestMethod]
        public async Task GetUserImages_ReturnsOk_WhenImagesAreAvailable()
        {
            // Arrange
            var userId = "TestUserId";
            var userImagesResult = new UserImagesResult
            {
                IDCopy = new FileContentResult(new byte[0], "image/jpeg"),
                Selfie = new FileContentResult(new byte[0], "image/jpeg")
            };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllUserImagesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(userImagesResult);

            // Act
            var result = await _controller.GetUserImages(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(UserImagesResult));
            var returnedImages = okResult.Value as UserImagesResult;
            Assert.AreEqual(userImagesResult, returnedImages);
        }
        #endregion

        #region Generate QR code
        [TestMethod]
        public async Task GenerateQRCode_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.GenerateQRCode();

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public async Task GenerateQRCode_ReturnsOk_WhenQRCodeIsGenerated()
        {
            // Arrange
            var userId = "TestUserId";
            var expectedQRCode = new QRCodeResult("", "TestQRCode");

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        };
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GenerateConsignerQRCodeCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(expectedQRCode);

            // Act
            var result = await _controller.GenerateQRCode();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(expectedQRCode, okResult.Value as QRCodeResult);
        }

        #endregion
    }
}
