using AspDigitalMemoSlip.Application.CQRS.SalesConfirmations;
using AspDigitalMemoSlip.Application.CQRS.Validators.SalesConfirmation;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.ProductSale;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Test.SalesConfirmation
{
    public class UpdateSalesConfirmationValidatorTests
    {
        private readonly UpdateSalesConfirmationValidator validator;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;

        public UpdateSalesConfirmationValidatorTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            validator = new UpdateSalesConfirmationValidator(mockUnitOfWork.Object);
        }

        public void TestSalesConfirmationDTOIsNull()
        {
            // Arrange
            var command = new UpdateSalesConfirmation { SalesConfirmationDTO = null };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.SalesConfirmationDTO);
        }


        public void TestIfSalesConfirmationIDisNull()
        {
            // Arrange
            var command = new UpdateSalesConfirmation { SalesConfirmationDTO = new SalesConfirmationDTO { Id = 0 } };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.SalesConfirmationDTO.Id);
        }

        public void TestIfRoleIsValid()
        {
            // Arrange
            var command = new UpdateSalesConfirmation { RoleInitiator = "InvalidRole" };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.RoleInitiator);
        }


        public void TestIfUpdatedProductSaleExists()
        {
            // Arrange
            mockUnitOfWork.Setup(uow => uow.ProductSaleRepository.GetById(It.IsAny<int>()))
                          .ReturnsAsync((ProductSale)null);
            var command = new UpdateSalesConfirmation
            {
                SalesConfirmationDTO = new SalesConfirmationDTO
                {
                    SoldProducts = new List<ProductSaleDTO> { new ProductSaleDTO { Id = 1 } }
                }
            };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor("SalesConfirmationDTO.SoldProducts[0]");
        }
    }
}
