using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VendorBookingProcessor.Services;
using VendorBookingProcessor.Interfaces;
using VendorBookingProcessor.Validators;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddSingleton<IXmlParserService, XmlParserService>()
    .AddSingleton<BookingValidator>();

builder.Build().Run();