using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

var hostname = app.Configuration.GetValue<string>("HOSTNAME");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/ready");
app.MapHealthChecks("/live", new HealthCheckOptions { Predicate = _ => false });

app.MapGet("/hello-world", () => $"Hello World from {hostname}").WithName("HelloWorld");

app.Run();
