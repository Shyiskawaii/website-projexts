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

        [Required(ErrorMessage = "Project Name is required")]
        [StringLength(20, MinimumLength = 3)]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "Short Description is required")]
        [StringLength(50, MinimumLength = 3)]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Long Description is required")]
        [StringLength(2000, MinimumLength = 3)]
        public string LongDescription { get; set; } 
        [Range(100000,100000000,ErrorMessage = "100k - 100m")]
        public int Goal{ get; set; } 
        public int Raised { get; set; }
        //public Image ProjectImage { get; set; }
        public DateTime PostedTime { get; set; }


    }
}