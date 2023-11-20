using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using website_projexts.Models;

namespace website_projexts.Context
{
    public class OurDBContext : DbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Donation> Donation { get; set; }
        public DbSet<Update> Update { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure the relationship between Users and Donations
            modelBuilder.Entity<Donation>()
                .HasRequired(d => d.User)
                .WithMany(u => u.Donations)
                .HasForeignKey(d => d.UserID)
                .WillCascadeOnDelete(false); // Specify ON DELETE NO ACTION

            base.OnModelCreating(modelBuilder);
        }
    }

}
