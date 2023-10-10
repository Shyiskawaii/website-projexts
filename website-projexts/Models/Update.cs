using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace website_projexts.Models
{
    public class Update
    {
        public int UpdateId { get; set; }
        public int ProjectID { get; set; }
        public Projects Project { get; set; }

        public string UpdateText {  get; set; }
        public DateTime UpdateTime { get; set; }
        public ICollection<Projects> Projects { get; set; }

    }
}