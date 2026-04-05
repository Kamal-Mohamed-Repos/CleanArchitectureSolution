using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Clean Architecture layer registrations
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "CleanArchitecture API";
        document.Info.Version = "v1";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Global exception handler for validation errors
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = validationException.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage })
                .ToArray();

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Detail = string.Join("; ", errors.Select(e => e.ErrorMessage))
            });
        }
    });
});

// Serve the OpenAPI document at /openapi/v1.json
app.MapOpenApi();

// Serve Swagger UI at /swagger, pointing to the built-in OpenAPI endpoint
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "CleanArchitecture API v1");
});

app.MapControllers();

app.Run();