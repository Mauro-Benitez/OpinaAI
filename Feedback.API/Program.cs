using Feedback.Application.Interfaces;
using Feedback.Application.Services;
using Feedback.Domain.Repositories;
using Feedback.Infrastructure.Context;
using Feedback.Infrastructure.Persistence;
using Feedback.Infrastructure.Workers;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

//EF
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IFeedbackNpsRepository, FeedbackNpsRepository>();
builder.Services.AddScoped<IFeedbackNpsService, FeedbackNpsService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


//Add NpsProcessingWorker como hosted service
builder.Services.AddHostedService<NpsProcessingWorker>();

// Add services to the container.

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

app.UseAuthorization();

app.MapControllers();

app.Run();
