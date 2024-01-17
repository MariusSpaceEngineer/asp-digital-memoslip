using AspDigitalMemoSlip.Application.CQRS.MemoSlips;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Test.Memo
{
    [TestClass]
    public class AcceptMemoCommandValidatorTests
    {
        private AcceptMemoCommandValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            validator = new AcceptMemoCommandValidator();
        }

        [TestMethod]
        public async Task ValidateAsync_UserIdIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AcceptMemoCommand(string.Empty, new DTOClassLibrary.DTO.Memo.AcceptMemoDTO { Id = 1, Password = "Password" });

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual("User ID is required.", result.Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task ValidateAsync_AcceptMemoDTOIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new AcceptMemoCommand("123userId", null);

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual("Accept memo data is required.", result.Errors[0].ErrorMessage);
        }
    }
}
