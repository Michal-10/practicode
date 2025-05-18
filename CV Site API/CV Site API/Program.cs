using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Service; // Ensure this namespace exists in your project  

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("*") // כתובת של React שלך
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// הוספת קונפיגורציה ל-secrets  
//builder.Configuration.AddUserSecrets<Program>();  


// רישום השירות בצורה נכונה ל-DI  
builder.Configuration.AddUserSecrets<Program>();

// Bind GitHub settings
builder.Services.Configure<GitHubSettings>(builder.Configuration.GetSection("GitHub"));

builder.Services.AddSingleton<IGitHubService, GitHubService>();
builder.Services.AddMemoryCache();


builder.Services.AddSingleton<IGitHubService, GitHubService>();
builder.Services.AddMemoryCache();

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

// הפעלת ה-CORS
app.UseCors("AllowReact");

app.UseAuthorization();

app.MapControllers();


app.Run();
