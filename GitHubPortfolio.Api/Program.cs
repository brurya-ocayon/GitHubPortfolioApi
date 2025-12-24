using GitHubPortfolio.Service.Configurations;
using GitHubPortfolio.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GitHubSettings>(builder.Configuration.GetSection("GitHubSettings"));
builder.Services.AddMemoryCache(); 


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
