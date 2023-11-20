using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace website_projexts.Models
{
    public class Category
    {
        [Key, Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string CategoryDescription { get; set; }

        public ICollection<Projects> Projects { get; set; }

        public string CategoryImage { get; set; }

        [NotMapped]
        public HttpPostedFileBase UploadImage { get; set; }

    }
}