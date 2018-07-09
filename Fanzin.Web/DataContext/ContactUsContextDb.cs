using Fanzin.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Fanzin.Web.DataContext
{
    public class ContactUsContextDb : DbContext
    {
        public ContactUsContextDb() : base("DefaultConnection")
        {

        }

        public DbSet<ContactUs> ContactUses { get; set; }
    }
}