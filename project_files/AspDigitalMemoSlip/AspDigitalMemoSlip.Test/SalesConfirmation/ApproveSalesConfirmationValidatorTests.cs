using AspDigitalMemoSlip.Application.CQRS.SalesConfirmations;
using AspDigitalMemoSlip.Application.CQRS.Validators.SalesConfirmation;
using AspDigitalMemoSlip.Application.Interfaces;
using DTOClassLibrary.DTO;
using DTOClassLibrary.DTO.SalesConfirmation;
using FluentValidation.TestHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Test.SalesConfirmation
{
    [TestClass]
    public class ApproveSalesConfirmationValidationTests
    {
        private readonly ApproveSalesConfirmationCommandValidator validator;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;

        public ApproveSalesConfirmationValidationTests()
        {
            mockUnitOfWork = new Mock<IUnitOfWork>();
            validator = new ApproveSalesConfirmationCommandValidator(mockUnitOfWork.Object);
        }


        [TestMethod]
        public void TestRoleInitiatorNotEmpty()
        {
            // Arrange
            var command = new ApproveSalesConfirmationCommand { RoleInitiator = "" };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.RoleInitiator);
        }

        [TestMethod]
        public void TestRoleInitiatorCorrect()
        {
            // Arrange
            var command = new ApproveSalesConfirmationCommand { RoleInitiator = "Consigner" };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(c => c.RoleInitiator);
        }

        

    }
}
