using PloggingAPI.Models;
using PloggingAPI.Repository;
using PloggingAPI.Repository.Interfaces;
using PloggingAPI.Services;
using PloggingAPI.Services.Interfaces;

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

var serviceAccountPath = Environment.GetEnvironmentVariable("SERVICEACCOUNT_GOOGLEDRIVE", EnvironmentVariableTarget.Process);
if(serviceAccountPath == null)
{
    serviceAccountPath = "/etc/secrets/serviceaccountgoogledrive.json";
}

builder.Services.Configure<GoogleDriveSettings>(options =>
{
    builder.Configuration.GetSection("GoogleDriveSettings").Bind(options);
    options.ServiceAccount = File.ReadAllText(serviceAccountPath);
    options.DirectoryId = "1Vc_R08GhzScbik-Y7RO0tj6id-ENMwSF";
});

//Register Services
builder.Services.AddSingleton<IPloggingSessionService, PloggingSessionService>();
builder.Services.AddSingleton<ILitterbagPlacementService, LitterbagPlacementService>();
builder.Services.AddSingleton<IStreakService, StreakService>();
builder.Services.AddTransient<GoogleDriveService>();

//Register Repositories
builder.Services.AddSingleton<IPloggingSessionRepository, PloggingSessionRepository>();
builder.Services.AddSingleton<ILitterLocationsRepository, LitterLocationsRepository>();
builder.Services.AddSingleton<ILitterbagRepository, LitterbagRepository>();
builder.Services.AddSingleton<IStreakRepository, StreakRepository>();

builder.Services.AddSingleton<IPlogTogetherRepository, PlogTogetherRepository>();

builder.Services.AddSingleton<IUserInfoRepository, UserInfoRepository>();

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
