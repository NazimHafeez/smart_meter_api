using FluentValidation;
using MeterReadingApi.Models;
using MeterReadingApi.Repository;
using MeterReadingUploads.Mappers;
using MeterReadingUploads.Models;
using MeterReadingUploads.Repository;
using MeterReadingUploads.Services;
using MeterReadingUploads.SourceDataAdaptors;
using MeterReadingUploads.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add an in memory database using entity framework db context/EF Core


builder.Services.AddDbContext<MeterReadingContext>(options => options.UseInMemoryDatabase("MeterReadings"), ServiceLifetime.Singleton);
builder.Services.AddSingleton<IRepository<MeterReading>, MeterReadingRepository>();
builder.Services.AddSingleton<IRepository<Account>, AccountRepository>();
builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();
builder.Services.AddScoped<ISourceDataAdapter<CsvFileAdapterOption, string[]>, CsvFileAdapter>();
builder.Services.AddScoped<IMapper<string[], Account>, StringArrayToAccountsMap>();
builder.Services.AddScoped<IMapper<string[], MeterReading>, StringArrayToMeterReading>();
builder.Services.AddScoped<IDBInitializer, DBInitializerService>(serviceProvider => new DBInitializerService(serviceProvider.GetRequiredService<MeterReadingContext>(), serviceProvider.GetRequiredService<ISourceDataAdapter<CsvFileAdapterOption, string[]>>(), serviceProvider.GetRequiredService<IMapper<string[], Account>>()));
builder.Services.AddValidatorsFromAssemblyContaining<CsvFileAdapterValidator>();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await SeedData(app);

app.UseHttpsRedirection();

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();

static async Task SeedData(WebApplication application)
{
    using (var scope = application.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var dbInitializer = services.GetRequiredService<IDBInitializer>();
        await dbInitializer.InitializeAsync();
    }

}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
