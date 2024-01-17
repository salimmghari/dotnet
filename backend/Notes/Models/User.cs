using System.ComponentModel.DataAnnotations;

namespace Notes.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id
        {
            get;
            set;
        }

        [StringLength(50)]
        [Required]
        public string? Username
        {
            get;
            set;
        }

        [StringLength(250)]
        [Required]
        public string? Password
        {
            get;
            set;
        }
    }
}
