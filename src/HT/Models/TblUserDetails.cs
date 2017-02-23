using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HT.Models
{
    public partial class TblUserDetails
    {
        public TblUserDetails()
        {
            TblLoginDetails = new HashSet<TblLoginDetails>();
        }

        public int UserId { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]

        public decimal Phone { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }

        public virtual ICollection<TblLoginDetails> TblLoginDetails { get; set; }
    }
}
