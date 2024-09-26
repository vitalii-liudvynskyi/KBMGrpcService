using KBMGrpcService.Repositories.Context;
using KBMGrpcService.Controllers;
using Microsoft.EntityFrameworkCore;
using KBMGrpcService.Repositories.Interfaces;
using KBMGrpcService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<KBMDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

// Configure the HTTP request pipeline.;
app.MapGrpcService<OrganizationController>();
app.MapGrpcService<UserController>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
