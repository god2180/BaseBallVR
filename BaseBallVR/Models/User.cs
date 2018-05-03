using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BaseBallVR.Models
{
    public class User : Base
    {
        [Key]
        [MaxLength(30)]
        public string Id { get; set; }
                
        [Required]
        [MaxLength(30)]
        public string TeamName { get; set; }
                
        public int MMR { get; set; }

        public IList<Member> Members { get; set; } = new List<Member>();
        
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string TeamName { get; set; }
        public int MMR { get; set; }

        public IList<MemberViewModel> Members { get; set; } = new List<MemberViewModel>();
    }
}
