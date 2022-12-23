using AppCore.Interfaces;
using Infrastructure.DataContext;
using Infrastructure.Helper;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Tasks.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add database Sqlite
var connection = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<TaskContext>(option => option.UseSqlite(connection));

//add services
builder.Services.AddTransient<IProject, ProjectService>();
builder.Services.AddTransient<ITask, TaskService>();
builder.Services.AddTransient<ContextHelper>();
builder.Services.AddTransient<StateHelper>();

var app = builder.Build().MigrateDbContext<TaskContext>();

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
//dotnet ef --startup-project ..\Tasks\ migrations add Initial --context TaskContext
//dotnet ef --startup-project ..\Tasks\ database update --context TaskContext
