using FluentValidation;
using LabEquipmentApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
});

builder.Services.AddDbContext<LabDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LabDbConnection")));

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Lab Equipment API v1"));
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
