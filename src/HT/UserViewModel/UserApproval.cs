using System.ComponentModel.DataAnnotations;

namespace HT.UserViewModel
{
    public class UserApproval
    {
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
        public int loginid { get; set; }
    }
}
