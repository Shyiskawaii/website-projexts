using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace website_projexts.Models
{
    public class User
    {
        
        public string UserRoles { get; set; }
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        
        [Required(ErrorMessage ="Vui lòng nhập tên của bạn!")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Phải dài từ 2-20 chữ cái!" )]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ của bạn!")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Phải dài từ 2-20 chữ cái!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Phải dài từ 3-30 chữ cái!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email của bạn")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu của bạn")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage ="Mật khẩu không hợp lệ!")]
        public string Password { get; set; }

        [NotMapped]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
        public string FullName()
        {
            return this.LastName + " " + this.FirstName;
        }


        public ICollection<Projects> Projects { get; set; }
        public ICollection<Donation> Donations { get; set; }

        public string UserImage { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }


    }
}