using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace BaseBallVR.Models
{
    public class Member :Base
    {
        [Key]
        [MaxLength(30)]
        public string Id { get; set; }

        [Required]
        public int reinforce { get; set; }

        [ForeignKey("UserId")]
        [MaxLength(30)]
        public User User { get; set; }

        [Key]
        [MaxLength(30)]
        public string UserId { get; set; }
    }
    
    public class MemberViewModel
    {
        public string Id { get; set; }
        public int reinforce { get; set; }
    }
}
