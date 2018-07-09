using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanzin.Entities
{
    public class AboutUs
    {
        public int Id { get; set; }
        [Required]
        [StringLength(5000)]
        [AllowHtml]
        public string description { get; set; }
    }
}
