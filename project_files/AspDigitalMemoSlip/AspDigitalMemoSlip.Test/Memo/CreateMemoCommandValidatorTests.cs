using AspDigitalMemoSlip.Application.CQRS.MemoSlips;
using DTOClassLibrary.DTO.Consignee;
using DTOClassLibrary.DTO.Consigner;
using DTOClassLibrary.DTO.Memo;
using DTOClassLibrary.DTO.Product;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Test.Memo
{
    [TestClass]
    public class CreateMemoCommandValidatorTests
    {
        private CreateMemoCommandValidator validator;
        private Mock<IMediator> mediatorMock;

        [TestInitialize]
        public void Initialize()
        {
            mediatorMock = new Mock<IMediator>();
            validator = new CreateMemoCommandValidator(mediatorMock.Object);
        }

        [TestMethod]
        public async Task ValidateAsync_ProductsIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = GetValidCreateMemoCommand();
            command.Memo.Products.Clear();

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Product list cannot be empty."));
        }

        [TestMethod]
        public async Task ValidateAsync_ConsignerIdIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = GetValidCreateMemoCommand();
            command.Memo.ConsignerId = string.Empty;

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Consigner ID cannot be empty."));
        }

        [TestMethod]
        public async Task ValidateAsync_ConsigneeIdIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = GetValidCreateMemoCommand();
            command.Memo.ConsigneeId = string.Empty;

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Consignee ID cannot be empty."));
        }

        [TestMethod]
        public async Task ValidateAsync_TermsAcceptedIsFalse_ShouldHaveValidationError()
        {
            // Arrange
            var command = GetValidCreateMemoCommand();
            command.Memo.TermsAccepted = false;

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Terms must be accepted."));
        }

        [TestMethod]
        public async Task ValidateAsync_NegativeCarat_ShouldHaveValidationError()
        {
            // Arrange
            var command = GetValidCreateMemoCommand();
            command.Memo.Products[0].Carat = -1; // Set negative value for Carat

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Product carat value cannot be negative."));
        }

        [TestMethod]
        public async Task ValidateAsync_NegativePrice_ShouldHaveValidationError()
        {
            // Arrange
            var command = GetValidCreateMemoCommand();
            command.Memo.Products[0].Price = -1; // Set negative value for Price

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Product price cannot be negative."));
        }

        private CreateMemoCommand GetValidCreateMemoCommand()
        {
            var memoDto = new MemoDTO
            {
                ConsignerId = "341743f0-asd2-42de-afbf-59kmkkmk72cf6",
                ConsigneeId = "02174cf0-9412-4cfe-afbf-59f706d72cf6",
                TermsAccepted = true,
                Products = new List<ProductDTO>
        {
            new ProductDTO
            {
                ID = 0,
                SalesConfirmationId = 0,
                ConsignerId = "341743f0-asd2-42de-afbf-59kmkkmk72cf6",
                ConsigneeId = "02174cf0-9412-4cfe-afbf-59f706d72cf6",
                LotNumber = "13245",
                Description = "valid memo",
                MemoId = 0,
                Carat = 123,
                Price = 123,
                Remarks = "for testing",
                Selected = false,
                State = ProductState.NONE,
                SuggestedPrice = 0
            }
        }
            };

            return new CreateMemoCommand(memoDto);
        }

    }
}







