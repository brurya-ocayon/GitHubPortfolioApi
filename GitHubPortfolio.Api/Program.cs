using GitHubPortfolio.Service.Configurations;
using GitHubPortfolio.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GitHubSettings>(builder.Configuration.GetSection("GitHubSettings"));
builder.Services.AddMemoryCache(); // הוספת MemoryCache

// רישום GitHubService ב-DI
builder.Services.AddScoped<GitHubService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();
//הרצה לדוגמא;
//http://localhost:5012/api/github/search?name=Render_app&language=JavaScript&user=YiskaAvramsky
//http://localhost:5012/api/github/portfolio