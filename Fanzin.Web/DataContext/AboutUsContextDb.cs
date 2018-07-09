using Fanzin.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Fanzin.Web.DataContext
{
    public class AboutUsContextDb : DbContext
    {
        public AboutUsContextDb() : base("DefaultConnection")
        {

        }
        
        public DbSet<AboutUs> AboutUses { get; set; }
    }
}