using Fanzin.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Fanzin.Web.DataContext
{
    public class CompanyServiceContextDb : DbContext
    {
        public CompanyServiceContextDb() : base("DefaultConnection")
        {

        }

        public DbSet<CompanyService> CompanyServices { get; set; }
        public DbSet<ServicePhotoTitle> ServicePhotoTitles { get; set; }
        public DbSet<CompanyServicePhoto> CompanyServicePhotos { get; set; }
    }
}