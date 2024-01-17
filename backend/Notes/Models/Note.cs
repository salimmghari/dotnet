using System.ComponentModel.DataAnnotations;

namespace Notes.Models
{
    public class Note
    {
        [Key]
        [Required]
        public int Id
        {
            get;
            set;
        }

        [StringLength(100)]
        [Required]
        public string? Title
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        } = "";

        public User? User
        {
            get;
            set;
        }
    }
}
