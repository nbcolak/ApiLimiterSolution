using ApiLimiter.Decorator;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
// Redis Configuration
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("localhost:6379", true);
    return ConnectionMultiplexer.Connect(configuration);
});
//with scrutor
/*builder.Services.AddScoped<IRateLimiter, BasicRateLimiter>()
    .Decorate<IRateLimiter, LoggingRateLimiterDecorator>()
    .Decorate<IRateLimiter, WithIPRateLimiterDecorator>();*/


builder.Services.AddSingleton<IRateLimiter>(sp =>
{
    var redis = sp.GetRequiredService<IConnectionMultiplexer>();
    IRateLimiter basicRateLimiter = new BasicRateLimiter(redis,
        requestLimit: 5,
        timeSpan: TimeSpan.FromMinutes(1));
    
    IRateLimiter rateLimiterWithLogging = new LoggingRateLimiterDecorator(basicRateLimiter);
    IRateLimiter rateLimiterWithIP = new WithIPRateLimiterDecorator(rateLimiterWithLogging);
    
    return rateLimiterWithIP; 
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

