namespace HT.Models
{
    public partial class TblLoginDetails
    {
        public int LoginId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IsApproved { get; set; }
        public string IsAdmin { get; set; }
        public int UserId { get; set; }

        public virtual TblUserDetails User { get; set; }
    }
}
