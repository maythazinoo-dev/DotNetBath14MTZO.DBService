using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetBath14MTZO.ConsoleApp
{
    public class AppDbContext : DbContext
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;
        public AppDbContext()
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = ".",
                InitialCatalog = "WalletDB",
                UserID = "sa",
                Password = "mtzoo@123",
                TrustServerCertificate = true
            };

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_sqlConnectionStringBuilder.ConnectionString);

            }
        }

        public DbSet<BlogModel> Blogs { get; set; }
    }
}
