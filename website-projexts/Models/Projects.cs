using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace website_projexts.Models
{
    public class Projects
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProjectID { get; set; }


        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category{ get; set; }


        public ICollection<Donation> Donations { get; set; }
        public ICollection<Update> Updates { get; set; }



        [Required(ErrorMessage = "Vui lòng nhập tên dự án!")]
        [StringLength(30, MinimumLength = 3,ErrorMessage ="Tên dự án không hợp lệ!")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tóm tắt dự án!")]
        [StringLength(50, MinimumLength = 3,ErrorMessage = "Tóm tắt dự án dài từ 3-50 chữ!")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thông tin dự án!")]
        [StringLength(2000, MinimumLength = 3,ErrorMessage = "Thông tin dự án dài từ 3-2000 chữ!")]
        public string LongDescription { get; set; } 
        [Required(ErrorMessage ="Vui lòng nhập mục tiêu gây quỹ!")]
        [Range(100000,100000000,ErrorMessage = "Mục tiêu gây quỹ từ 100k-100tr!")]
        public int Goal{ get; set; } 
        public int Raised { get; set; }
        public DateTime PostedTime { get; set; }

        public string ProjectImage { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }
    }
}