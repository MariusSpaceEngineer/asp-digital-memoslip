using AspDigitalMemoSlip.Application.CQRS.Authentication;
using AspDigitalMemoSlip.Application.CQRS.Validators.Authentication;
using DTOClassLibrary.DTO.Authentication;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Text;

namespace AspDigitalMemoSlip.Test.Authentication
{
    public class AuthenticationValidationTests
    {
        [TestClass]
        public class LoginValidationTests
        {
            private LoginCommandValidator validator;

            [TestInitialize]
            public void TestInitialize()
            {
                validator = new LoginCommandValidator();
            }

            [DataTestMethod]
            [DataRow("", false, "Username is required.")] // Empty string
            [DataRow("Test@User", false, "Username can only contain alphanumeric characters.")] // Non-alphanumeric characters
            [DataRow("TestUser", true, "")] // Valid username
            public void Validate_Username(string username, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var dto = new LoginDTO { Username = username, Password = "Password123!" };
                var command = new LoginCommand(dto, "http://localhost");

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.Username" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("12345", false, "Password must be at least 6 characters long.")] // Less than 6 characters
            [DataRow("abcdef", false, "Password must contain at least one uppercase letter.")] // No uppercase letters
            [DataRow("ABCDEF", false, "Password must contain at least one digit.")] // No digits
            [DataRow("Abcdef", false, "Password must contain at least one non-alphanumeric character.")] // No non-alphanumeric characters
            [DataRow("Abcdef1", false, "Password must contain at least one special character.")] // No special characters
            [DataRow("Abcdef1!", true, "")] // Valid password
            public void Validate_Password(string password, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var dto = new LoginDTO { Username = "TestUser", Password = password };
                var command = new LoginCommand(dto, "http://localhost");

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.Password" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("12345", false, "Multi-Factor code must be 6 digits long.")] // Less than 6 digits
            [DataRow("1234567", false, "Multi-Factor code must be 6 digits long.")] // More than 6 digits
            [DataRow("12345a", false, "Multi-Factor code can only contain digits.")] // Non-digit characters
            [DataRow("123456", true, "")] // Valid OTCode
            public void Validate_OTCode(string otCode, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var validator = new LoginCommandValidator();
                var dto = new LoginDTO { Username = "TestUser", OTCode = otCode };
                var command = new LoginCommand(dto, "http://localhost");

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.OTCode" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("", false, "Url is required.")] // Empty URL
            [DataRow("http://localhost", true, "")] // Valid URL
            public void Validate_Url(string url, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var validator = new LoginCommandValidator();
                var dto = new LoginDTO { Username = "TestUser", Password = "Password123!" };
                var command = new LoginCommand(dto, url);

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Url" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("", "", false, "Either Password or OTCode must be provided.")] // Both Password and OTCode are empty
            [DataRow("Password1!", "", true, "")] // Only Password is provided
            [DataRow("", "123456", true, "")] // Only OTCode is provided
            [DataRow("Password1!", "123456", true, "")] // Both Password and OTCode are provided
            public void Validate_EitherPasswordOrOTCodeProvided(string password, string otCode, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var validator = new LoginCommandValidator();
                var dto = new LoginDTO { Username = "TestUser", Password = password, OTCode = otCode };
                var command = new LoginCommand(dto, "http://localhost");

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == expectedErrorMessage));
                }
            }

        }

        [TestClass]
        public class RegistrationValidationTests
        {
            private RegisterCommandValidator validator;
            private IList<string> roles;

            [TestInitialize]
            public void TestInitialize()
            {
                validator = new RegisterCommandValidator();
                roles = new List<string>() { "Consignee" }; // Add roles as necessary

            }

