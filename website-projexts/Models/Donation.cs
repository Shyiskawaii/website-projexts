using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace website_projexts.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }

        public int ProjectID { get; set; }
        public Projects Project { get; set; }


        public int UserID { get; set; }
        public int Donated { get; set; }
        public string DonationText { get; set; }
        public DateTime DonationTime { get; set; }
        
    }
}