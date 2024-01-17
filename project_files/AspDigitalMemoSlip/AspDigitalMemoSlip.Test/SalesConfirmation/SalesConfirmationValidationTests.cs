using AspDigitalMemoSlip.Application.Commands;
using AspDigitalMemoSlip.Application.CQRS.Validators.SalesConfirmation;
using AspDigitalMemoSlip.Application.Interfaces;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.ProductSale;
using FluentValidation.TestHelper;
using Moq;

namespace AspDigitalMemoSlip.Test.SalesConfirmation
{
    namespace AspDigitalMemoSlip.Test.SalesConfirmation
    {
        public class SalesConfirmationValidationTests
        {
            private readonly CreateSalesConfirmationCommandValidator validator;
            private readonly Mock<IUnitOfWork> mockUnitOfWork;

            public SalesConfirmationValidationTests()
            {
                mockUnitOfWork = new Mock<IUnitOfWork>();
                validator = new CreateSalesConfirmationCommandValidator(mockUnitOfWork.Object);
            }

            [TestMethod]
            public void TestConsigneeEmpty()
            {
                // Arrange
                var command = new CreateSalesConfirmationCommand { ConsigneeId = string.Empty };

                // Act
                var result = validator.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.ConsigneeId);
            }

            [TestMethod]
            public void TestSalesConfirmationEmpty()
            {
                // Arrange
                var command = new CreateSalesConfirmationCommand { SalesConfirmation = null };

                // Act
                var result = validator.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.SalesConfirmation);
            }

            [TestMethod]
            public void TestProductListEmpty()
            {
                // Arrange
                var command = new CreateSalesConfirmationCommand
                {
                    SalesConfirmation = new SalesConfirmationDTO { SoldProducts = new List<ProductSaleDTO>() }
                };

                // Act
                var result = validator.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.SalesConfirmation.SoldProducts);
            }

            [TestMethod]
            public void TestSellingCaratsAreUnderZero()
            {
                // Arrange
                var command = new CreateSalesConfirmationCommand
                {
                    SalesConfirmation = new SalesConfirmationDTO
                    {
                        SoldProducts = new List<ProductSaleDTO> { new ProductSaleDTO { CaratsSold = -10 } }
                    }
                };

                // Act
                var result = validator.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor("SalesConfirmation.SoldProducts[0].CaratsSold");
            }

            [TestMethod]
            public void TestCommisionOutOfRange()
            {
                // Arrange
                var command = new CreateSalesConfirmationCommand
                {
                    SalesConfirmation = new SalesConfirmationDTO { SuggestedCommision = 150 }
                };

                // Act
                var result = validator.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.SalesConfirmation.SuggestedCommision);
            }
        }
    }
}
