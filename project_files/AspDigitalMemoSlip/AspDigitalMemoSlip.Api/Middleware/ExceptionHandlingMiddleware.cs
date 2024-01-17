using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Exceptions.Authentication;
using AspDigitalMemoSlip.Application.Exceptions.Consignee;
using AspDigitalMemoSlip.Application.Exceptions.Consigner;
using AspDigitalMemoSlip.Application.Exceptions.Email;
using AspDigitalMemoSlip.Application.Exceptions.Memo;
using AspDigitalMemoSlip.Application.Exceptions.Multi_Factor;
using DTOClassLibrary.DTO.ErrorHandling;
using System.Text.Json;
using ValidationException = FluentValidation.ValidationException;

namespace AspDigitalMemoSlip.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var response = new ErrorResponse();
                response.Message = ex.Message;

                switch (ex)
                {
                    case UserAlreadyExistsException:
                        response.StatusCode = StatusCodes.Status409Conflict;
                        break;
                    case InvalidVatNumberException:
                    case UserCreationFailedException:
                    case UserTypeNotRecognizedException:
                    case ValidationException:
                    case InvalidPasswordException:
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case ConsignerNotFoundException:
                    case UserNotFoundException:
                    case MemoNotFoundException:
                    case InvalidOTCodeException:
                    case FileNotFoundException:
                        response.StatusCode = StatusCodes.Status404NotFound;
                        break;
                    case UserValidationFailedException:
                        response.StatusCode = StatusCodes.Status401Unauthorized;
                        break;
                    case RoleUrlCheckFailedException:
                        response.StatusCode = StatusCodes.Status403Forbidden;
                        break;
                    case VatValidationFailedExpection:
                    case ClaimsGenerationFailedException:
                    case EmailSendingException:
                    case UpdateFailedException:
                    case QrCodeGenerationException:
                    case TotpGenerationException:
                    case ConsigneeDeletionFailedException:
                    case ConfigurationNotFoundException:
                    default:
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                httpContext.Response.StatusCode = response.StatusCode;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));

            }
        }
    }
}
