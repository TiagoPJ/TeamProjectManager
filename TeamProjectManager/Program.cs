using AutoMapper;
using Domain.AutoMapper;
using Domain.Interface;
using Domain.Interface.Generic;
using Domain.Services;
using FluentValidation.AspNetCore;
using Infrastructure.Configuration;
using Infrastructure.Repository;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using Shared;
using TeamProjectManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TeamProjectManagerDatabaseConnection"));
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers().AddFluentValidation(conf => { conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly); });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(typeof(IGenericInterface<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskHistoryService, TaskHistoryService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITaskCommentService, TaskCommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGenericProjectInterface, ProjectRepository>();
builder.Services.AddScoped<IGenericTaskHistoryInterface, TaskHistoryRepository>();
builder.Services.AddScoped<IGenericUserInterface, UserRepository>();
builder.Services.AddScoped<IGenericTaskInterface, TaskRepository>();
builder.Services.AddScoped<IGenericReportInterface, ReportRepository>();
builder.Services.AddScoped<IGenericTaskCommentInterface, TaskCommentRepository>();

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new EntityToDto());
});

builder.Services.AddSingleton(mappingConfig.CreateMapper());

builder.Services.Configure<Variables>(builder.Configuration.GetSection("Variables"));

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
//Initial data Migrations
app.Seed();
app.Run();
