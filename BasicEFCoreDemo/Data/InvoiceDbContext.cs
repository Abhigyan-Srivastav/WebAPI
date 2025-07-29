using BasicEfCoreDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicEfCoreDemo.Data
{
    public class InvoiceDbContext(DbContextOptions<InvoiceDbContext> options) : DbContext(options)
    {
        public DbSet<Invoice> Invoices => Set<Invoice>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    Id = Guid.Parse("7D8AC0DD-A509-45E6-96C8-64CA8E756C74"),
                    InvoiceNumber = "INV-001",
                    ContactName = "Iron Man",
                    Description = "Invoice for the first month",
                    Amount = 100,
                    InvoiceDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    DueDate = new DateTimeOffset(2023, 1, 15, 0, 0, 0, TimeSpan.Zero),
                    Status = InvoiceStatus.AwaitPayment
                },
                new Invoice
                {
                    Id = Guid.Parse("18AE515F-F96B-4D22-A069-F0FAD2A122D9"),
                    InvoiceNumber = "INV-002",
                    ContactName = "Captain America",

                    Description = "Invoice for the first month",
                    Amount = 200,
                    InvoiceDate = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
                    DueDate = new DateTimeOffset(2021, 1, 15, 0, 0, 0, TimeSpan.Zero),
                    Status = InvoiceStatus.AwaitPayment
                },
            new Invoice
            {
                Id = Guid.Parse("E2C797DA-DBF8-4D5F-8AF9-8B4E58CF390C"),
                InvoiceNumber = "INV-003",
                ContactName = "Thor",
                Description = "Invoice for the first month",
                Amount = 300,
                InvoiceDate = new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero),
                DueDate = new DateTimeOffset(2021, 1, 15, 0, 0, 0, TimeSpan.Zero),
                Status = InvoiceStatus.Draft
            }
                );
            //base.OnModelCreating(modelBuilder);
        }
    }
}
