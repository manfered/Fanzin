using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanzin.Entities
{
    public class SlidePhoto : IPhoto
    {
        [ForeignKey("Slide")]
        public int SlidePhotoId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set;}

        public virtual Slide Slide { get; set; }
    }
}
