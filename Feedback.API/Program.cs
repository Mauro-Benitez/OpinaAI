using Feedback.Application.Interfaces;
using Feedback.Application.Interfaces.Services;
using Feedback.Application.Services;
using Feedback.Application.Services_Implementation;
using Feedback.Domain.Repositories;
using Feedback.Infrastructure.Context;
using Feedback.Infrastructure.Persistence;
using Feedback.Infrastructure.Services;
using Feedback.Infrastructure.Workers;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

//CORS
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          // Permite requisições do front-end React
                          policy.WithOrigins("http://localhost:3000")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


//Config EF
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

//injeção de dependência
builder.Services.AddScoped<IFeedbackNpsRepository, FeedbackNpsRepository>();
builder.Services.AddScoped<IFeedbackNpsService, FeedbackNpsService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISentimentAnalysisService, OpenAISentimentAnalysisService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<NpsCalculatorService>();





//Add Workers como hosted service
builder.Services.AddHostedService<NpsProcessingWorker>();
builder.Services.AddHostedService<SentimentAnalysisWorker>();



//Add NpsProcessingWorker como hosted service
builder.Services.AddHostedService<NpsProcessingWorker>();

//Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
