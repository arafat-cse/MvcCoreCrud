using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopManage.Models
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        //[Required, Display(Name = "Student Name")]
        public string? UserName { get; set; }
        //[Required]
        public int Age { get; set; }
        //[Required]
        public DateTime Date { get; set; }
        //[Required]
        public bool IsActive { get; set; }
        //[Required]
        public string? ImagePath { get; set; }
        public virtual IList<Detail>? Details { get; set; }

    }
}
