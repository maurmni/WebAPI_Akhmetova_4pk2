using WebAPI.Data;
using WebAPI.Repositories;
using WebAPI.Services;
using WebAPI.Mapping;
using Microsoft.EntityFrameworkCore;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CarRentalContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IRenterRepository, RenterRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IRenterService, RenterService>();
builder.Services.AddScoped<IRentalService, RentalService>();

builder.Services.AddLogging();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MiddleWare>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<CarRentalContext>();
        context.Database.EnsureCreated();
        Data.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при заполнении базы данных");
    }
}

app.Run();