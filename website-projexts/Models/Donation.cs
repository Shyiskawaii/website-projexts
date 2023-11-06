using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace website_projexts.Models
{
    public class Donation
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DonationID { get; set; }

        public int ProjectID { get; set; }
        [ForeignKey("ProjectID")]
        public virtual Projects Project { get; set; }


        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số tiền gây quỹ!")]
        [Range(10000, 100000000, ErrorMessage = "Số tiền gây quỹ ít nhất 10k!")]
        public int Donated { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lời ủng hộ!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Lời ủng hộ dài từ 3-50 chữ!")]
        public string DonationText { get; set; }

        public DateTime DonationTime { get; set; }
        
    }
}