            [DataTestMethod]
            [DataRow("", false, "Username is required.")] // Empty username
            [DataRow("Test@User", false, "Username can only contain alphanumeric characters.")] // Non-alphanumeric characters
            [DataRow("TestUser", true, "")] // Valid username
            public void Validate_Username(string username, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var dto = new RegisterDTO
                {
                    Username = username,
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.Username" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("", false, "Email is required.")] // Empty email
            [DataRow("testuser", false, "Email is not valid.")] // Invalid email
            [DataRow("testuser@example.com", true, "")] // Valid email
            public void Validate_Email(string email, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = email,
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.Email" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("", false, "Password is required.")] // Empty password
            [DataRow("12345", false, "Password must be at least 6 characters long.")] // Less than 6 characters
            [DataRow("abcdef", false, "Password must contain at least one uppercase letter.")] // No uppercase letters
            [DataRow("ABCDEF", false, "Password must contain at least one digit.")] // No digits
            [DataRow("Abcdef", false, "Password must contain at least one non-alphanumeric character.")] // No non-alphanumeric characters
            [DataRow("Abcdef1!", true, "")] // Valid password
            public void Validate_Password(string password, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = password,
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.Password" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("", false, "Phone number is required.")] // Empty phone number
            [DataRow("123", false, "Phone number must be between 4 and 16 characters long.")] // Less than 4 characters
            [DataRow("12345678901234567", false, "Phone number must be between 4 and 16 characters long.")] // More than 16 characters
            [DataRow("abc", false, "Phone number can only contain digits and optionally a '+' prefix.")] // Contains non-digit characters
            [DataRow("+1234567890", true, "")] // Valid phone number
            public void Validate_PhoneNumber(string phoneNumber, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = phoneNumber,
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.PhoneNumber" && e.ErrorMessage == expectedErrorMessage));
                }
            }


            [DataTestMethod]
            [DataRow("", false, "Name is required.")] // Empty name
            [DataRow("John Doe", true, "")] // Valid name
            public void Validate_Name(string name, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = name,
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.Name" && e.ErrorMessage == expectedErrorMessage));
                }

            }

            [DataTestMethod]
            [DataRow("", false, "VAT number is required.")] // Empty VAT number
            [DataRow("BE0123456789", true, "")] // Valid VAT number
            public void Validate_VATNumber(string vatNumber, bool expectedIsValid, string expectedErrorMessage)
            {
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = vatNumber,
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.VATNumber" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("", false, "Insurance coverage is required.")] // Empty insurance number
            [DataRow("1234567890", true, "")] // Valid insurance number
            public void Validate_InsuranceNumber(string insuranceNumber, bool expectedIsValid, string expectedErrorMessage)
            {
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = insuranceNumber,
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.InsuranceNumber" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("", false, "Insurance coverage is required.")] // Empty insurance coverage
            [DataRow("abc", false, "Insurance coverage can only contain numbers and optionally a decimal point or comma.")] // Contains non-digit characters
            [DataRow("1000000", true, "")] // Valid insurance coverage
            public void Validate_InsuranceCoverage(string insuranceCoverage, bool expectedIsValid, string expectedErrorMessage)
            {
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = insuranceCoverage,
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.InsuranceCoverage" && e.ErrorMessage == expectedErrorMessage));
                }

            }

            [DataTestMethod]
            [DataRow("", false, "National registry number is required.")] // Empty national registry number
            [DataRow("1234567890", true, "")] // Valid national registry number
            public void Validate_NationalRegistryNumber(string nationalRegistryNumber, bool expectedIsValid, string expectedErrorMessage)
            {
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = nationalRegistryNumber,
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.NationalRegistryNumber" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("", false, "National registry expirationDate is required.")] // Empty national registry expiration date
            [DataRow("2025-12-31", true, "")] // Valid date in yyyy-MM-dd format
            [DataRow("31-12-2025", true, "")] // Valid date in dd-MM-yyyy format
            [DataRow("12/31/2025", true, "")] // Valid date in MM/dd/yyyy format
            [DataRow("12-31-2025", true, "")] // Valid date in MM-dd-yyyy format
            [DataRow("31-12-2025", true, "")] // Valid date in d-M-yyyy format
            [DataRow("12/31/2025", true, "")] // Valid date in M/d/yyyy format
            [DataRow("2025/12/31", true, "")] // Valid date in yyyy/M/d format
            public void Validate_NationalRegistryExpirationDate(string nationalRegistryExpirationDate, bool expectedIsValid, string expectedErrorMessage)
            {
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = nationalRegistryExpirationDate,
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")
                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.NationalRegistryExpirationDate" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("dummy.txt", false, "Selfie must be a valid image file.")] // Invalid ID-Copy (text file)
            [DataRow("dummy.gif", false, "Selfie must be a valid image file.")] // Invalid ID-Copy (text file)
            [DataRow("dummy.jpg", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.jpeg", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.png", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.bmp", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.tiff", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.pdf", false, "Selfie must be a valid image file.")] // Valid ID-Copy (PDF)
            public void Validate_Selfie(string fileName, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                IFormFile selfie = null;
                if (fileName != null)
                {
                    selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", fileName);
                }

                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = selfie,
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                };

                var command = new RegisterCommand("ConsignerTest", dto, roles);
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.Selfie" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("dummy.txt", false, "ID-Copy must be a valid image file.")] // Invalid ID-Copy (text file)
            [DataRow("dummy.gif", false, "ID-Copy must be a valid image file.")] // Invalid ID-Copy (text file)
            [DataRow("dummy.jpg", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.jpeg", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.png", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.bmp", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.tiff", true, "")] // Valid ID-Copy (image)
            [DataRow("dummy.pdf", true, "")] // Valid ID-Copy (PDF)
            public void Validate_IDCopy(string fileName, bool expectedIsValid, string expectedErrorMessage)
            {
                // Arrange
                IFormFile idCopy = null;
                if (fileName != null)
                {
                    idCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", fileName);
                }

                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = idCopy
                };

                var command = new RegisterCommand("ConsignerTest", dto, roles);
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.IDCopy" && e.ErrorMessage == expectedErrorMessage));
                }
            }

            [DataTestMethod]
            [DataRow("Consignee", true, "")] // Valid roles
            public void Validate_Roles(string rolesString, bool expectedIsValid, string expectedErrorMessage)
            {
                var roles = new List<string>();
                if (!string.IsNullOrEmpty(rolesString))
                {
                    roles = rolesString.Split(',').ToList();
                }
                var dto = new RegisterDTO
                {
                    Username = "TestUser",
                    Email = "testuser@example.com",
                    Password = "Password1!",
                    PhoneNumber = "+1234567890",
                    Name = "Test User",
                    VATNumber = "BE0123456789",
                    InsuranceNumber = "1234567890",
                    InsuranceCoverage = "1000000",
                    NationalRegistryNumber = "1234567890",
                    NationalRegistryExpirationDate = "2025-12-31",
                    Selfie = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png"),
                    IDCopy = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.png")

                };
                var command = new RegisterCommand("ConsignerTest", dto, roles);

                // Act
                var result = validator.Validate(command);

                // Assert
                Assert.AreEqual(expectedIsValid, result.IsValid);
                if (!expectedIsValid)
                {
                    Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Dto.Roles" && e.ErrorMessage == expectedErrorMessage));
                }
            }

        }
    }
}
