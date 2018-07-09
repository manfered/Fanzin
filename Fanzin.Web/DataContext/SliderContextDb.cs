using Fanzin.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Fanzin.Web.DataContext
{
    public class SliderContextDb : DbContext
    {
        public SliderContextDb() : base("DefaultConnection")
        {

        }

        public DbSet<Slide> Slides { get; set; }
        public DbSet<SlidePhoto> SlidePhotos { get; set; }
    }
}