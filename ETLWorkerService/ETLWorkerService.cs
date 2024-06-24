using CsvHelper;
using CsvHelper.Configuration;
using ETLWorkerService.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLWorkerService
{
    internal class ETLWorkerService : BackgroundService
    {
        private readonly ILogger<ETLWorkerService> _logger;
        private readonly FileSettings _fileSettings;
        private readonly IServiceScopeFactory _scope;

        public ETLWorkerService(ILogger<ETLWorkerService> logger,
            IOptions<FileSettings> settings,
            IServiceScopeFactory scope) // DI
        {
            _logger = logger;
            _scope = scope;
            _fileSettings = settings.Value;
            //lazy load pattern itu kita panggil kalau pas kita butuh gitu
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("EtlWorkerService running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(2000, stoppingToken);
            }
        }
        private async void Process()
        {
            try
            {
                using (var scope = _scope.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ProductdbContext>();
                    var files = Directory.GetFiles(_fileSettings.CsvFolder, "*.csv");
                    foreach (var file in files)
                    {
                        Console.WriteLine("File: " + file);
                        //ProcessFile(file, db);
                        // baca file csv     
                        using (var reader = new StreamReader(file))
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            MissingFieldFound = null,
                            HeaderValidated = null,
                            HasHeaderRecord = true,
                        }))
                        {
                            var records = csv.GetRecords<Product>();

                            // simpan ke database
                            await db.Products.AddRangeAsync(records);
                            await db.SaveChangesAsync();
                        }
                        Console.WriteLine("Remove File: " + file);
                        File.Delete(file);

                    }
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }
            // baca files
            
        }
        // private async void ProcessFile(string file, ProductdbContext db)
        // {
        //     // baca file
        //     // simpan ke database
        //     // baca file csv     
        //     using (var reader = new StreamReader(file))
        //     using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        //     {
        //         MissingFieldFound = null,
        //         HeaderValidated = null,
        //         HasHeaderRecord = true,
        //     }))
        //     {
        //         var records = csv.GetRecords<Product>();

        //         // simpan ke database
        //         await db.Products.AddRangeAsync(records);
        //         await db.SaveChangesAsync();
        //     }
        //     // hapus file
        //     File.Delete(file);
        // }

    }
}
