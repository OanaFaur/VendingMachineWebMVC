using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class DataContext:DbContext
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<ShoppingBasketItem> ShoppingBasketItems { get; set; }
        public DbSet<AdminUser> AdminUser { get; set; }

        public DataContext()
        {

        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = VendingMachineDB; Trusted_Connection = True; ");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminUser>(b =>
            {
                b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("FirstName");

                b.Property<string>("SecondName");

                b.Property<string>("Password");
                
                b.Property<string>("Username");

                b.HasKey("Id");

                b.ToTable("AdminUser");


            });

            modelBuilder.Entity("DataAccess.Models.Products", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("Image");

                b.Property<int>("ItemsLeft");

                b.Property<int>("ItemsSold");

                b.Property<string>("Name");

                b.Property<double>("Price");

                b.HasKey("Id");

                b.ToTable("Products");
            });

        }
        
    }
}
