using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace website_projexts.Models
{
    public class Update
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UpdateId { get; set; }
        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public virtual Projects Project { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên cập nhật!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Cập nhật dài từ 3-100 chữ!")]
        public string UpdateName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập cập nhật!")]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Cập nhật dài từ 3-2000 chữ!")]
        public string UpdateText {  get; set; }
        public DateTime UpdateTime { get; set; }

        public string UpdateImage { get; set; } 

        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }

    }
}