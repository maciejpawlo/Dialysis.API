using Dialysis.API;
using Dialysis.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Applying migrations");
var webHost = new WebHostBuilder()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .UseStartup<ConsoleStartup>()
    .Build();
using (var context = (DialysisContext)webHost.Services.GetService(typeof(DialysisContext)))
{
    context.Database.Migrate();
}
Console.WriteLine("Done");