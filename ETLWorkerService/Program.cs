using ETLWorkerService;
using ETLWorkerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);


//baca configuration

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

//baca connection string
var connectionString = configuration.GetConnectionString("MyDB");

//DI dependnecy injection
builder.Services.AddDbContext<ProductdbContext>(options =>
    options.UseSqlServer(connectionString));
//var services = new ServiceCollection()
//    .AddDbContext<ProductdbContext>(options => options.UseSqlServer(connectionString))
//    .BuildServiceProvider(); inikan kemarin pakai console soalnya

//DI File Setting
// gak jadi var fileSettings = configuration.GetSection("FileSettings").Get<FileSettings>();
builder.Services.Configure<FileSettings>(configuration.GetSection("FileSettings"));

//services
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<ETLWorkerService.ETLWorkerService>();

var host = builder.Build();
host.Run();
