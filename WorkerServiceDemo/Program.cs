using WorkerServiceDemo;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<MyWorker>();

var host = builder.Build();
host.Run();
