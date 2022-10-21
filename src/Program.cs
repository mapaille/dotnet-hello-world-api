using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/ready");
app.MapHealthChecks("/live", new HealthCheckOptions { Predicate = _ => false });

app.MapGet("/hello-world", async (HttpResponse response) =>
{
    response.ContentType = MediaTypeNames.Text.Plain;
    await response.WriteAsync($"Hello World from {response.HttpContext.Request.Host.Value}");
}).WithName("HelloWorld");

app.Run();
