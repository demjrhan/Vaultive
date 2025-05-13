using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Repositories;
using Project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<MasterContext>(options => options.UseSqlite("Data Source=vaultive.db"));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<SubscriptionConfirmationRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();
SampleData.Initialize(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();
app.Run();