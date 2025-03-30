using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register application services
builder.Services.AddSingleton<HcaDeidentifier.Services.Interfaces.IHL7ParserService, HcaDeidentifier.Services.HL7ParserService>();
builder.Services.AddSingleton<HcaDeidentifier.Services.Interfaces.IDeIdentificationService, HcaDeidentifier.Services.DeIdentificationService>();
builder.Services.AddSingleton<HcaDeidentifier.Services.Interfaces.IFileProcessingService, HcaDeidentifier.Services.FileProcessingService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();