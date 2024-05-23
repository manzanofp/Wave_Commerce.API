using Wave.Commerce.DependencyInjection;
using Wave.Commerce.Persistence.StartupService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IApplicationBuilder, ApplicationBuilder>();
builder.Services.AddServices(builder.Configuration);
var app = builder.Build();

await new EntityFrameworkCoreMigrator(app.Services).ApplyMigrationsWithRetryAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
