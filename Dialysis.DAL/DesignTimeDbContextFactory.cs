using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Dialysis.DAL
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DialysisContext>
    {
        public DialysisContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.dal.json")
                    .Build();

            var builder = new DbContextOptionsBuilder<DialysisContext>();

            var connectionString = configuration
                        .GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new DialysisContext(builder.Options);
        }
    }
}
