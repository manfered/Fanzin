using Fanzin.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Fanzin.Web.DataContext
{
    public class CompanyMemberContextDb : DbContext
    {
        public CompanyMemberContextDb() : base("DefaultConnection")
        {

        }

        public DbSet<CompanyMember> CompanyMembers { get; set; }
        public DbSet<CompanyMemberPhoto> CompanyMemberPhotoes { get; set; }
    }
}