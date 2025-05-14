using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Project.Context;
using Project.Repositories;
using Project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    /* This option removes the nulls from the json.*/
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddDbContext<MasterContext>(options => options.UseSqlite("Data Source=vaultive.db"));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<SubscriptionRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<WatchHistoryRepository>();

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