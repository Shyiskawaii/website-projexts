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

    }

}