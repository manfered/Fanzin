using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanzin.Entities
{
    public class ContactUs
    {
        public int ContactUsId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "وارد کردن نام الزامی است")]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "وارد کردن آدرس پست الکترونیکی الزامی است")]
        [EmailAddress(ErrorMessage = "پست الکترونیکی صحیح نمی باشد")]
        public string Email { get; set; }

        [Display(Name = "Telephone number")]
        [Required(ErrorMessage = "وارد کردن شماره تماس الزامی است")]
        [Phone(ErrorMessage = "شماره تماس صحیح نمی باشد")]
        public string Tel { get; set; }

        [Display(Name = "Message")]
        [Required(ErrorMessage = "پیغامی وارد نشده است")]
        [StringLength(500)]
        public string Message { get; set; }

        [Required]
        public bool Read { get; set; }

        [Display(Name = "Submit DateTime")]
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime SubmitDateTime { get; set; }
    }
}
