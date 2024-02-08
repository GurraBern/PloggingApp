using PloggingAPI.Models;
using PloggingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var dbUrl = Environment.GetEnvironmentVariable("PLOGGINGDB_CONNECTIONSTRING", EnvironmentVariableTarget.Process);
if (dbUrl != null)
{
    builder.Services.Configure<PloggingDatabaseSettings>(options =>
    {
        builder.Configuration.GetSection("PloggingDatabaseSettings").Bind(options);
        options.ConnectionString = dbUrl;
    });
}
else
{
    builder.Services.Configure<PloggingDatabaseSettings>(builder.Configuration.GetSection("PloggingDatabaseSettings"));
}

//Register Services
builder.Services.AddSingleton<IRankingService, RankingService>();


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

//app.UseAuthorization();

app.MapControllers();

app.Run();
