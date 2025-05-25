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
    .AddJsonOptions(options =>
    {
        /* This option removes the nulls from the json.*/
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
builder.Services.AddDbContext<MasterContext>(options => options.UseSqlite("Data Source=vaultive.db"));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<WatchHistoryRepository>();

builder.Services.AddScoped<MediaContentRepository>();
builder.Services.AddScoped<MediaContentService>();

builder.Services.AddScoped<StreamingServiceRepository>();
builder.Services.AddScoped<StreamingServiceService>();

builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<SubscriptionRepository>();
builder.Services.AddScoped<SubscriptionConfirmationRepository>();


builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<ReviewRepository>();


var app = builder.Build();
SampleData.Initialize(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();