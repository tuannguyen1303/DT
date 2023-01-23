using System.Text;
using System.Text.Json;
using DigitalTwin.Common.Constants;
using DigitalTwin.Common.Exceptions;
using DigitalTwin.Models.Responses;
using DigitalTwin.Models.Responses.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace DigitalTwin.Api.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseFluentValidationExceptionHandler(this IApplicationBuilder builder)
        {
            builder.UseExceptionHandler(app =>
            {
                app.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature != null)
                    {
                        var exception = errorFeature.Error;
                        if (!(exception is ValidationException validationException))
                        {
                            // Let other middleware handle this exception
                            throw exception;
                        }
                        var errors = validationException
                        .Errors
                        .Select(e => new BaseResponseException
                        {
                            ErrorCode = e.ErrorCode,
                            Message = e.ErrorMessage,
                        });
                        var errorResponse = Response.CreateResponse(null);
                        errorResponse.Errors = errors;
                        errorResponse.StatusCode = ErrorCodes.BadRequest;
                        errorResponse.StatusText = "BadRequest";
                        var errorText = JsonSerializer.Serialize(errorResponse);
                        context.Response.StatusCode = errorResponse.StatusCode;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(errorText, Encoding.UTF8);
                    }
                });
            });
        }

        public static void UseCustomExceptionHandler(this IApplicationBuilder builder, IWebHostEnvironment env)
        {
            builder.UseExceptionHandler(app =>
            {
                app.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature != null)
                    {
                        var exception = errorFeature.Error;
                        var errorResponse = Response.CreateResponse(null);
                        var errors = new List<BaseResponseException>();
                        if (exception is ResponseException responseException)
                        {
                            errors.Add(
                                new BaseResponseException
                                {
                                    ErrorCode = responseException.StatusCode.ToString(),
                                    Message = responseException.Message,
                                    ErrorData = responseException.ExceptionAdditionalData
                                });

                            errorResponse.StatusCode = responseException.StatusCode;
                            errorResponse.StatusText = responseException.StatusText;
                        }
                        else
                        {
                            // Unhandled Exception
                            errors.Add(
                                new BaseResponseException
                                {
                                    ErrorCode = ErrorCodes.InternalServerErrors.ToString(),
                                    Message = exception.Message,
                                    FullMessage = env.IsDevelopment() ? exception.StackTrace : null
                                });

                            errorResponse.StatusCode = ErrorCodes.InternalServerErrors;
                            errorResponse.StatusText = "InternalServerErrors";
                        }
                        errorResponse.Errors = errors;
                        var errorText = JsonSerializer.Serialize(errorResponse);
                        context.Response.StatusCode = errorResponse.StatusCode;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(errorText, Encoding.UTF8);
                    }
                });
            });
        }
    }
}
